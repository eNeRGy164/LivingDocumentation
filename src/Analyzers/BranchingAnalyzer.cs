using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace roslyn_uml
{
    internal class BranchingAnalyzer : CSharpSyntaxWalker
    {
        private readonly SemanticModel semanticModel;
        private readonly IList<Statement> statements;

        public BranchingAnalyzer(in SemanticModel semanticModel, IList<Statement> statements)
        {
            this.semanticModel = semanticModel;
            this.statements = statements;
        }

        public override void VisitIfStatement(IfStatementSyntax node)
        {
            var ifStatement = new If();
            statements.Add(ifStatement);

            var ifSection = new IfElseSection();
            ifStatement.Sections.Add(ifSection);

            ifSection.Condition = node.Condition.ToString();

            var ifInvocationAnalyzer = new InvocationsAnalyzer(semanticModel, ifSection.Statements);
            ifInvocationAnalyzer.Visit(node.Statement);

            var elseNode = node.Else;
            while (elseNode != null)
            {
                var section = new IfElseSection();
                ifStatement.Sections.Add(section);

                var elseInvocationAnalyzer = new InvocationsAnalyzer(semanticModel, section.Statements);
                elseInvocationAnalyzer.Visit(elseNode.Statement);

                if (elseNode.Statement.IsKind(SyntaxKind.IfStatement))
                {
                    var elseIfNode = (IfStatementSyntax)elseNode.Statement;
                    section.Condition = elseIfNode.Condition.ToString();

                    elseNode = elseIfNode.Else;
                }
                else
                {
                    elseNode = null;
                }
            }
        }

        public override void VisitSwitchStatement(SwitchStatementSyntax node)
        {
            var switchStatement = new Switch();
            statements.Add(switchStatement);

            switchStatement.Expression = node.Expression.ToString();

            foreach (var section in node.Sections)
            {
                var switchSection = new SwitchSection();
                switchStatement.Sections.Add(switchSection);

                switchSection.Labels.AddRange(section.Labels.Select(l => Label(l)));

                var invocationAnalyzer = new InvocationsAnalyzer(semanticModel, switchSection.Statements);
                invocationAnalyzer.Visit(section);
            }
        }

        private static string Label(SwitchLabelSyntax label)
        {
            switch (label)
            {
                case CasePatternSwitchLabelSyntax casePatternLabel:
                    return casePatternLabel.WhenClause?.Condition?.ToString() ?? casePatternLabel.Pattern?.ToString();

                case CaseSwitchLabelSyntax caseLabel:
                    return caseLabel.Value.ToString();

                case DefaultSwitchLabelSyntax defaultLabel:
                    return defaultLabel.Keyword.ToString();

                default:
                    return label.ToString();
            }
        }
    }
}