using System;

namespace LivingDocumentation
{
    public interface IParameterDescription
    {
        string Type { get; }

        string Name { get; }

        bool HasDefaultValue { get; }
    }
}