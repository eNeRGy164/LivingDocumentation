namespace LivingDocumentation;

public static class IEnumerableIAttributeDescriptionExtensions
{
    public static IReadOnlyList<IAttributeDescription> OfType(this IEnumerable<IAttributeDescription> list, string fullname)
    {
        return list.Where(ad => string.Equals(ad.Type, fullname, StringComparison.Ordinal)).ToList();
    }

    public static bool HasAttribute(this IEnumerable<IAttributeDescription> list, string fullname)
    {
        return list.OfType(fullname).Any();
    }
}
