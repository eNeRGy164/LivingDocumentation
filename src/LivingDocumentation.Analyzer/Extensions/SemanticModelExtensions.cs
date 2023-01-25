namespace LivingDocumentation;

public static class SemanticModelExtensions
{
    public static string GetTypeDisplayString(this SemanticModel semanticModel, SyntaxNode node)
    {
        return semanticModel.GetTypeInfo(node).Type.ToDisplayString();
    }

    public static string GetTypeDisplayString(this SemanticModel semanticModel, ExpressionSyntax expression)
    {
        var type = semanticModel.GetTypeInfo(expression).Type?.ToDisplayString();
        if (type is not null)
        {
            return type;
        }

        type = semanticModel.GetTypeInfo(expression).ConvertedType?.ToDisplayString();
        if (type is not null)
        {
            return type;
        }

        return semanticModel.GetCollectionInitializerSymbolInfo(expression).Symbol?.ContainingType.ToDisplayString() ?? throw new KeyNotFoundException($"Could not resolve a display name for {expression}");
    }
}
