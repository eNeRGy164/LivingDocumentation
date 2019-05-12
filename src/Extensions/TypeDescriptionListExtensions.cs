using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace roslyn_uml
{
    public static class TypeDescriptionListExtensions
    {
        public static TypeDescription FirstOrDefault(this IList<TypeDescription> types, string typeName)
        {
            return types.FirstOrDefault(t => string.Equals(t.FullName, typeName));
        }

        public static IList<InvocationDescription> GetInvocationConsequences(this IList<TypeDescription> types, InvocationDescription invocation)
        {
            var consequences = types
                .Where(t => string.Equals(t.FullName, invocation.ContainingType))
                .SelectMany(t => t.Methods.Cast<IHaveAMethodBody>().Concat(t.Constructors))
                .Where(m => string.Equals(m.Name, invocation.Name) && MatchParameters(invocation, m))
                .SelectMany(m => m.InvokedMethods)
                .SelectMany(im => types.GetInvocationConsequences(im))
                .Prepend(invocation)
                .ToList();

            return consequences;
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
