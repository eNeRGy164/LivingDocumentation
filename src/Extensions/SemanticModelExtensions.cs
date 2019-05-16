using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace roslyn_uml
{
    public static class SemanticModelExtensions
    {
        public static string GetTypeDisplayString(this SemanticModel semanticModel, SyntaxNode node)
        {
            return semanticModel.GetTypeInfo(node).Type.ToDisplayString();
        }

        public static string GetTypeDisplayString(this SemanticModel semanticModel, ExpressionSyntax expression)
        {
            var type = semanticModel.GetTypeInfo(expression).Type?.ToDisplayString();
            if (type != null)
            {
                return type;
            }

            return semanticModel.GetTypeInfo(expression).ConvertedType?.ToDisplayString();
        }
    }
}