using LivingDocumentation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LivingDocumentation
{
    public static class TypeDescriptionListExtensions
    {
        public static TypeDescription First(this IEnumerable<TypeDescription> types, string typeName)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));

            return types.First(t => string.Equals(t.FullName, typeName, StringComparison.Ordinal));
        }

        public static TypeDescription FirstOrDefault(this IEnumerable<TypeDescription> types, string typeName)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));

            return types.FirstOrDefault(t => string.Equals(t.FullName, typeName, StringComparison.Ordinal));
        }

        public static IReadOnlyList<IHaveAMethodBody> GetInvokedMethod(this IEnumerable<TypeDescription> types, InvocationDescription invocation)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));

            var type = types.FirstOrDefault(invocation.ContainingType);
            if (type == null)
            {
                return new List<IHaveAMethodBody>(0);
            }

            return type.MethodBodies()
                .Where(m => invocation.MatchesMethod(m))
                .ToList();
        }

        public static IReadOnlyList<InvocationDescription> GetInvocationConsequences(this IEnumerable<TypeDescription> types, InvocationDescription invocation)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));

            var consequences = types.GetInvokedMethod(invocation)
                .SelectMany(m => m.Statements.OfType<InvocationDescription>())
                .SelectMany(im => types.GetInvocationConsequences(im))
                .Prepend(invocation)
                .ToList();

            return consequences;
        }

        public static IReadOnlyList<Statement> GetInvocationConsequenceStatements(this IEnumerable<TypeDescription> types, InvocationDescription invocation)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));

            var consequences = types.GetInvokedMethod(invocation)
                .SelectMany(m => m.Statements)
                .SelectMany(im => TraverseStatement(types, im))
                .Prepend(invocation)
                .ToList();

            return consequences;
        }

        public static IReadOnlyList<Statement> TraverseStatement(this IEnumerable<TypeDescription> types, Statement sourceStatement)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));

            switch (sourceStatement)
            {
                case InvocationDescription invocationDescription:
                    return types.GetInvocationConsequenceStatements(invocationDescription);

                case Switch sourceSwitch:
                    var destinationSwitch = new Switch();
                    foreach (var switchSection in sourceSwitch.Sections)
                    {
                        var section = new SwitchSection();
                        section.Labels.AddRange(switchSection.Labels);

                        foreach (var statement in switchSection.Statements)
                        {
                            section.Statements.AddRange(types.TraverseStatement(statement));
                        }

                        destinationSwitch.Sections.Add(section);
                    }

                    destinationSwitch.Expression = sourceSwitch.Expression;

                    return new List<Statement>(1) { destinationSwitch };

                case If sourceIf:
                    var destinationÍf = new If();

                    foreach (var ifElseSection in sourceIf.Sections)
                    {
                        var section = new IfElseSection();

                        foreach (var statement in ifElseSection.Statements)
                        {
                            section.Statements.AddRange(types.TraverseStatement(statement));
                        }

                        section.Condition = ifElseSection.Condition;

                        destinationÍf.Sections.Add(section);
                    }

                    return new List<Statement>(1) { destinationÍf };

                default:
                    return new List<Statement>(0);
            }
        }

        public static void PopulateInheritedBaseTypes(this IEnumerable<TypeDescription> types)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));

            foreach (var type in types)
            {
                for (var i = 0; i < type.BaseTypes.Count; i++)
                {
                    string baseType = type.BaseTypes[i];

                    types.PopulateInheritedBaseTypes(baseType, type.BaseTypes);
                }
            }
        }

        private static void PopulateInheritedBaseTypes(this IEnumerable<TypeDescription> types, string fullName, List<string> baseTypes)
        {
            var type = types.FirstOrDefault(fullName);
            if (type == null)
            {
                return;
            }

            foreach (var baseType in type.BaseTypes)
            {
                if (!baseTypes.Contains(baseType))
                {
                    baseTypes.Add(baseType);
                }

                types.PopulateInheritedBaseTypes(baseType, baseTypes);
            }
        }

        public static void PopulateInheritedMembers(this IEnumerable<TypeDescription> types)
        {
            if (types == null) throw new ArgumentNullException(nameof(types));

            foreach (var type in types)
            {
                foreach (string baseType in type.BaseTypes)
                {
                    var inheretedType = types.FirstOrDefault(baseType);
                    if (inheretedType == null)
                    {
                        continue;
                    }

                    InheritMember(type, type.Fields, inheretedType.Fields);
                    InheritMember(type, type.Constructors, inheretedType.Constructors);
                    InheritMember(type, type.Properties, inheretedType.Properties);
                    InheritMember(type, type.Methods, inheretedType.Methods);
                    InheritMember(type, type.EnumMembers, inheretedType.EnumMembers);
                    InheritMember(type, type.Events, inheretedType.Events);
                }
            }
        }

        private static void InheritMember(TypeDescription type, IReadOnlyList<MemberDescription> typeMembers, IReadOnlyList<MemberDescription> baseTypeMembers)
        {
            foreach (var member in baseTypeMembers.Where(f => !f.IsPrivate()))
            {
                // TODO: More complex support for overrides, etc.
                if (!typeMembers.Contains(member))
                {
                    // TODO: Clone?
                    type.AddMember(member);
                }
            }
        }
    }
}
