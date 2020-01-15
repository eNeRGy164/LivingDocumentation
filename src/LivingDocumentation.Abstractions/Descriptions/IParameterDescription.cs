using System;
using System.Collections.Generic;

namespace LivingDocumentation
{
    public interface IParameterDescription
    {
        string Type { get; }

        string Name { get; }

        bool HasDefaultValue { get; }

        List<IAttributeDescription> Attributes { get; }
    }
}
