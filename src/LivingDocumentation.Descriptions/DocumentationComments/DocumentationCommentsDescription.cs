using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using LivingDocumentation.DocumentationComments;
using Attribute = LivingDocumentation.DocumentationComments.Attribute;

namespace LivingDocumentation
{
    public partial class DocumentationCommentsDescription : IHaveDocumentationComments
    {
        private static readonly Regex InlineWhitespace = new Regex("(\\s{2,})", RegexOptions.ECMAScript);
        private static readonly Regex MemberIdPrefix = new Regex("^[NTFPME\\!]:", RegexOptions.ECMAScript);

        [DefaultValue("")]
        public string Example { get; set; } = string.Empty;

        [DefaultValue("")]
        public string Remarks { get; set; } = string.Empty;

        [DefaultValue("")]
        public string Returns { get; set; } = string.Empty;

        [DefaultValue("")]
        public string Summary { get; set; } = string.Empty;

        [DefaultValue("")]
        public string Value { get; set; } = string.Empty;

        public IDictionary<string, string> Exceptions { get; set; } = new Dictionary<string, string>();

        public IDictionary<string, string> Permissions { get; set; } = new Dictionary<string, string>();

        public IDictionary<string, string> Params { get; set; } = new Dictionary<string, string>();

        public IDictionary<string, string> SeeAlsos { get; set; } = new Dictionary<string, string>();

        public IDictionary<string, string> TypeParams { get; set; } = new Dictionary<string, string>();

        public static DocumentationCommentsDescription Parse(string documentationCommentXml)
        {
            if (string.IsNullOrWhiteSpace(documentationCommentXml) || documentationCommentXml.StartsWith("<!--", StringComparison.Ordinal))
            {
                // No documenation or unparseable documentation
                return null;
            }

            var element = XElement.Parse(documentationCommentXml);

            var documentation = new DocumentationCommentsDescription();

            documentation.Example = documentation.ParseSection(element.Element(Section.Example));
            documentation.Remarks = documentation.ParseSection(element.Element(Section.Remarks));
            documentation.Returns = documentation.ParseSection(element.Element(Section.Returns));
            documentation.Summary = documentation.ParseSection(element.Element(Section.Summary), true);
            documentation.Value = documentation.ParseSection(element.Element(Section.Value));

            documentation.ParseSection(element.Elements(Section.Exception));
            documentation.ParseSection(element.Elements(Section.Param));
            documentation.ParseSection(element.Elements(Section.Permission));
            documentation.ParseSection(element.Elements(Section.SeeAlso));
            documentation.ParseSection(element.Elements(Section.TypeParam));

            return documentation;
        }

        private void ParseSection(IEnumerable<XElement> sections)
        {
            if (sections == null || !sections.Any())
            {
                return;
            }

            foreach (var section in sections)
            {
                switch (section.Name.LocalName)
                {
                    case Section.Exception when !string.IsNullOrWhiteSpace(section.Attribute(Attribute.CRef).Value):
                        this.Exceptions.Add(StripIDPrefix(section.Attribute(Attribute.CRef).Value), this.ParseSection(section));
                        break;

                    case Section.Param when !string.IsNullOrWhiteSpace(section.Attribute(Attribute.Name).Value):
                        this.Params.Add(StripIDPrefix(section.Attribute(Attribute.Name).Value), this.ParseSection(section));
                        break;

                    case Section.Permission when !string.IsNullOrWhiteSpace(section.Attribute(Attribute.CRef).Value):
                        this.Permissions.Add(StripIDPrefix(section.Attribute(Attribute.CRef).Value), this.ParseSection(section));
                        break;

                    case Section.SeeAlso when !string.IsNullOrWhiteSpace(section.Attribute(Attribute.CRef).Value):
                        this.ProcessSeeAlsoTag(section, false);
                        break;

                    case Section.TypeParam when !string.IsNullOrWhiteSpace(section.Attribute(Attribute.Name).Value):
                        this.TypeParams.Add(StripIDPrefix(section.Attribute(Attribute.Name).Value), this.ParseSection(section));
                        break;
                }
            }
        }

