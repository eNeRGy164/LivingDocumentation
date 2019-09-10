using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using LivingDocumentation.DocumentationComments;

namespace LivingDocumentation
{
    public partial class DocumentationCommentsDescription : IHaveDocumentationComments
    {
        private static readonly Regex InlineWhitespace = new Regex("(\\s{2,})", RegexOptions.ECMAScript);
        private static readonly Regex MemberIdPrefix = new Regex("^[NTFPME\\!]:", RegexOptions.ECMAScript);

        [DefaultValue("")]
        public string Summary { get; set; } = string.Empty;

        [DefaultValue("")]
        public string Remarks { get; set; } = string.Empty;

        [DefaultValue("")]
        public string Value { get; set; } = string.Empty;

        [DefaultValue("")]
        public string Code { get; set; } = string.Empty;

        [DefaultValue("")]
        public string Returns { get; set; } = string.Empty;

        public IDictionary<string, string> Permission { get; set; } = new Dictionary<string, string>();

        public IDictionary<string, string> Param { get; set; } = new Dictionary<string, string>();

        public IDictionary<string, string> SeeAlso { get; set; } = new Dictionary<string, string>();

        public IDictionary<string, string> TypeParam { get; set; } = new Dictionary<string, string>();

        public static DocumentationCommentsDescription Parse(string documentationCommentXml)
        {
            if (string.IsNullOrWhiteSpace(documentationCommentXml) || documentationCommentXml.StartsWith("<!--", StringComparison.Ordinal))
            {
                // No documenation or unparseable documentation
                return null;
            }

            var documentation = new DocumentationCommentsDescription();

            var element = XElement.Parse(documentationCommentXml);

            documentation.Code = documentation.ParseSection(element.Element("code"));
            documentation.Remarks = documentation.ParseSection(element.Element("remarks"));
            documentation.Returns = documentation.ParseSection(element.Element("returns"));
            documentation.Summary = documentation.ParseSection(element.Element("summary"));
            documentation.Value = documentation.ParseSection(element.Element("value"));

            return documentation;
        }

        private string ParseSection(XElement section)
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
                        ProcessInlineContent(contents, text.Value);
                        break;

                    case XElement element when element.Name == Block.Code || element.Name == Block.List || element.Name == Block.Para:
                        ProcessBlockContent(contents, element.Value);
                        break;

                    case XElement element when element.Name == Inline.C:
                        ProcessInlineContent(contents, element.Value);
                        break;

                    case XElement element when element.Name == Inline.ParamRef || element.Name == Inline.TypeParamRef:
                        ProcessInlineContent(contents, this.StripIDPrefix(element.Attribute(Argument.Name)?.Value));
                        break;

                    case XElement element when element.Name == Inline.See:
                        ProcessInlineContent(contents, element.IsEmpty ? this.StripIDPrefix(element.Attribute(Argument.CRef)?.Value) : element.Value);
                        break;

                    case XElement element when !element.IsEmpty:
                        ProcessInlineContent(contents, element.Value);
                        break;
                }
            }

            return contents.ToString().Trim();
        }

        private static void ProcessInlineContent(StringBuilder stringBuilder, string text)
        {
            if (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] != ' ' && stringBuilder[stringBuilder.Length - 1] != '\n')
            {
                stringBuilder.Append(' ');
            }

            stringBuilder.Append(InlineWhitespace.Replace(text, " ").Trim());
        }

        private static void ProcessBlockContent(StringBuilder stringBuilder, string text)
        {
            if (stringBuilder.Length > 0 && stringBuilder[stringBuilder.Length - 1] != '\n')
            {
                stringBuilder.Append('\n');
            }

            stringBuilder.Append(InlineWhitespace.Replace(text, " "));
            stringBuilder.Append('\n');
        }

        private string StripIDPrefix(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return MemberIdPrefix.Replace(value, string.Empty);
        }
    }
}
