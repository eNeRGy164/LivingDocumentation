namespace LivingDocumentation;

public static class IEnumerableMethodDescriptionExtensions
{
    public static IReadOnlyList<MethodDescription> WithName(this IEnumerable<MethodDescription> list, string name)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));

        return list.Where(m => string.Equals(m.Name, name, StringComparison.Ordinal)).ToList();
    }
}
