using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LivingDocumentation
{
    internal class InvocationsAnalyzer : CSharpSyntaxWalker
    {
        private readonly SemanticModel semanticModel;
        private readonly List<Statement> statements;

        public InvocationsAnalyzer(in SemanticModel semanticModel, List<Statement> statements)
        {
            this.semanticModel = semanticModel;
            this.statements = statements;
        }

        public override void VisitObjectCreationExpression(ObjectCreationExpressionSyntax node)
        {
            string containingType = this.semanticModel.GetTypeDisplayString(node);

            var invocation = new InvocationDescription(containingType, node.Type.ToString());
            this.statements.Add(invocation);

            if (node.ArgumentList != null)
            {
                foreach (var argument in node.ArgumentList.Arguments)
                {
                    var argumentDescription = new ArgumentDescription(this.semanticModel.GetTypeDisplayString(argument.Expression), argument.Expression.ToString());
                    invocation.Arguments.Add(argumentDescription);
                }
            }

            if (node.Initializer != null)
            {
                foreach (var expression in node.Initializer.Expressions)
                {
                    var argumentDescription = new ArgumentDescription(this.semanticModel.GetTypeDisplayString(expression), expression.ToString());
                    invocation.Arguments.Add(argumentDescription);
                }
            }

            base.VisitObjectCreationExpression(node);
        }

        public override void VisitSwitchStatement(SwitchStatementSyntax node)
        {
            var branchingAnalyzer = new BranchingAnalyzer(this.semanticModel, this.statements);
            branchingAnalyzer.Visit(node);
        }

        public override void VisitIfStatement(IfStatementSyntax node)
        {
            var branchingAnalyzer = new BranchingAnalyzer(this.semanticModel, this.statements);
            branchingAnalyzer.Visit(node);
        }

        public override void VisitForEachStatement(ForEachStatementSyntax node)
        {
            var loopingAnalyzer = new LoopingAnalyzer(this.semanticModel, this.statements);
            loopingAnalyzer.Visit(node);
        }

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            var expression = this.GetExpressionWithSymbol(node);

            if (Program.RuntimeOptions.VerboseOutput && this.semanticModel.GetSymbolInfo(expression).Symbol == null)
            {
                Console.WriteLine("WARN: Could not resolve type of invocation of the following block:");
                Console.WriteLine(node.ToFullString());
                return;
            }

            if (this.semanticModel.GetConstantValue(node).HasValue && string.Equals((node.Expression as IdentifierNameSyntax)?.Identifier.ValueText, "nameof", StringComparison.Ordinal))
            {
                // nameof is compiler sugar, and is actually a method we are not interrested in
                return;
            }

            var containingType = this.semanticModel.GetSymbolInfo(expression).Symbol?.ContainingSymbol.ToDisplayString();
            if (containingType == null)
            {
                containingType = this.semanticModel.GetSymbolInfo(expression).CandidateSymbols.FirstOrDefault()?.ContainingSymbol.ToDisplayString();
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
            this.statements.Add(invocation);

            foreach (var argument in node.ArgumentList.Arguments)
            {
                var argumentDescription = new ArgumentDescription(this.semanticModel.GetTypeDisplayString(argument.Expression), argument.Expression.ToString());
                invocation.Arguments.Add(argumentDescription);
            }

            base.VisitInvocationExpression(node);
        }

        private ExpressionSyntax GetExpressionWithSymbol(InvocationExpressionSyntax node)
        {
            var expression = node.Expression;

            if (this.semanticModel.GetSymbolInfo(expression).Symbol == null)
            {
                // This might be part of a chain of extention methods (f.e. Fluent API's), the symbols are only available at the beginning of the chain.
                var pNode = (SyntaxNode)node;

                while (pNode != null && (pNode is not InvocationExpressionSyntax || (pNode is InvocationExpressionSyntax && (this.semanticModel.GetTypeInfo(pNode).Type?.Kind == SymbolKind.ErrorType || this.semanticModel.GetSymbolInfo(expression).Symbol == null))))
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
            var returnDescription = new ReturnDescription(node.Expression?.ToString() ?? string.Empty);
            this.statements.Add(returnDescription);

            base.VisitReturnStatement(node);
        }

        public override void VisitArrowExpressionClause(ArrowExpressionClauseSyntax node)
        {
            var returnDescription = new ReturnDescription(node.Expression.ToString());
            this.statements.Add(returnDescription);

            base.VisitArrowExpressionClause(node);
        }

        public override void VisitAssignmentExpression(AssignmentExpressionSyntax node)
        {
            var assignmentDescription = new AssignmentDescription(node.Left.ToString(), node.OperatorToken.Text, node.Right.ToString());
            this.statements.Add(assignmentDescription);

            base.VisitAssignmentExpression(node);
        }
    }
}
