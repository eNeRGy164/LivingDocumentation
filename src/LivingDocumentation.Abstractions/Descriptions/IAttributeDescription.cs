using System;
using System.Collections.Generic;

namespace LivingDocumentation
{
    public interface IAttributeDescription
    {
        string Type { get; }

        string Name { get; }

        List<IAttributeArgumentDescription> Arguments { get; }
    }
}
