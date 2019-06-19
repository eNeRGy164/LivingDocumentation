namespace LivingDocumentation
{
    public static class IHaveModifiersExtensions
    {
        public static bool IsStatic(this IHaveModifiers iHaveModifiers)
        {
            return iHaveModifiers.Modifiers.Contains(Modifier.Static);
        }

        public static bool IsPublic(this IHaveModifiers iHaveModifiers)
        {
            return iHaveModifiers.Modifiers.Contains(Modifier.Public);
        }

        public static bool IsInternal(this IHaveModifiers iHaveModifiers)
        {
            return iHaveModifiers.Modifiers.Contains(Modifier.Internal);
        }

        public static bool IsProtected(this IHaveModifiers iHaveModifiers)
        {
            return iHaveModifiers.Modifiers.Contains(Modifier.Protected);
        }

        public static bool IsAbstract(this IHaveModifiers iHaveModifiers)
        {
            return iHaveModifiers.Modifiers.Contains(Modifier.Abstract);
        }

        public static bool IsPrivate(this IHaveModifiers iHaveModifiers)
        {
            return iHaveModifiers.Modifiers.Contains(Modifier.Private);
        }

        public static bool IsAsync(this IHaveModifiers iHaveModifiers)
        {
            return iHaveModifiers.Modifiers.Contains(Modifier.Async);
        }

        public static bool IsOverride(this IHaveModifiers iHaveModifiers)
        {
            return iHaveModifiers.Modifiers.Contains(Modifier.Override);
        }

        public static bool IsReadonly(this IHaveModifiers iHaveModifiers)
        {
            return iHaveModifiers.Modifiers.Contains(Modifier.Readonly);
        }
    }
}
