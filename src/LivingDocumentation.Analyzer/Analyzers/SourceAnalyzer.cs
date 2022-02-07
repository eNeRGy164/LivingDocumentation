using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LivingDocumentation
{
    public class SourceAnalyzer : CSharpSyntaxWalker
    {
        private readonly SemanticModel semanticModel;
        private readonly List<TypeDescription> types;

        private TypeDescription? currentType = null;

        public SourceAnalyzer(in SemanticModel semanticModel, List<TypeDescription> types)
        {
            this.types = types;
            this.semanticModel = semanticModel;
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            if (this.ProcessedEmbeddedType(node)) return;

            this.ExtractBaseTypeDeclaration(TypeType.Class, node);

            base.VisitClassDeclaration(node);
        }

#if NET6_0_OR_GREATER
        public override void VisitRecordDeclaration(RecordDeclarationSyntax node)
        {
            if (this.ProcessedEmbeddedType(node)) return;

            var type = TypeType.Class;
            if (node.ClassOrStructKeyword.ValueText == "struct")
            {
                type = TypeType.Struct;
            }

            this.ExtractBaseTypeDeclaration(type, node);

            base.VisitRecordDeclaration(node);
        }
#endif

        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            if (this.ProcessedEmbeddedType(node)) return;

            this.ExtractBaseTypeDeclaration(TypeType.Enum, node);

            base.VisitEnumDeclaration(node);
        }

        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            if (this.ProcessedEmbeddedType(node)) return;

            this.ExtractBaseTypeDeclaration(TypeType.Struct, node);

            base.VisitStructDeclaration(node);
        }

        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            if (this.ProcessedEmbeddedType(node)) return;

            this.ExtractBaseTypeDeclaration(TypeType.Interface, node);

            base.VisitInterfaceDeclaration(node);
        }

        public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            foreach (var variable in node.Declaration.Variables)
            {
                var fieldDescription = new FieldDescription(this.semanticModel.GetTypeDisplayString(node.Declaration.Type), variable.Identifier.ValueText);
                this.currentType.AddMember(fieldDescription);

                fieldDescription.Modifiers |= ParseModifiers(node.Modifiers);
                this.EnsureMemberDefaultAccessModifier(fieldDescription);
                this.ExtractAttributes(node.AttributeLists, fieldDescription.Attributes);

                fieldDescription.Initializer = variable.Initializer?.Value.ToString();
                fieldDescription.DocumentationComments = this.ExtractDocumentation(variable);
            }

            base.VisitFieldDeclaration(node);
        }

        public override void VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
        {
            foreach (var variable in node.Declaration.Variables)
            {
                var eventDescription = new EventDescription(this.semanticModel.GetTypeDisplayString(node.Declaration.Type), variable.Identifier.ValueText);
                this.currentType.AddMember(eventDescription);

                eventDescription.Modifiers |= ParseModifiers(node.Modifiers);
                this.EnsureMemberDefaultAccessModifier(eventDescription);
                this.ExtractAttributes(node.AttributeLists, eventDescription.Attributes);

                eventDescription.Initializer = variable.Initializer?.Value.ToString();
                eventDescription.DocumentationComments = this.ExtractDocumentation(variable);
            }

            base.VisitEventFieldDeclaration(node);
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            var propertyDescription = new PropertyDescription(this.semanticModel.GetTypeDisplayString(node.Type), node.Identifier.ToString());
            this.currentType.AddMember(propertyDescription);

            propertyDescription.Modifiers |= ParseModifiers(node.Modifiers);
            this.EnsureMemberDefaultAccessModifier(propertyDescription);
            this.ExtractAttributes(node.AttributeLists, propertyDescription.Attributes);

            propertyDescription.Initializer = node.Initializer?.Value.ToString();
            propertyDescription.DocumentationComments = this.ExtractDocumentation(node);

            base.VisitPropertyDeclaration(node);
        }

        public override void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
        {
            var enumMemberDescription = new EnumMemberDescription(node.Identifier.ToString(), node.EqualsValue?.Value.ToString());
            this.currentType.AddMember(enumMemberDescription);

            enumMemberDescription.Modifiers |= Modifier.Public;
            enumMemberDescription.DocumentationComments = this.ExtractDocumentation(node);

            base.VisitEnumMemberDeclaration(node);
        }

        public override void VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            var constructorDescription = new ConstructorDescription(node.Identifier.ToString());
            this.currentType.AddMember(constructorDescription);

            this.ExtractBaseMethodDeclaration(node, constructorDescription);

            base.VisitConstructorDeclaration(node);
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            var methodDescription = new MethodDescription(this.semanticModel.GetTypeInfo(node.ReturnType).Type?.ToDisplayString(), node.Identifier.ToString());
            this.currentType.AddMember(methodDescription);

            this.ExtractBaseMethodDeclaration(node, methodDescription);

            base.VisitMethodDeclaration(node);
        }

        private void ExtractBaseTypeDeclaration(TypeType type, BaseTypeDeclarationSyntax node)
        {
            var currentType = new TypeDescription(type, this.semanticModel.GetDeclaredSymbol(node)?.ToDisplayString());
            if (!this.types.Contains(currentType))
            {
                this.types.Add(currentType);
                this.currentType = currentType;
            }
            else
            {
                this.currentType = this.types.First(t => string.Equals(t.FullName, currentType.FullName, StringComparison.Ordinal));
            }

            if (node.BaseList != null)
            {
                this.currentType.BaseTypes.AddRange(node.BaseList.Types.Select(t => this.semanticModel.GetTypeDisplayString(t.Type)));
            }

            this.currentType.Modifiers |= ParseModifiers(node.Modifiers);
            this.EnsureTypeDefaultAccessModifier(node);

            this.currentType.DocumentationComments = this.ExtractDocumentation(node);

            this.ExtractAttributes(node.AttributeLists, this.currentType.Attributes);
        }

        private void EnsureTypeDefaultAccessModifier(BaseTypeDeclarationSyntax node)
        {
            if (!node.Ancestors().Any(a => a.IsKind(SyntaxKind.ClassDeclaration) || a.IsKind(SyntaxKind.StructDeclaration)))
            {
                // Not nested, default is internal
                if (!this.currentType.IsPublic() && !this.currentType.IsInternal())
                {
                    this.currentType.Modifiers |= Modifier.Internal;
                }
            }
            else
            {
                // Nested, default is private
                if (!this.currentType.IsPublic() && !this.currentType.IsInternal() && !this.currentType.IsPrivate() && !this.currentType.IsProtected())
                {
                    this.currentType.Modifiers |= Modifier.Private;
                }
            }
        }

        private void EnsureMemberDefaultAccessModifier(IHaveModifiers member)
        {
            // Default is private
            if (!member.IsPublic() && !member.IsInternal() && !member.IsPrivate() && !member.IsProtected())
            {
                member.Modifiers |= Modifier.Private;
            }
        }

        private bool ProcessedEmbeddedType(SyntaxNode node)
        {
            if (this.currentType == null || (!node.Parent.IsKind(SyntaxKind.ClassDeclaration) && !node.Parent.IsKind(SyntaxKind.StructDeclaration)))
            {
                return false;
            }

            var embeddedAnalyzer = new SourceAnalyzer(this.semanticModel, this.types);
            embeddedAnalyzer.Visit(node);

            return true;
        }

        private void ExtractAttributes(SyntaxList<AttributeListSyntax> attributes, List<IAttributeDescription> attributeDescriptions)
        {
            if (attributes == null)
            {
                return;
            }

            foreach (var attribute in attributes.SelectMany(a => a.Attributes))
            {
                var attributeDescription = new AttributeDescription(this.semanticModel.GetTypeDisplayString(attribute), attribute.Name.ToString());
                attributeDescriptions.Add(attributeDescription);

                if (attribute.ArgumentList != null)
                {
                    foreach (var argument in attribute.ArgumentList.Arguments)
                    {
                        var value = argument.Expression switch
                        {
                            LiteralExpressionSyntax literalExpression => literalExpression.Token.ValueText,
                            _ => argument.Expression?.ToString(),
                        };

                        var argumentDescription = new AttributeArgumentDescription(argument.NameEquals?.Name.ToString() ?? argument.Expression?.ToString(), this.semanticModel.GetTypeDisplayString(argument.Expression), value);
                        attributeDescription.Arguments.Add(argumentDescription);
                    }
                }
            }
        }

        private DocumentationCommentsDescription? ExtractDocumentation(SyntaxNode node)
        {
            return DocumentationCommentsDescription.Parse(this.semanticModel.GetDeclaredSymbol(node)?.GetDocumentationCommentXml());
        }

        private void ExtractBaseMethodDeclaration(BaseMethodDeclarationSyntax node, IHaveAMethodBody method)
        {
            method.Modifiers |= ParseModifiers(node.Modifiers);
            method.DocumentationComments = this.ExtractDocumentation(node);

            this.EnsureMemberDefaultAccessModifier(method);
            this.ExtractAttributes(node.AttributeLists, method.Attributes);

            foreach (var parameter in node.ParameterList.Parameters)
            {
                var parameterDescription = new ParameterDescription(this.semanticModel.GetTypeDisplayString(parameter.Type), parameter.Identifier.ToString());
                method.Parameters.Add(parameterDescription);

                parameterDescription.HasDefaultValue = parameter.Default != null;
                this.ExtractAttributes(parameter.AttributeLists, parameterDescription.Attributes);
            }

            var invocationAnalyzer = new InvocationsAnalyzer(this.semanticModel, method.Statements);
            invocationAnalyzer.Visit((SyntaxNode?)node.Body ?? node.ExpressionBody);
        }

        private static Modifier ParseModifiers(SyntaxTokenList modifiers)
        {
            return (Modifier)modifiers.Select(m => Enum.TryParse(typeof(Modifier), m.ValueText, true, out var value) ? (int)value : 0).Sum();
        }
    }
}
