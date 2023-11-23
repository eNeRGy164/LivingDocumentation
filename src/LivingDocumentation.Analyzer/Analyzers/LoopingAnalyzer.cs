namespace LivingDocumentation;

internal class LoopingAnalyzer(SemanticModel semanticModel, List<Statement> statements) : CSharpSyntaxWalker
{
    public override void VisitForEachStatement(ForEachStatementSyntax node)
    {
        var forEachStatement = new ForEach();
        statements.Add(forEachStatement);

        forEachStatement.Expression = $"{node.Identifier} in {node.Expression}";

        var invocationAnalyzer = new InvocationsAnalyzer(semanticModel, forEachStatement.Statements);
        invocationAnalyzer.Visit(node.Statement);
    }
}
