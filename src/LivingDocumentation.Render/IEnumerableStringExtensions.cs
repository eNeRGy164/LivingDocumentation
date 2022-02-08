namespace LivingDocumentation;

public static class IEnumerableStringExtensions
{
    public static IReadOnlyList<string> StartsWith(this IEnumerable<string> list, string partialName)
    {
        return list.Where(bt => bt.StartsWith(partialName, StringComparison.Ordinal)).ToList();
    }
}
