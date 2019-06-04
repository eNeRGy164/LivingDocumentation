using System.Collections.Generic;
using System.Linq;

namespace LivingDocumentation.Uml
{
    public static class InteractionFragmentExtensions
    {
        public static IReadOnlyList<T> Descendants<T>(this IEnumerable<InteractionFragment> nodes)
            where T : InteractionFragment
        {
            var result = new List<T>();

            foreach (var node in nodes)
            {
                switch (node)
                {
                    case T t:
                        result.Add(t);
                        break;

                    case Alt a:
                        result.AddRange(a.Sections.SelectMany(s => s.Fragments).Descendants<T>());
                        break;

                    default:
                        break;
                }
            }

            return result;
        }

        public static IReadOnlyList<InteractionFragment> Ancestors(this InteractionFragment fragment)
        {
            var result = new List<InteractionFragment>();

            var parent = fragment.Parent;
            while (parent != null)
            {
                result.Add(parent);

                parent = parent.Parent;
            }

            return result;
        }

        public static IReadOnlyList<InteractionFragment> StatementsBeforeSelf(this InteractionFragment fragment)
        {
            if (fragment.Parent != null)
            {
                return fragment.Parent.Fragments.TakeWhile(s => s != fragment).ToList();
            }

            return new List<InteractionFragment>(0);
        }
    }
}
