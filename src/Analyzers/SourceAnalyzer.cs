using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace roslyn_uml
{
    internal class SourceAnalyzer : CSharpSyntaxWalker
    {
        private readonly SemanticModel semanticModel;
        private readonly IList<TypeDescription> types;
        private readonly IReadOnlyList<AssemblyIdentity> referencedAssemblies;

        private TypeDescription currentType = null;

        public SourceAnalyzer(in SemanticModel semanticModel, IList<TypeDescription> types, IReadOnlyList<AssemblyIdentity> referencedAssemblies)
        {
            this.types = types;
            this.semanticModel = semanticModel;
            this.referencedAssemblies = referencedAssemblies;
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            if (ProcessEmbeddedType(node)) return;

            ExtractBaseTypeDeclaration(TypeType.Class, node);

            base.VisitClassDeclaration(node);
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            if (ProcessEmbeddedType(node)) return;

            ExtractBaseTypeDeclaration(TypeType.Enum, node);

            base.VisitEnumDeclaration(node);
        }

        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            if (ProcessEmbeddedType(node)) return;

            ExtractBaseTypeDeclaration(TypeType.Struct, node);

            base.VisitStructDeclaration(node);
        }

        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            if (ProcessEmbeddedType(node)) return;

            ExtractBaseTypeDeclaration(TypeType.Interface, node);

            base.VisitInterfaceDeclaration(node);
        }

        public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            var fieldDescription = new FieldDescription(semanticModel.GetTypeDisplayString(node.Declaration.Type), node.Declaration.Variables.First().Identifier.ValueText);
            this.currentType.AddMember(fieldDescription);

            fieldDescription.Modifiers.AddRange(node.Modifiers.Select(m => m.ValueText));
            fieldDescription.Initializer = node.Declaration.Variables.First().Initializer?.Value.ToString(); // Assumption: Field has only a single initializer

            base.VisitFieldDeclaration(node);
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            var propertyDescription = new PropertyDescription(semanticModel.GetTypeDisplayString(node.Type), node.Identifier.ToString());
            this.currentType.AddMember(propertyDescription);

            propertyDescription.Modifiers.AddRange(node.Modifiers.Select(m => m.ValueText));
            propertyDescription.Initializer = node.Initializer?.Value.ToString();

            base.VisitPropertyDeclaration(node);
        }

        public override void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
        {
            var enumMemberDescription = new EnumMemberDescription(node.Identifier.ToString(), node.EqualsValue?.Value.ToString());
            this.currentType.AddMember(enumMemberDescription);

            base.VisitEnumMemberDeclaration(node);
        }

        public override void VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            var constructorDescription = new ConstructorDescription(node.Identifier.ToString());
            this.currentType.AddMember(constructorDescription);

            ExtractBaseMethodDeclaration(node, constructorDescription);

            base.VisitConstructorDeclaration(node);
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            var methodDescription = new MethodDescription(semanticModel.GetTypeInfo(node.ReturnType).Type.ToDisplayString(), node.Identifier.ToString());
            this.currentType.AddMember(methodDescription);

            ExtractBaseMethodDeclaration(node, methodDescription);

            base.VisitMethodDeclaration(node);
        }

        private void ExtractBaseTypeDeclaration(TypeType type, BaseTypeDeclarationSyntax node)
        {
            this.currentType = new TypeDescription(type, semanticModel.GetDeclaredSymbol(node).ToDisplayString());
            if (!this.types.Contains(this.currentType))
            {
                this.types.Add(this.currentType);
            }

            if (node.BaseList != null)
            {
                this.currentType.BaseTypes.AddRange(node.BaseList.Types.Select(t => semanticModel.GetTypeDisplayString(t.Type)));
            }

            this.currentType.Modifiers.AddRange(node.Modifiers.Select(m => m.ValueText));
            this.currentType.Documentation = ExtractDocumentation(node);
        }

        private bool ProcessEmbeddedType(SyntaxNode node)
        {
            if (this.currentType == null || !node.Parent.IsKind(SyntaxKind.ClassDeclaration))
            {
                return false;
            }

            var embeddedAnalyzer = new SourceAnalyzer(semanticModel, types, referencedAssemblies);
            embeddedAnalyzer.Visit(node);

            return true;
        }

        private string ExtractDocumentation(BaseTypeDeclarationSyntax node)
        {
            var documentationCommentXml = semanticModel.GetDeclaredSymbol(node).GetDocumentationCommentXml();

            if (string.IsNullOrWhiteSpace(documentationCommentXml) || documentationCommentXml.StartsWith("<!--", StringComparison.Ordinal))
            {
                // No documenation or unparseable documentation
                return null;
            }

            var element = XElement.Parse(documentationCommentXml);
            var summary = element.Element("summary");

            return summary?.Value.Trim();
        }

        private void ExtractBaseMethodDeclaration(BaseMethodDeclarationSyntax node, IHaveAMethodBody method)
        {
            method.Modifiers.AddRange(node.Modifiers.Select(m => m.ValueText));

            foreach (var parameter in node.ParameterList.Parameters)
            {
                var parameterDescription = new ParameterDescription(semanticModel.GetTypeDisplayString(parameter.Type), parameter.Identifier.ToString());
                method.Parameters.Add(parameterDescription);

                parameterDescription.HasDefaultValue = parameter.Default != null;
            }

            var invocationAnalyzer = new InvocationsAnalyzer(semanticModel, method.InvokedMethods, referencedAssemblies);
            invocationAnalyzer.Visit(node.Body);
        }
    }
}