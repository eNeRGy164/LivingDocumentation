using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LivingDocumentation
{
    internal class InvocationsAnalyzer : CSharpSyntaxWalker
    {
        private readonly SemanticModel semanticModel;
        private readonly IList<Statement> statements;

        public InvocationsAnalyzer(in SemanticModel semanticModel, IList<Statement> statements)
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
            if (Program.RuntimeOptions.VerboseOutput && this.semanticModel.GetTypeInfo(node).Type.Kind == SymbolKind.ErrorType)
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

            string containingType = this.semanticModel.GetSymbolInfo(node.Expression).Symbol?.ContainingSymbol.ToDisplayString();
            if (containingType == null)
            {
                containingType = this.semanticModel.GetSymbolInfo(node.Expression).CandidateSymbols.FirstOrDefault()?.ContainingSymbol.ToDisplayString();
            }

            string methodName = string.Empty;

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
