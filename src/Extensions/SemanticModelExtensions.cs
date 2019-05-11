using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace roslyn_uml
{
    public static class SemanticModelExtensions
    {
        public static string GetTypeDisplayString(this SemanticModel semanticModel, TypeSyntax typeSyntax)
        {
            return semanticModel.GetTypeInfo(typeSyntax).Type.ToDisplayString();
        }

        public static string GetTypeDisplayString(this SemanticModel semanticModel, ExpressionSyntax expressionSyntax)
        {
            var type = semanticModel.GetTypeInfo(expressionSyntax).Type?.ToDisplayString();
            if (type != null)
            {
                return type;
            }

            return semanticModel.GetTypeInfo(expressionSyntax).ConvertedType.ToDisplayString();
        }
    }
}