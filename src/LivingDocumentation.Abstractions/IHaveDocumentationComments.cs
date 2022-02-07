namespace LivingDocumentation; 

public interface IHaveDocumentationComments
{
    string Example { get; }

    Dictionary<string, string> Exceptions { get; }

    Dictionary<string, string> Params { get; }

    Dictionary<string, string> Permissions { get; }

    string Remarks { get; }

    string Returns { get; }

    Dictionary<string, string> SeeAlsos { get; }

    string Summary { get; }

    Dictionary<string, string> TypeParams { get; }

    string Value { get; }
}
