using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using roslyn_uml;

namespace roslyn_uml.eShopOnContainers
{
    public class AsciiDocRenderer
    {
        private readonly IReadOnlyList<TypeDescription> types;
        private readonly IReadOnlyDictionary<string, string> aggregateFiles;
        private readonly IReadOnlyDictionary<string, string> commandHandlerFiles;
        private readonly IReadOnlyDictionary<string, string> eventHandlerFiles;
        private static readonly Regex replaceTypeSuffix = new Regex("(?:(?:Command|(?:Domain|Integration)Event))(?:Handler)?$", RegexOptions.CultureInvariant);

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
            RenderCommands(stringBuilder);
            RenderCommandHandlers(stringBuilder);
            RenderDomainEvents(stringBuilder);
            RenderDomainEventHandlers(stringBuilder);
            RenderIntegrationEvents(stringBuilder);

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
                stringBuilder.AppendLine($"// tag::aggregate-{StripTypeSuffix(type.Name).ToLowerInvariant()}[]");
                stringBuilder.AppendLine($"=== {type.Name}");
                stringBuilder.AppendLine($"The \"`{type.Name.ToLower()}`\" aggregate.");
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($".{FormatTechnicalName(type.Name)}");
                stringBuilder.AppendLine($"[plantuml, aggregate.{StripTypeSuffix(type.Name).ToLowerInvariant()}, png]");
                stringBuilder.AppendLine("....");
                stringBuilder.AppendLine($"include::{path}[]");
                stringBuilder.AppendLine("....");
                stringBuilder.AppendLine($"// end::aggregate-{StripTypeSuffix(type.Name).ToLowerInvariant()}[]");
            }

