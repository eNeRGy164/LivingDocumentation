using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using roslyn_uml;

namespace roslyn_uml.eShopOnContainers
{
    public class AsciiDocRenderer
    {
        private readonly IReadOnlyList<TypeDescription> types;
        private readonly IReadOnlyDictionary<string, string> aggregateFiles;
        private readonly IReadOnlyDictionary<string, string> commandHandlerFiles;
        private readonly IReadOnlyDictionary<string, string> eventHandlerFiles;

        public AsciiDocRenderer(IReadOnlyList<TypeDescription> types, IReadOnlyDictionary<string, string> aggregateFiles, IReadOnlyDictionary<string, string> commandHandlerFiles, IReadOnlyDictionary<string, string> eventHandlerFiles)
        {
            this.types = types;
            this.aggregateFiles = aggregateFiles;
            this.commandHandlerFiles = commandHandlerFiles;
            this.eventHandlerFiles = eventHandlerFiles;
        }

        public void Render()
        {
            var stringBuilder = new StringBuilder();

            RenderFileHeader(stringBuilder);

            RenderAggregates(stringBuilder);
            RenderCommandHandlers(stringBuilder);
            RenderEventHandlers(stringBuilder);

            File.WriteAllText("documentation.generated.adoc", stringBuilder.ToString());
        }

        private void RenderAggregates(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("// tag::aggregates[]");
            stringBuilder.AppendLine("== Aggregates");
            stringBuilder.AppendLine("Aggregates in the eShop application.");

            foreach (var (type, path) in this.aggregateFiles.Select(kv => (Type: this.types.FirstOrDefault(kv.Key), Path: kv.Value)).OrderBy(t => t.Type.Name))
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"// tag::aggregate-{type.Name.ToLower()}[]");
                stringBuilder.AppendLine($"=== {type.Name}");
                stringBuilder.AppendLine($"The \"`{type.Name.ToLower()}`\" aggregate.");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($".{FormatTechnicalName(type.Name)}");
                stringBuilder.AppendLine($"[plantuml, aggregate.{type.Name.ToLowerInvariant()}, png]");
                stringBuilder.AppendLine("....");
                stringBuilder.AppendLine($"include::{path}[]");
                stringBuilder.AppendLine("....");
                stringBuilder.AppendLine($"// end::aggregate-{type.Name.ToLower()}[]");
            }

            stringBuilder.AppendLine("// end::aggregates[]");
        }

        private void RenderCommandHandlers(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("// tag::commandhandlers[]");
            stringBuilder.AppendLine("== Command Handlers");
            stringBuilder.AppendLine("Command handlers in the eShop application.");

            foreach (var (type, path) in this.commandHandlerFiles.Select(kv => (Type: this.types.FirstOrDefault(kv.Key), Path: kv.Value)).OrderBy(t => t.Type.Name))
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"// tag::commandhandler-{type.Name.ToLower()}[]");
                stringBuilder.AppendLine($"=== {FormatTechnicalName(type.Name)}");
                stringBuilder.AppendLine($"The \"`{FormatTechnicalName(type.Name).ToLower()}`\" command handler.");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($".{FormatTechnicalName(type.Name)}");
                stringBuilder.AppendLine($"[plantuml, commandhandler.{type.Name.ToLowerInvariant()}, png]");
                stringBuilder.AppendLine("....");
                stringBuilder.AppendLine($"include::{path}[]");
                stringBuilder.AppendLine("....");
                stringBuilder.AppendLine($"// end::commandhandler-{type.Name.ToLower()}[]");
            }

            stringBuilder.AppendLine("// end::commandhandlers[]");
        }

        private void RenderEventHandlers(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("// tag::eventhandlers[]");
            stringBuilder.AppendLine("== Event Handlers");
            stringBuilder.AppendLine("Event handlers in the eShop application.");

            foreach (var (type, path) in this.eventHandlerFiles.Select(kv => (Type: this.types.FirstOrDefault(kv.Key), Path: kv.Value)).OrderBy(t => t.Type.Name))
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"// tag::eventhandler-{type.Name.ToLower()}[]");
                stringBuilder.AppendLine($"=== {FormatTechnicalName(type.Name)}");
                stringBuilder.AppendLine($"The \"`{FormatTechnicalName(type.Name).ToLower()}`\" event handler.");

                if (!string.IsNullOrWhiteSpace(type.Documentation))
                {
                    stringBuilder.AppendLine(type.Documentation);
                    stringBuilder.AppendLine();
                }

                stringBuilder.AppendLine();
                stringBuilder.AppendLine($".{FormatTechnicalName(type.Name)}");
                stringBuilder.AppendLine($"[plantuml, eventhandler.{type.Name.ToLowerInvariant()}, png]");
                stringBuilder.AppendLine("....");
                stringBuilder.AppendLine($"include::{path}[]");
                stringBuilder.AppendLine("....");
                stringBuilder.AppendLine($"// end::eventhandler-{type.Name.ToLower()}[]");
            }

            stringBuilder.AppendLine("// end::eventhandlers[]");
        }

        private static void RenderFileHeader(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("= Generated documentation");
            stringBuilder.AppendLine("Michaël Hompus <michael@hompus.nl>");
            stringBuilder.AppendLine($"{Assembly.GetEntryAssembly().GetName().Version.ToString(3)}, {DateTime.Today:yyyy-MM-dd}");
            stringBuilder.AppendLine(":toc: left");
            stringBuilder.AppendLine(":toc-level: 2");
            stringBuilder.AppendLine(":sectnums:");
            stringBuilder.AppendLine(":icons: font");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("NOTE: This document has been automatically generated");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("The documentation is generated from the source code of https://github.com/dotnet-architecture/eShopOnContainers[eShopOnContainers^].");
            stringBuilder.AppendLine();
        }

        private static string FormatTechnicalName(string topic)
        {
            topic = topic.Replace("CommandHandler", "", StringComparison.Ordinal);
            topic = topic.Replace("DomainEventHandler", "", StringComparison.Ordinal);

            return topic.ToSentenceCase();
        }
    }
}
