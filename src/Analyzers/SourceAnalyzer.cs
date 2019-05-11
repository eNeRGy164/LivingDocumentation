using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace roslyn_uml
{
    internal class SourceAnalyzer : CSharpSyntaxWalker
    {
        private readonly List<TypeDescription> types;
        private readonly IReadOnlyList<AssemblyIdentity> referencedAssemblies;
        private readonly SemanticModel semanticModel;
        private TypeDescription currentType = null;

        public SourceAnalyzer(in SemanticModel semanticModel, ref List<TypeDescription> types, IReadOnlyList<AssemblyIdentity> referencedAssemblies)
        {
            this.types = types;
            this.referencedAssemblies = referencedAssemblies;
            this.semanticModel = semanticModel;
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            ExtractTypeDeclaration(TypeType.Class, node);

            base.VisitClassDeclaration(node);
        }

        public override void VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
        {
            var constructorDescription = new ConstructorDescription(node.Identifier.ToString());
            this.currentType.AddMember(constructorDescription);

            constructorDescription.Modifiers.AddRange(node.Modifiers.Select(m => m.ValueText));

            foreach (var parameter in node.ParameterList.Parameters)
            {
                var parameterDescription = new ParameterDescription(semanticModel.GetTypeDisplayString(parameter.Type), parameter.Identifier.ToString());
                constructorDescription.Parameters.Add(parameterDescription);

                parameterDescription.HasDefaultValue = parameter.Default != null;
            }
            
            var invocationAnalyzer = new InvocationsAnalyzer(semanticModel, referencedAssemblies, constructorDescription.InvokedMethods);
            invocationAnalyzer.Visit(node.Body);

            base.VisitConstructorDeclaration(node);
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            ExtractBaseTypeDeclaration(TypeType.Enum, node);

            base.VisitEnumDeclaration(node);
        }

        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            ExtractTypeDeclaration(TypeType.Struct, node);

            base.VisitStructDeclaration(node);
        }

        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            ExtractTypeDeclaration(TypeType.Interface, node);

            base.VisitInterfaceDeclaration(node);
        }

        public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            var fieldDescription = new FieldDescription(semanticModel.GetTypeDisplayString(node.Declaration.Type), node.Declaration.Variables.First().Identifier.ValueText);
            this.currentType.AddMember(fieldDescription);

            fieldDescription.Initializer = node.Declaration.Variables.First().Initializer?.Value.ToString();
            fieldDescription.Modifiers.AddRange(node.Modifiers.Select(m => m.ValueText));

            
            base.VisitFieldDeclaration(node);
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            var propertyDescription = new PropertyDescription(semanticModel.GetTypeDisplayString(node.Type), node.Identifier.ToString());
            this.currentType.AddMember(propertyDescription);

            propertyDescription.Modifiers.AddRange(node.Modifiers.Select(m => m.ValueText));
            propertyDescription.IsOptional = node.Initializer != null;

            base.VisitPropertyDeclaration(node);
        }

        public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            var methodDescription = new MethodDescription(semanticModel.GetTypeInfo(node.ReturnType).Type.ToDisplayString(), node.Identifier.ToString());
            this.currentType.AddMember(methodDescription);

            methodDescription.Modifiers.AddRange(node.Modifiers.Select(m => m.ValueText));

            foreach (var parameter in node.ParameterList.Parameters)
            {
                var parameterDescription = new ParameterDescription(semanticModel.GetTypeDisplayString(parameter.Type), parameter.Identifier.ToString());
                methodDescription.Parameters.Add(parameterDescription);

                parameterDescription.HasDefaultValue = parameter.Default != null;
            }

            var invocationAnalyzer = new InvocationsAnalyzer(semanticModel, referencedAssemblies, methodDescription.InvokedMethods);
            invocationAnalyzer.Visit(node.Body);

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
        }

        private void ExtractTypeDeclaration(TypeType type, TypeDeclarationSyntax node)
        {
            this.ExtractBaseTypeDeclaration(type, node);
        }
    }
}