            stringBuilder.AppendLine("// end::aggregates[]");
        }

        private void RenderCommands(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("// tag::commands[]");
            stringBuilder.AppendLine("== Commands");
            stringBuilder.AppendLine("Commands in the eShop application.");

            foreach (var type in this.types.Where(t => t.IsCommand() && !t.FullName.IsGeneric()).OrderBy(t => t.Name))
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"=== {FormatTechnicalName(type.Name)}");
                stringBuilder.AppendLine($"The \"`{FormatTechnicalName(type.Name).ToLower()}`\" command.");
                stringBuilder.AppendLine();

                if (!string.IsNullOrWhiteSpace(type.Documentation))
                {
                    stringBuilder.AppendLine(type.Documentation);
                    stringBuilder.AppendLine();
                }

                stringBuilder.AppendLine($".{type.Name.ToSentenceCase()} Fields");
                stringBuilder.AppendLine("[%header%,width=\"75%\",cols=\"2h,3d\"]");
                stringBuilder.AppendLine("|===");
                stringBuilder.AppendLine("|Name|Type");

                foreach (var property in type.Properties)
                {
                    stringBuilder.AppendLine($"|{property.Name}|{property.Type.ForDiagram()}");
                }

                stringBuilder.AppendLine("|===");
            }

            stringBuilder.AppendLine("// end::commands[]");
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
                stringBuilder.AppendLine($"// tag::commandhandler-{StripTypeSuffix(type.Name).ToLowerInvariant()}[]");
                stringBuilder.AppendLine($"=== {FormatTechnicalName(type.Name)}");
                stringBuilder.AppendLine($"The \"`{FormatTechnicalName(type.Name).ToLower()}`\" command handler.");

                if (!string.IsNullOrWhiteSpace(type.Documentation))
                {
                    stringBuilder.AppendLine(type.Documentation);
                    stringBuilder.AppendLine();
                }

                stringBuilder.AppendLine();
                stringBuilder.AppendLine($".{FormatTechnicalName(type.Name)}");
                stringBuilder.AppendLine($"[plantuml, commandhandler.{StripTypeSuffix(type.Name).ToLowerInvariant()}, png]");
                stringBuilder.AppendLine("....");
                stringBuilder.AppendLine($"include::{path}[]");
                stringBuilder.AppendLine("....");
                stringBuilder.AppendLine($"// end::commandhandler-{StripTypeSuffix(type.Name).ToLowerInvariant()}[]");
            }

            stringBuilder.AppendLine("// end::commandhandlers[]");
        }

        private void RenderDomainEvents(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("// tag::domainevents[]");
            stringBuilder.AppendLine("== Domain Events");
            stringBuilder.AppendLine("Domain events in the eShop application.");

            foreach (var type in this.types.Where(t => t.IsDomainEvent()).OrderBy(t => t.Name))
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"=== {FormatTechnicalName(type.Name)}");
                stringBuilder.AppendLine($"The \"`{FormatTechnicalName(type.Name).ToLower()}`\" domain event.");
                stringBuilder.AppendLine();

                if (!string.IsNullOrWhiteSpace(type.Documentation))
                {
                    stringBuilder.AppendLine(type.Documentation);
                    stringBuilder.AppendLine();
                }

                stringBuilder.AppendLine($".{type.Name.ToSentenceCase()} Fields");
                stringBuilder.AppendLine("[%header%,width=\"75%\",cols=\"2h,3d\"]");
                stringBuilder.AppendLine("|===");
                stringBuilder.AppendLine("|Name|Type");

                foreach (var property in type.Properties)
                {
                    stringBuilder.AppendLine($"|{property.Name}|{property.Type.ForDiagram()}");
                }

                stringBuilder.AppendLine("|===");
            }

            stringBuilder.AppendLine("// end::domainevents[]");
        }

        private void RenderDomainEventHandlers(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("// tag::domaineventhandlers[]");
            stringBuilder.AppendLine("== Domain Event Handlers");
            stringBuilder.AppendLine("Domain event handlers in the eShop application.");

            foreach (var (type, path) in this.eventHandlerFiles.Select(kv => (Type: this.types.FirstOrDefault(kv.Key), Path: kv.Value)).OrderBy(t => t.Type.Name))
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"// tag::domaineventhandler-{StripTypeSuffix(type.Name).ToLowerInvariant()}[]");
                stringBuilder.AppendLine($"=== {FormatTechnicalName(type.Name)}");
                stringBuilder.AppendLine($"The \"`{FormatTechnicalName(type.Name).ToLower()}`\" event handler.");

                if (!string.IsNullOrWhiteSpace(type.Documentation))
                {
                    stringBuilder.AppendLine(type.Documentation);
                    stringBuilder.AppendLine();
                }

                stringBuilder.AppendLine();
                stringBuilder.AppendLine($".{FormatTechnicalName(type.Name)}");
                stringBuilder.AppendLine($"[plantuml, domaineventhandler.{StripTypeSuffix(type.Name).ToLowerInvariant()}, png]");
                stringBuilder.AppendLine("....");
                stringBuilder.AppendLine($"include::{path}[]");
                stringBuilder.AppendLine("....");
                stringBuilder.AppendLine($"// end::domaineventhandler-{StripTypeSuffix(type.Name).ToLowerInvariant()}[]");
            }

            stringBuilder.AppendLine("// end::domaineventhandlers[]");
        }
        private void RenderIntegrationEvents(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("// tag::integrationevents[]");
            stringBuilder.AppendLine("== Integration Events");
            stringBuilder.AppendLine("Integration events in the eShop application.");

            foreach (var type in this.types.Where(t => t.IsIntegrationEvent() && t.FullName.StartsWith("Ordering.API", StringComparison.Ordinal)).OrderBy(t => t.Name))
            {
                stringBuilder.AppendLine();
                stringBuilder.AppendLine($"=== {FormatTechnicalName(type.Name)}");
                stringBuilder.AppendLine($"The \"`{FormatTechnicalName(type.Name).ToLower()}`\" integration event.");
                stringBuilder.AppendLine();

                if (!string.IsNullOrWhiteSpace(type.Documentation))
                {
                    stringBuilder.AppendLine(type.Documentation);
                    stringBuilder.AppendLine();
                }

                stringBuilder.AppendLine($".{type.Name.ToSentenceCase()} Fields");
                stringBuilder.AppendLine("[%header%,width=\"75%\",cols=\"2h,3d\"]");
                stringBuilder.AppendLine("|===");
                stringBuilder.AppendLine("|Name|Type");

                foreach (var property in type.Properties)
                {
                    stringBuilder.AppendLine($"|{property.Name}|{property.Type.ForDiagram()}");
                }

                stringBuilder.AppendLine("|===");
            }

            stringBuilder.AppendLine("// end::integrationevents[]");
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

        private static string StripTypeSuffix(string name)
        {
            return replaceTypeSuffix.Replace(name, string.Empty);
        }

        private static string FormatTechnicalName(string name)
        {
            return StripTypeSuffix(name).ToSentenceCase();
        }
    }
}
