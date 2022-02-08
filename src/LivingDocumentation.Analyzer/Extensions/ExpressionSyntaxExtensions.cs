using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LivingDocumentation;

public static class ExpressionSyntaxExtensions
{
    public static string ResolveValue(this ExpressionSyntax expression, SemanticModel semanticModel)
    {
        return expression switch
        {
            IdentifierNameSyntax identifier when semanticModel.GetSymbolInfo(identifier).Symbol is IFieldSymbol field && field.IsConst => field.ConstantValue!.ToString()!,
            IdentifierNameSyntax identifier => identifier.Identifier.ValueText,
            LiteralExpressionSyntax literal => literal.Token.ValueText,
            _ => expression.ToString()
        };
    }
}
