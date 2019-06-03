using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LivingDocumentation.eShopOnContainers
{
    public class EventHandlerRenderer
    {
        private readonly IList<TypeDescription> types;

        public EventHandlerRenderer(IList<TypeDescription> types)
        {
            this.types = types;
        }

        public IReadOnlyDictionary<string, string> Render()
        {
            var files = new Dictionary<string, string>();

            var eventHandlers = types.Where(t => t.IsDomainEventHandler()).ToList();

            foreach (var eventHandler in eventHandlers)
            {
                var eventHandlerName = eventHandler.Name;

                var message = eventHandler.GetDomainEventHandlerDeclaration().GenericTypes().First();
                var messageType = types.FirstOrDefault(message);

                var aggregates = new List<string>();
                var flowBuilder = new StringBuilder();

                flowBuilder.AppendLine($"DQ-{messageType.Name.ArrowColor()}>H ++:{messageType.Name.FormatForDiagram()}");

                var handlingMethod = eventHandler.HandlingMethod(message);

                foreach (var statement in handlingMethod.Statements)
                {
                    TraverseInvocation(aggregates, flowBuilder, statement);
                }

                flowBuilder.AppendLine("deactivate H");

                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("@startuml");
                stringBuilder.AppendLine("skinparam lifelineStrategy solid");
                stringBuilder.AppendLine("skinparam SequenceArrowThickness 2");
                stringBuilder.AppendLine("skinparam SequenceBoxBackgroundColor SeaShell");
                stringBuilder.AppendLine("skinparam SequenceLifeLineBorderColor Black");
                stringBuilder.AppendLine("skinparam SequenceLifeLineBorderThickness 2");
                stringBuilder.AppendLine("skinparam BoxPadding 20");
                stringBuilder.AppendLine("skinparam ParticipantPadding 20");
                stringBuilder.AppendLine("skinparam ArrowColor Black");
                stringBuilder.AppendLine("skinparam SequenceMessageAlignment ReverseDirection");
                stringBuilder.AppendLine("queue Domain as DQ");
                stringBuilder.AppendLine($"box \"{eventHandlerName.FormatForDiagram()}\"");
                stringBuilder.AppendLine($"participant Handle as H");

                foreach (var aggregate in aggregates)
                {
                    stringBuilder.AppendLine($"entity {aggregate}");
                }

                stringBuilder.AppendLine("end box");
                stringBuilder.AppendLine("||5||");

                stringBuilder.Append(flowBuilder);

                foreach (var aggregate in aggregates)
                {
                    stringBuilder.AppendLine($"deactivate {aggregate}");
                }

                stringBuilder.AppendLine("||5||");
                stringBuilder.AppendLine("@enduml");

                var fileName = $"eventhandler.{eventHandlerName.ToLowerInvariant()}.puml";
                files.Add(eventHandler.FullName, fileName);

                File.WriteAllText(fileName, stringBuilder.ToString());
            }

            return files;
        }

        private void TraverseInvocation(List<string> aggregates, StringBuilder stringBuilder, Statement statement)
        {
            switch (statement)
            {
                case Switch switchStatement:
                    var switchBuilder = new StringBuilder();

                    foreach (SwitchSection section in switchStatement.Sections)
                    {
                        var sectionBuilder = new StringBuilder();

                        foreach (var invokedMethod in section.Statements.OfType<InvocationDescription>())
                        {
                            TraverseInvocation(aggregates, sectionBuilder, invokedMethod);
                        }

                        if (sectionBuilder.Length > 0)
                        {
                            var first = switchBuilder.Length == 0;
                            if (first) switchBuilder.AppendLine("||5||");
                            switchBuilder.Append(first ? "alt " : "else ");
                            switchBuilder.AppendJoin(',', section.Labels);
                            switchBuilder.AppendLine();
                            switchBuilder.AppendLine("||5||");
                            switchBuilder.Append(sectionBuilder);
                            switchBuilder.AppendLine("||5||");
                        }
                    }

                    if (switchBuilder.Length > 0)
                    {
                        stringBuilder.Append(switchBuilder);
                        stringBuilder.AppendLine("end");
                        stringBuilder.AppendLine("||5||");
                    }
                    break;

                case If ifStatement:
                    var ifBuilder = new StringBuilder();

                    foreach (IfElseSection section in ifStatement.Sections)
                    {
                        var sectionBuilder = new StringBuilder();

                        foreach (var invokedMethod in section.Statements.OfType<InvocationDescription>())
                        {
                            TraverseInvocation(aggregates, sectionBuilder, invokedMethod);
                        }

                        if (sectionBuilder.Length > 0)
                        {
                            var first = ifBuilder.Length == 0;
                            if (first) ifBuilder.AppendLine("||5||");
                            ifBuilder.Append(first ? "group if " : "else ");
                            ifBuilder.AppendLine(string.IsNullOrEmpty(section.Condition) ? string.Empty : $" [{section.Condition}]");
                            ifBuilder.AppendLine("||5||");
                            ifBuilder.Append(sectionBuilder);
                            ifBuilder.AppendLine("||5||");
                        }
                    }

                    if (ifBuilder.Length > 0)
                    {
                        stringBuilder.Append(ifBuilder);
                        stringBuilder.AppendLine("end");
                        stringBuilder.AppendLine("||5||");
                    }
                    break;

                case InvocationDescription invokedMethod:
                    {
                        var containingType = types.FirstOrDefault(invokedMethod.ContainingType);
                        if (containingType.IsAggregateRoot())
                        {
                            if (!aggregates.Contains(containingType.Name))
                            {
                                aggregates.Add(containingType.Name);
                                var prefix = string.Empty;
                                if (containingType.Name == invokedMethod.Name) prefix = "new ";
                                stringBuilder.AppendLine($"H->{containingType.Name} ++:{prefix}{invokedMethod.Name.FormatForDiagram()}");
                            }
                            else
                            {
                                stringBuilder.AppendLine($"H->{containingType.Name}:{invokedMethod.Name.FormatForDiagram()}");
                            }

                            foreach (var call in types.GetInvocationConsequenceStatements(invokedMethod).Where(s => s != invokedMethod))
                            {
                                TraverseInvocation(aggregates, stringBuilder, call);
                            }
                        }
                        else
                        {
                            foreach (var call in types.GetInvocationConsequences(invokedMethod).Where(c => c.IsDomainEventCreation()))
                            {
                                var eventType = types.FirstOrDefault(call.Arguments.First().Type);
                                stringBuilder.AppendLine($"H-{eventType.Name.ArrowColor()}>DQ:{eventType.Name.FormatForDiagram()}");
                            }
                        }
                    }

                    break;
            }
        }
    }

}