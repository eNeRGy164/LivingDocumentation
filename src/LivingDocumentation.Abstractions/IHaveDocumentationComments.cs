using System.Collections.Generic;

namespace LivingDocumentation
{
    public interface IHaveDocumentationComments
    {
        string Code { get; }

        IDictionary<string, string> Param { get; }

        IDictionary<string, string> Permission { get; }

        string Remarks { get; }

        string Returns { get; }

        IDictionary<string, string> SeeAlso { get; }

        string Summary { get; }

        IDictionary<string, string> TypeParam { get; }

        string Value { get; }
    }
}
