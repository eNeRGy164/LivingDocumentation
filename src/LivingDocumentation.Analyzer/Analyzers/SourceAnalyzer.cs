namespace LivingDocumentation;

public class SourceAnalyzer(SemanticModel semanticModel, List<TypeDescription> types) : CSharpSyntaxWalker
{
    private TypeDescription? currentType = null;

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

        // Check for parts that are generated and not declared by the author
        var symbol = semanticModel.GetDeclaredSymbol(node);
        if (symbol != null)
        {
            foreach (var constructor in symbol.Constructors)
            {
                var constructorDescription = new ConstructorDescription(symbol.Name);

                foreach (var parameter in constructor.Parameters)
                {
                    var parameterDescription = new ParameterDescription(parameter.Type.ToDisplayString(), parameter.Name);
                    constructorDescription.Parameters.Add(parameterDescription);

                    parameterDescription.HasDefaultValue = parameter.HasExplicitDefaultValue;
                }

                if (!this.currentType.Constructors.Any(c => c.Parameters.Select(p => p.Type).SequenceEqual(constructorDescription.Parameters.Select(p => p.Type))))
                {
                    this.currentType.AddMember(constructorDescription);

                    constructorDescription.Modifiers |= constructor.IsSealed ? Modifier.Sealed : 0;

                    this.EnsureMemberDefaultAccessModifier(constructorDescription);
                }
            }

            foreach (var member in symbol.GetMembers())
            {
                if (member is IPropertySymbol property)
                {
                    if (this.currentType.Properties.Any(p => p.Name == property.Name))
                    {
                        continue;
                    }

                    var propertyDescription = new PropertyDescription(property.Type.ToDisplayString(), property.Name);
                    this.currentType.AddMember(propertyDescription);

                    propertyDescription.Modifiers |= property.IsReadOnly ? Modifier.Readonly : 0;

                    this.EnsureMemberDefaultAccessModifier(propertyDescription);
                }
            }
        }
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
        if (this.currentType is null) return;

        foreach (var variable in node.Declaration.Variables)
        {
            var fieldDescription = new FieldDescription(semanticModel.GetTypeDisplayString(node.Declaration.Type), variable.Identifier.ValueText);
            this.currentType.AddMember(fieldDescription);

            fieldDescription.Modifiers |= ParseModifiers(node.Modifiers);
            this.EnsureMemberDefaultAccessModifier(fieldDescription);
            this.ExtractAttributes(node.AttributeLists, fieldDescription.Attributes);

            fieldDescription.Initializer = variable.Initializer?.Value.ResolveValue(semanticModel);
            fieldDescription.DocumentationComments = this.ExtractDocumentation(variable);
        }

