namespace LivingDocumentation
{
    public static class IHaveModifiersExtensions
    {
        public static bool IsStatic(this IHaveModifiers iHaveModifiers)
        {
            return (iHaveModifiers.Modifiers & Modifier.Static) == Modifier.Static;
        }

        public static bool IsPublic(this IHaveModifiers iHaveModifiers)
        {
            return (iHaveModifiers.Modifiers & Modifier.Public) == Modifier.Public;
        }

        public static bool IsInternal(this IHaveModifiers iHaveModifiers)
        {
            return (iHaveModifiers.Modifiers & Modifier.Internal) == Modifier.Internal;
        }

        public static bool IsProtected(this IHaveModifiers iHaveModifiers)
        {
            return (iHaveModifiers.Modifiers & Modifier.Protected) == Modifier.Protected;
        }

        public static bool IsAbstract(this IHaveModifiers iHaveModifiers)
        {
            return (iHaveModifiers.Modifiers & Modifier.Abstract) == Modifier.Abstract;
        }

        public static bool IsPrivate(this IHaveModifiers iHaveModifiers)
        {
            return (iHaveModifiers.Modifiers & Modifier.Private) == Modifier.Private;
        }

        public static bool IsAsync(this IHaveModifiers iHaveModifiers)
        {
            return (iHaveModifiers.Modifiers & Modifier.Async) == Modifier.Async;
        }

        public static bool IsOverride(this IHaveModifiers iHaveModifiers)
        {
            return (iHaveModifiers.Modifiers & Modifier.Override) == Modifier.Override;
        }

        public static bool IsReadonly(this IHaveModifiers iHaveModifiers)
        {
            return (iHaveModifiers.Modifiers & Modifier.Readonly) == Modifier.Readonly;
        }
    }
}
