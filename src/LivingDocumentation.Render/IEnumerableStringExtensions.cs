namespace LivingDocumentation;

public static class IEnumerableStringExtensions
{
    public static IReadOnlyList<string> StartsWith(this IEnumerable<string> list, string partialName)
    {
        if (list is null) throw new ArgumentNullException(nameof(list));
        if (partialName is null) throw new ArgumentNullException(nameof(partialName));

        return list.Where(bt => bt.StartsWith(partialName, StringComparison.Ordinal)).ToList();
    }
}