        base.VisitFieldDeclaration(node);
    }

    public override void VisitEventFieldDeclaration(EventFieldDeclarationSyntax node)
    {
        if (this.currentType is null) return;

        foreach (var variable in node.Declaration.Variables)
        {
            var eventDescription = new EventDescription(semanticModel.GetTypeDisplayString(node.Declaration.Type), variable.Identifier.ValueText);
            this.currentType.AddMember(eventDescription);

            eventDescription.Modifiers |= ParseModifiers(node.Modifiers);
            this.EnsureMemberDefaultAccessModifier(eventDescription);
            this.ExtractAttributes(node.AttributeLists, eventDescription.Attributes);

            eventDescription.Initializer = variable.Initializer?.Value.ResolveValue(semanticModel);
            eventDescription.DocumentationComments = this.ExtractDocumentation(variable);
        }

        base.VisitEventFieldDeclaration(node);
    }

    public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
    {
        if (this.currentType is null) return;

        var propertyDescription = new PropertyDescription(semanticModel.GetTypeDisplayString(node.Type), node.Identifier.ToString());
        this.currentType.AddMember(propertyDescription);

        propertyDescription.Modifiers |= ParseModifiers(node.Modifiers);
        this.EnsureMemberDefaultAccessModifier(propertyDescription);
        this.ExtractAttributes(node.AttributeLists, propertyDescription.Attributes);

        propertyDescription.Initializer = node.Initializer?.Value.ResolveValue(semanticModel);
        propertyDescription.DocumentationComments = this.ExtractDocumentation(node);

        base.VisitPropertyDeclaration(node);
    }

    public override void VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node)
    {
        if (this.currentType is null) return;

        var enumMemberDescription = new EnumMemberDescription(node.Identifier.ToString(), node.EqualsValue?.Value.ToString());
        this.currentType.AddMember(enumMemberDescription);

        enumMemberDescription.Modifiers |= Modifier.Public;
        enumMemberDescription.DocumentationComments = this.ExtractDocumentation(node);

        base.VisitEnumMemberDeclaration(node);
    }

    public override void VisitConstructorDeclaration(ConstructorDeclarationSyntax node)
    {
        if (this.currentType is null) return;

        var constructorDescription = new ConstructorDescription(node.Identifier.ToString());
        this.currentType.AddMember(constructorDescription);

        this.ExtractBaseMethodDeclaration(node, constructorDescription);

        base.VisitConstructorDeclaration(node);
    }

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        if (this.currentType is null) return;

        var methodDescription = new MethodDescription(semanticModel.GetTypeInfo(node.ReturnType).Type?.ToDisplayString(), node.Identifier.ToString());
        this.currentType.AddMember(methodDescription);

        this.ExtractBaseMethodDeclaration(node, methodDescription);

        base.VisitMethodDeclaration(node);
    }

    private void ExtractBaseTypeDeclaration(TypeType type, BaseTypeDeclarationSyntax node)
    {
        var currentType = new TypeDescription(type, semanticModel.GetDeclaredSymbol(node)?.ToDisplayString());
        if (!types.Contains(currentType))
        {
            types.Add(currentType);
            this.currentType = currentType;
        }
        else
        {
            this.currentType = types.First(t => string.Equals(t.FullName, currentType.FullName, StringComparison.Ordinal));
        }

        if (node.BaseList != null)
        {
            this.currentType.BaseTypes.AddRange(node.BaseList.Types.Select(t => semanticModel.GetTypeDisplayString(t.Type)));
        }

        this.currentType.Modifiers |= ParseModifiers(node.Modifiers);
        this.EnsureTypeDefaultAccessModifier(node);

        this.currentType.DocumentationComments = this.ExtractDocumentation(node);

        this.ExtractAttributes(node.AttributeLists, this.currentType.Attributes);
    }

    private void EnsureTypeDefaultAccessModifier(BaseTypeDeclarationSyntax node)
    {
        if (this.currentType is null) return;

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

        var embeddedAnalyzer = new SourceAnalyzer(semanticModel, types);
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
            var attributeDescription = new AttributeDescription(semanticModel.GetTypeDisplayString(attribute), attribute.Name.ToString());
            attributeDescriptions.Add(attributeDescription);

            if (attribute.ArgumentList != null)
            {
                foreach (var argument in attribute.ArgumentList.Arguments)
                {
                    var value = argument.Expression!.ResolveValue(semanticModel);

                    var argumentDescription = new AttributeArgumentDescription(argument.NameEquals?.Name.ToString() ?? argument.Expression.ResolveValue(semanticModel), semanticModel.GetTypeDisplayString(argument.Expression!), value);
                    attributeDescription.Arguments.Add(argumentDescription);
                }
            }
        }
    }

    private DocumentationCommentsDescription? ExtractDocumentation(SyntaxNode node)
    {
        return DocumentationCommentsDescription.Parse(semanticModel.GetDeclaredSymbol(node)?.GetDocumentationCommentXml());
    }

    private void ExtractBaseMethodDeclaration(BaseMethodDeclarationSyntax node, IHaveAMethodBody method)
    {
        method.Modifiers |= ParseModifiers(node.Modifiers);
        method.DocumentationComments = this.ExtractDocumentation(node);

        this.EnsureMemberDefaultAccessModifier(method);
        this.ExtractAttributes(node.AttributeLists, method.Attributes);

        foreach (var parameter in node.ParameterList.Parameters)
        {
            var parameterDescription = new ParameterDescription(semanticModel.GetTypeDisplayString(parameter.Type!), parameter.Identifier.ToString());
            method.Parameters.Add(parameterDescription);

            parameterDescription.HasDefaultValue = parameter.Default != null;
            this.ExtractAttributes(parameter.AttributeLists, parameterDescription.Attributes);
        }

        var invocationAnalyzer = new InvocationsAnalyzer(semanticModel, method.Statements);
        invocationAnalyzer.Visit((SyntaxNode?)node.Body ?? node.ExpressionBody);
    }

    private static Modifier ParseModifiers(SyntaxTokenList modifiers)
    {
        return (Modifier)modifiers.Select(m => Enum.TryParse(typeof(Modifier), m.ValueText, true, out var value) ? (int)value : 0).Sum();
    }
}
