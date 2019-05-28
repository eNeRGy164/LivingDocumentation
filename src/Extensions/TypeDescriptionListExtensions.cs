using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace roslyn_uml
{
    public static class TypeDescriptionListExtensions
    {
        public static TypeDescription FirstOrDefault(this IEnumerable<TypeDescription> types, string typeName)
        {
            return types.FirstOrDefault(t => string.Equals(t.FullName, typeName));
        }

        public static IReadOnlyList<IHaveAMethodBody> GetInvokedMethod(this IEnumerable<TypeDescription> types, InvocationDescription invocation)
        {
            return types
                .Where(t => string.Equals(t.FullName, invocation.ContainingType))
                .SelectMany(t => t.Methods.Cast<IHaveAMethodBody>().Concat(t.Constructors))
                .Where(m => string.Equals(m.Name, invocation.Name) && MatchParameters(invocation, m))
                .ToList();
        }

        public static IReadOnlyList<InvocationDescription> GetInvocationConsequences(this IEnumerable<TypeDescription> types, InvocationDescription invocation)
        {
            var consequences = types.GetInvokedMethod(invocation)
                .SelectMany(m => m.InvokedMethods)
                .SelectMany(im => types.GetInvocationConsequences(im))
                .Prepend(invocation)
                .ToList();

            return consequences;
        }

        public static IReadOnlyList<Statement> GetInvocationConsequenceStatements(this IEnumerable<TypeDescription> types, InvocationDescription invocation)
        {
            var consequences = types.GetInvokedMethod(invocation)
                .SelectMany(m => m.Statements)
                .SelectMany(im => TraverseStatement(types, im))
                .Prepend(invocation)
                .ToList();

            return consequences;
        }

        private static IReadOnlyList<Statement> TraverseStatement(this IEnumerable<TypeDescription> types, Statement sourceStatement)
        {
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

                    return new List<Statement> { destinationSwitch };

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

                    return new List<Statement> { destinationÍf };

                default:
                    return new List<Statement>(0);
            }
        }

        private static bool MatchParameters(InvocationDescription invocation, IHaveAMethodBody method)
        {
            if (invocation.Arguments.Count == 0)
            {
                return method.Parameters.Count == 0;
            }

            var invokedWithTypes = invocation.Arguments.Select(a => a.Type).ToList();
            if (invokedWithTypes.Count > method.Parameters.Count)
            {
                return false;
            }

            var optionalArguments = method.Parameters.Count(p => p.HasDefaultValue);
            if (optionalArguments == 0)
            {
                return method.Parameters.Select(p => p.Type).SequenceEqual(invokedWithTypes);
            }

            return method.Parameters.Take(invokedWithTypes.Count).Select(p => p.Type).SequenceEqual(invokedWithTypes);
        }
    }
}
