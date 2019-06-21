using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LivingDocumentation
{
    internal class LoopingAnalyzer : CSharpSyntaxWalker
    {
        private readonly SemanticModel semanticModel;
        private readonly IList<Statement> statements;

        public LoopingAnalyzer(in SemanticModel semanticModel, IList<Statement> statements)
        {
            this.semanticModel = semanticModel;
            this.statements = statements;
        }

        public override void VisitForEachStatement(ForEachStatementSyntax node)
        {
            var forEachStatement = new ForEach();
            statements.Add(forEachStatement);

            forEachStatement.Expression = $"{node.Identifier.ToString()} in {node.Expression.ToString()}";

            var invocationAnalyzer = new InvocationsAnalyzer(semanticModel, forEachStatement.Statements);
            invocationAnalyzer.Visit(node.Statement);
        }
    }
}