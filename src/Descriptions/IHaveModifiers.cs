using System.Collections.Generic;

namespace roslyn_uml
{
    public interface IHaveModifiers
    {
        List<string> Modifiers { get; }
        bool IsPublic { get; }
        bool IsInternal { get; }
        bool IsPrivate { get; }
        bool IsProtected { get; }
    }
}