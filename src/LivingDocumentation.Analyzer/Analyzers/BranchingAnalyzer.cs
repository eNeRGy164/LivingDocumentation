namespace LivingDocumentation;

internal class BranchingAnalyzer : CSharpSyntaxWalker
{
    private readonly SemanticModel semanticModel;
    private readonly List<Statement> statements;

    public BranchingAnalyzer(in SemanticModel semanticModel, List<Statement> statements)
    {
        this.semanticModel = semanticModel;
        this.statements = statements;
    }

    public override void VisitIfStatement(IfStatementSyntax node)
    {
        var ifStatement = new If();
        this.statements.Add(ifStatement);

        var ifSection = new IfElseSection();
        ifStatement.Sections.Add(ifSection);

        ifSection.Condition = node.Condition.ToString();

        var ifInvocationAnalyzer = new InvocationsAnalyzer(this.semanticModel, ifSection.Statements);
        ifInvocationAnalyzer.Visit(node.Statement);

        var elseNode = node.Else;
        while (elseNode != null)
        {
            var section = new IfElseSection();
            ifStatement.Sections.Add(section);

            var elseInvocationAnalyzer = new InvocationsAnalyzer(this.semanticModel, section.Statements);
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
        this.statements.Add(switchStatement);

        switchStatement.Expression = node.Expression.ToString();

        foreach (var section in node.Sections)
        {
            var switchSection = new SwitchSection();
            switchStatement.Sections.Add(switchSection);

            switchSection.Labels.AddRange(section.Labels.Select(l => Label(l)));

            var invocationAnalyzer = new InvocationsAnalyzer(this.semanticModel, switchSection.Statements);
            invocationAnalyzer.Visit(section);
        }
    }

    private static string Label(SwitchLabelSyntax label)
    {
        return label switch
        {
            CasePatternSwitchLabelSyntax casePattern when casePattern.WhenClause?.Condition is not null => $"{casePattern.Pattern} when {casePattern.WhenClause.Condition}",
            CasePatternSwitchLabelSyntax casePattern => casePattern.Pattern.ToString(),
            CaseSwitchLabelSyntax @case => @case.Value.ToString(),
            DefaultSwitchLabelSyntax @default => @default.Keyword.ToString(),
            _ => throw new ArgumentOutOfRangeException(nameof(label)),
        };
    }
}
