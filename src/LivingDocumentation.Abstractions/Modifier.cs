using System;

namespace LivingDocumentation
{
    [Flags]
    public enum Modifier
    {
        Internal = 0000_0000_0001,

        Public = 0000_0000_0010,

        Private = 0000_0000_0100,

        Protected = 0000_0000_1000,

        Static = 0000_0001_0000,

        Abstract = 0000_0010_0000,

        Override = 0000_0100_0000,

        Readonly = 0000_1000_0000,

        Async = 0001_0000_0000,
    }
}
