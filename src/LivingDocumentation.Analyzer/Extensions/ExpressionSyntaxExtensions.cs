namespace LivingDocumentation;

public static class ExpressionSyntaxExtensions
{
    public static string ResolveValue(this ExpressionSyntax expression, SemanticModel semanticModel)
    {
        return expression switch
        {
            IdentifierNameSyntax identifier when semanticModel.GetSymbolInfo(identifier).Symbol is IFieldSymbol field && field.IsConst => field.ConstantValue!.ToString()!,
            IdentifierNameSyntax identifier => identifier.Identifier.ValueText,
            InterpolatedStringExpressionSyntax interpolatedString => interpolatedString.Contents.ToString(),
            LiteralExpressionSyntax literal => literal.Token.ValueText,
            BinaryExpressionSyntax binary when binary.Right is InterpolatedStringExpressionSyntax || binary.Right is LiteralExpressionSyntax => ConcatBinaryExpression(binary),
            ArrayCreationExpressionSyntax arrayCreation => FormatArrayCreation(arrayCreation),
            ObjectCreationExpressionSyntax objectCreation => FormatObjectCreation(objectCreation),
            _ => expression.ToString()
        };
    }

    /// <summary>
    /// Format Object initializers to a single line, removing newlines and indenting.
    /// </summary>
    private static string FormatObjectCreation(ObjectCreationExpressionSyntax objectCreation)
    {
        var initializer = objectCreation.Initializer;
        if (initializer is not null)
        {
            var expressions = new SeparatedSyntaxList<ExpressionSyntax>();

            foreach (var expression in initializer.Expressions)
            {
                expressions = expressions.Add(expression.WithoutTrivia().WithLeadingTrivia(SyntaxFactory.Space));
            }

            initializer = initializer.Update(initializer.OpenBraceToken.WithoutTrivia(), expressions, initializer.CloseBraceToken.WithLeadingTrivia(SyntaxFactory.Space));
        }

        return objectCreation.Update(objectCreation.NewKeyword, objectCreation.Type.WithTrailingTrivia(SyntaxFactory.Space), objectCreation.ArgumentList, initializer).ToString();
    }

    /// <summary>
    /// Format Array initializers to a single line, removing newlines and indenting.
    /// </summary>
    private static string FormatArrayCreation(ArrayCreationExpressionSyntax arrayCreation)
    {
        var initializer = arrayCreation.Initializer;
        if (initializer is not null)
        {
            var expressions = new SeparatedSyntaxList<ExpressionSyntax>();

            foreach (var expression in initializer.Expressions)
            {
                expressions = expressions.Add(expression.WithLeadingTrivia(SyntaxFactory.Space));
            }

            initializer = initializer.Update(initializer.OpenBraceToken.WithoutTrivia(), expressions, initializer.CloseBraceToken);
        }

        return arrayCreation.Update(arrayCreation.NewKeyword, arrayCreation.Type, initializer).ToString();
    }

    /// <summary>
    /// Format String concatination to a single string, removing newlines and indenting.
    /// </summary>
    private static string ConcatBinaryExpression(BinaryExpressionSyntax binary)
    {
        var parts = new Stack<string>();

        AddPart(binary.Right);

        var left = binary.Left;

        while (left is BinaryExpressionSyntax binaryExpression && binaryExpression.IsKind(SyntaxKind.AddExpression))
        {
            AddPart(binaryExpression.Right);

            left = binaryExpression.Left;
        }

        AddPart(left);

        return string.Join(string.Empty, parts);

        void AddPart(ExpressionSyntax binary)
        {
            switch (binary)
            {
                case InterpolatedStringExpressionSyntax interpolatedRight:
                    parts.Push(interpolatedRight.Contents.ToFullString());
                    break;
                case LiteralExpressionSyntax literalRight:
                    parts.Push(literalRight.Token.ValueText);
                    break;
            }
        }
    }
}
