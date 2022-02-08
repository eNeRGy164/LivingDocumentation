using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LivingDocumentation;

internal class LoopingAnalyzer : CSharpSyntaxWalker
{
    private readonly SemanticModel semanticModel;
    private readonly List<Statement> statements;

    public LoopingAnalyzer(in SemanticModel semanticModel, List<Statement> statements)
    {
        this.semanticModel = semanticModel;
        this.statements = statements;
    }

    public override void VisitForEachStatement(ForEachStatementSyntax node)
    {
        var forEachStatement = new ForEach();
        this.statements.Add(forEachStatement);

        forEachStatement.Expression = $"{node.Identifier} in {node.Expression}";

        var invocationAnalyzer = new InvocationsAnalyzer(this.semanticModel, forEachStatement.Statements);
        invocationAnalyzer.Visit(node.Statement);
    }
}
