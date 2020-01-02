using System.Collections.Generic;

namespace LivingDocumentation
{
    public interface IHaveDocumentationComments
    {
        string Example { get; }

        IDictionary<string, string> Exceptions { get; }

        IDictionary<string, string> Params { get; }

        IDictionary<string, string> Permissions { get; }

        string Remarks { get; }

        string Returns { get; }

        IDictionary<string, string> SeeAlsos { get; }

        string Summary { get; }

        IDictionary<string, string> TypeParams { get; }

        string Value { get; }
    }
}
