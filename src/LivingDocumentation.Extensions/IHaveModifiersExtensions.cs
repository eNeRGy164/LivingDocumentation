namespace LivingDocumentation;
 
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

    public static bool IsConst(this IHaveModifiers iHaveModifiers)
    {
        return (iHaveModifiers.Modifiers & Modifier.Const) == Modifier.Const;
    }

    public static bool IsPartial(this IHaveModifiers iHaveModifiers)
    {
        return (iHaveModifiers.Modifiers & Modifier.Partial) == Modifier.Partial;
    }

    public static bool IsExtern(this IHaveModifiers iHaveModifiers)
    {
        return (iHaveModifiers.Modifiers & Modifier.Extern) == Modifier.Extern;
    }

    public static bool IsNew(this IHaveModifiers iHaveModifiers)
    {
        return (iHaveModifiers.Modifiers & Modifier.New) == Modifier.New;
    }

    public static bool IsSealed(this IHaveModifiers iHaveModifiers)
    {
        return (iHaveModifiers.Modifiers & Modifier.Sealed) == Modifier.Sealed;
    }

    public static bool IsUnsafe(this IHaveModifiers iHaveModifiers)
    {
        return (iHaveModifiers.Modifiers & Modifier.Unsafe) == Modifier.Unsafe;
    }

    public static bool IsVirtual(this IHaveModifiers iHaveModifiers)
    {
        return (iHaveModifiers.Modifiers & Modifier.Virtual) == Modifier.Virtual;
    }
}