        private string ParseSection(XElement section, bool removeNewLines = false)
        {
            if (section == null || section.IsEmpty)
            {
                return string.Empty;
            }

            var contents = new StringBuilder();

            foreach (var node in section.Nodes())
            {
                switch (node)
                {
                    case XText text:
                        ProcessInlineContent(contents, text.Value, removeNewLines);
                        break;

                    case XElement element when element.Name == Block.Code || element.Name == Block.Para:
                        this.ProcessBlockContent(contents, element);
                        break;

                    case XElement element when element.Name == Block.List:
                        this.ProcessListContent(contents, element);
                        break;

                    case XElement element when element.Name == Inline.C:
                        ProcessInlineContent(contents, element.Value, removeNewLines);
                        break;

                    case XElement element when (element.Name == Inline.ParamRef || element.Name == Inline.TypeParamRef) && element.Attribute(Attribute.Name) != null:
                        ProcessInlineContent(contents, StripIDPrefix(element.Attribute(Attribute.Name).Value), removeNewLines);
                        break;

                    case XElement element when element.Name == Inline.See:
                        ProcessInlineContent(contents, element.IsEmpty ? StripIDPrefix(element.Attribute(Attribute.CRef)?.Value) : element.Value, removeNewLines);
                        break;

                    case XElement element when element.Name == Section.SeeAlso:
                        this.ProcessSeeAlsoTag(element, removeNewLines);
                        break;

                    case XElement element when !element.IsEmpty:
                        ProcessInlineContent(contents, element.Value, removeNewLines);
                        break;
                }
            }

            return contents.ToString().Trim();
        }

        private void ProcessListContent(StringBuilder contents, XElement element)
        {
            var listType = element.Attribute(Attribute.Type)?.Value;
            switch (listType)
            {
                case ListType.Bullet:
                    foreach (var item in element.Elements(List.Item))
                    {
                        var term = this.ParseSection(item.Element(List.Term));
                        var description = this.ParseSection(item.Element(List.Description));

                        contents.Append("* ");

                        if (!string.IsNullOrEmpty(term))
                        {
                            contents.Append(term);
                            contents.Append(" - ");
                        }

                        if (!string.IsNullOrEmpty(description))
                        {
                            contents.Append(description);
                        }

                        if (string.IsNullOrEmpty(term) && string.IsNullOrEmpty(description))
                        {
                            contents.Append(item.Value.Trim());
                        }

                        contents.Append('\n');

                    }
                    break;

                case ListType.Number:
                    if (!int.TryParse(element.Attribute(Attribute.Start)?.Value.Trim() ?? "1", out var startIndex))
                    {
                        startIndex = 1;
                    }

                    foreach (var item in element.Elements(List.Item))
                    {
                        var term = this.ParseSection(item.Element(List.Term));
                        var description = this.ParseSection(item.Element(List.Description));

                        contents.Append(startIndex);
                        contents.Append(". ");

                        if (!string.IsNullOrEmpty(term))
                        {
                            contents.Append(term);
                            contents.Append(" - ");
                        }

                        if (!string.IsNullOrEmpty(description))
                        {
                            contents.Append(description);
                        }

                        if (string.IsNullOrEmpty(term) && string.IsNullOrEmpty(description))
                        {
                            contents.Append(item.Value.Trim());
                        }

                        contents.Append('\n');

                        startIndex++;
                    }
                    break;

                case ListType.Definition:
                    foreach (var item in element.Elements(List.Item))
                    {
                        var term = this.ParseSection(item.Element(List.Term));
                        var description = this.ParseSection(item.Element(List.Description));

                        contents.Append(term);
                        contents.Append('\n');
                        contents.Append(new string(' ', 4));
                        contents.Append(description);
                        contents.Append('\n');
                    }
                    break;

                default:
                    contents.Append(string.Join(" ", element.Descendants().Select(e => this.ParseSection(e))));
                    break;
            }
        }

        private static void ProcessInlineContent(StringBuilder stringBuilder, string text, bool removeNewLines)
        {
            if (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] != ' ' && stringBuilder[stringBuilder.Length - 1] != '\n')
            {
                stringBuilder.Append(' ');
            }

            if (removeNewLines)
            {
                stringBuilder.Append(InlineWhitespace.Replace(text, " ").Trim());
            }
            else
            {
                stringBuilder.Append(string.Join("\n", text.Split('\n').Select(t => t.Trim())));
            }
        }

        private void ProcessBlockContent(StringBuilder stringBuilder, XElement element)
        {
            if (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] != '\n')
            {
                stringBuilder.Append('\n');
            }

            stringBuilder.Append(this.ParseSection(element));
            stringBuilder.Append('\n');
        }

        private void ProcessSeeAlsoTag(XElement element, bool removeNewLines)
        {
            var key = StripIDPrefix(element.Attribute(Attribute.CRef).Value);

            var contents = new StringBuilder();

            ProcessInlineContent(contents, element.Value, removeNewLines);

            var value = contents.ToString().Trim();

            this.SeeAlsos.Add(key, !string.IsNullOrEmpty(value) ? value : key);
        }

        private static string StripIDPrefix(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return MemberIdPrefix.Replace(value, string.Empty);
        }
    }
}
