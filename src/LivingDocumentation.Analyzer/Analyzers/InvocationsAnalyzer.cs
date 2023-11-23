namespace LivingDocumentation;

internal class InvocationsAnalyzer(SemanticModel semanticModel, List<Statement> statements) : CSharpSyntaxWalker
{
    public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
    {
        string containingType = semanticModel.GetTypeDisplayString(node);

        var invocation = new InvocationDescription(containingType, node.Type.ToString());
        statements.Add(invocation);

        if (node.ArgumentList != null)
        {
            foreach (var argument in node.ArgumentList.Arguments)
            {
                var argumentDescription = new ArgumentDescription(semanticModel.GetTypeDisplayString(argument.Expression), argument.Expression.ToString());
                invocation.Arguments.Add(argumentDescription);
            }
        }

        if (node.Initializer != null)
        {
            foreach (var expression in node.Initializer.Expressions)
            {
                var argumentDescription = new ArgumentDescription(semanticModel.GetTypeDisplayString(expression), expression.ToString());
                invocation.Arguments.Add(argumentDescription);
            }
        }

        base.VisitObjectCreationExpression(node);
    }

    public override void VisitSwitchStatement(SwitchStatementSyntax node)
    {
        var branchingAnalyzer = new BranchingAnalyzer(semanticModel, statements);
        branchingAnalyzer.Visit(node);
    }

    public override void VisitIfStatement(IfStatementSyntax node)
    {
        var branchingAnalyzer = new BranchingAnalyzer(semanticModel, statements);
        branchingAnalyzer.Visit(node);
    }

    public override void VisitForEachStatement(ForEachStatementSyntax node)
    {
        var loopingAnalyzer = new LoopingAnalyzer(semanticModel, statements);
        loopingAnalyzer.Visit(node);
    }

    public override void VisitInvocationExpression(InvocationExpressionSyntax node)
    {
        var expression = this.GetExpressionWithSymbol(node);

        if (Program.RuntimeOptions.VerboseOutput && semanticModel.GetSymbolInfo(expression).Symbol == null)
        {
            Console.WriteLine("WARN: Could not resolve type of invocation of the following block:");
            Console.WriteLine(node.ToFullString());
            return;
        }

        if (semanticModel.GetConstantValue(node).HasValue && string.Equals((node.Expression as IdentifierNameSyntax)?.Identifier.ValueText, "nameof", StringComparison.Ordinal))
        {
            // nameof is compiler sugar, and is actually a method we are not interrested in
            return;
        }

        var containingType = semanticModel.GetSymbolInfo(expression).Symbol?.ContainingSymbol.ToDisplayString();
        if (containingType == null)
        {
            containingType = semanticModel.GetSymbolInfo(expression).CandidateSymbols.FirstOrDefault()?.ContainingSymbol.ToDisplayString();
        }

        var methodName = string.Empty;

        switch (node.Expression)
        {
            case MemberAccessExpressionSyntax m:
                methodName = m.Name.ToString();
                break;
            case IdentifierNameSyntax i:
                methodName = i.Identifier.ValueText;
                break;
        }

        var invocation = new InvocationDescription(containingType, methodName);
        statements.Add(invocation);

        foreach (var argument in node.ArgumentList.Arguments)
        {
            var value = argument.Expression.ResolveValue(semanticModel);

            var argumentDescription = new ArgumentDescription(semanticModel.GetTypeDisplayString(argument.Expression), value);
            invocation.Arguments.Add(argumentDescription);
        }

        base.VisitInvocationExpression(node);
    }

    private ExpressionSyntax GetExpressionWithSymbol(InvocationExpressionSyntax node)
    {
        var expression = node.Expression;

        if (semanticModel.GetSymbolInfo(expression).Symbol == null)
        {
            // This might be part of a chain of extention methods (f.e. Fluent API's), the symbols are only available at the beginning of the chain.
            var pNode = (SyntaxNode)node;

            while (pNode != null && (pNode is not InvocationExpressionSyntax || (pNode is InvocationExpressionSyntax && (semanticModel.GetTypeInfo(pNode).Type?.Kind == SymbolKind.ErrorType || semanticModel.GetSymbolInfo(expression).Symbol == null))))
            {
                pNode = pNode.Parent;

                if (pNode is InvocationExpressionSyntax syntax)
                {
                    expression = syntax.Expression;
                }
            }
        }

        return expression;
    }

    public override void VisitReturnStatement(ReturnStatementSyntax node)
    {
        var returnDescription = new ReturnDescription(node.Expression?.ResolveValue(semanticModel) ?? string.Empty);
        statements.Add(returnDescription);

        base.VisitReturnStatement(node);
    }

    public override void VisitArrowExpressionClause(ArrowExpressionClauseSyntax node)
    {
        var returnDescription = new ReturnDescription(node.Expression.ResolveValue(semanticModel));
        statements.Add(returnDescription);

        base.VisitArrowExpressionClause(node);
    }

    public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
    {
        var assignmentDescription = new AssignmentDescription(node.Left.ToString(), node.OperatorToken.Text, node.Right.ToString());
        statements.Add(assignmentDescription);

        base.VisitAssignmentExpression(node);
    }
}
