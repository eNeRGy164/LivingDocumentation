﻿using System.Collections.Generic;
using System.Diagnostics;

namespace LivingDocumentation
{
    [DebuggerDisplay("Invocation {Name} {ContainingType}")]
    public class InvocationDescription : Statement
    {
        public string ContainingType { get; }

        public string Name { get; }

        public List<ArgumentDescription> Arguments { get; } = new List<ArgumentDescription>();

        public InvocationDescription(string containingType, string name)
        {
            this.ContainingType = containingType;
            this.Name = name;
        }
    }
}