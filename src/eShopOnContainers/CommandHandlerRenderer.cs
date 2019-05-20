using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace roslyn_uml.eShopOnContainers
{
    public class CommandHandlerRenderer
    {
        private readonly IList<TypeDescription> types;

        public CommandHandlerRenderer(IList<TypeDescription> types)
        {
            this.types = types;
        }

        public IReadOnlyDictionary<string, string> Render()
        {
            var files = new Dictionary<string, string>();

            var commandHandlers = types.Where(t => t.IsCommandHandler()).ToList();

            foreach (var commandHandler in commandHandlers)
            {
                var commandHandlerName = commandHandler.Name;

                var message = commandHandler.GetCommandHandlerDeclaration().GenericTypes().First();
                var messageType = types.FirstOrDefault(message);

                var aggregates = new List<string>();
                var flowBuilder = new StringBuilder();

                flowBuilder.AppendLine($"Q-{messageType.Name.ArrowColor()}>H ++:{messageType.Name.FormatForDiagram()}");

                var handlingMethod = commandHandler.HandlingMethod(message);

                foreach (var statement in handlingMethod.Statements)
                {
                    switch (statement)
                    {
                        case Switch switchStatement:
                            var switchBuilder = new StringBuilder();

                            foreach (SwitchSection section in switchStatement.Sections)
                            {
                                var sectionBuilder = new StringBuilder();

                                foreach (var invocation in section.Statements.OfType<InvocationDescription>())
                                {
                                    TraverseInvocation(aggregates, sectionBuilder, invocation);
                                }

                                if (sectionBuilder.Length > 0)
                                {
                                    var first = switchBuilder.Length == 0;
                                    if (first) switchBuilder.AppendLine("|||");
                                    switchBuilder.Append(first ? "alt " : "else ");
                                    switchBuilder.AppendJoin(',', section.Labels);
                                    switchBuilder.AppendLine();
                                    switchBuilder.AppendLine("|||");
                                    switchBuilder.Append(sectionBuilder);
                                    switchBuilder.AppendLine("|||");
                                }
                            }

                            if (switchBuilder.Length > 0)
                            {
                                flowBuilder.Append(switchBuilder);
                                flowBuilder.AppendLine("end");
                                flowBuilder.AppendLine("|||");
                            }
                            break;

                        case If ifStatement:
                            var ifBuilder = new StringBuilder();

                            foreach (IfElseSection section in ifStatement.Sections)
                            {
                                var sectionBuilder = new StringBuilder();

                                foreach (var invocation in section.Statements.OfType<InvocationDescription>())
                                {
                                    TraverseInvocation(aggregates, sectionBuilder, invocation);
                                }

                                if (sectionBuilder.Length > 0)
                                {
                                    var first = ifBuilder.Length == 0;
                                    if (first) ifBuilder.AppendLine("|||");
                                    ifBuilder.Append(first ? "alt " : "else ");
                                    ifBuilder.AppendLine(section.Condition ?? "");
                                    ifBuilder.AppendLine("|||");
                                    ifBuilder.Append(sectionBuilder);
                                    ifBuilder.AppendLine("|||");
                                }
                            }

                            if (ifBuilder.Length > 0)
                            {
                                flowBuilder.Append(ifBuilder);
                                flowBuilder.AppendLine("end");
                                flowBuilder.AppendLine("|||");
                            }
                            break;

                        case InvocationDescription invocation:
                            TraverseInvocation(aggregates, flowBuilder, invocation);
                            break;
                    }
                }

                flowBuilder.AppendLine("deactivate H");

                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("@startuml");
                stringBuilder.AppendLine("skinparam lifelineStrategy solid");
                stringBuilder.AppendLine("skinparam SequenceArrowThickness 2");
                stringBuilder.AppendLine("skinparam SequenceBoxBackgroundColor SeaShell");
                stringBuilder.AppendLine("skinparam SequenceLifeLineBorderColor Black");
                stringBuilder.AppendLine("skinparam SequenceLifeLineBorderThickness 2");
                stringBuilder.AppendLine("skinparam ArrowColor Black");
                stringBuilder.AppendLine("skinparam SequenceMessageAlignment ReverseDirection");
                stringBuilder.AppendLine("queue Queue as Q");
                stringBuilder.AppendLine($"box \"{commandHandlerName.FormatForDiagram()}\"");
                stringBuilder.AppendLine($"participant Handle as H");

                foreach (var aggregate in aggregates)
                {
                    stringBuilder.AppendLine($"entity {aggregate}");
                }

                stringBuilder.AppendLine("end box");
                stringBuilder.AppendLine("queue Domain as DQ");
                stringBuilder.AppendLine("|||");

                stringBuilder.Append(flowBuilder);

                stringBuilder.AppendLine("|||");
                stringBuilder.AppendLine("@enduml");

                var fileName = $"commandhandler.{commandHandlerName.ToLowerInvariant()}.puml";
                files.Add(commandHandler.FullName, fileName);

                File.WriteAllText(fileName, stringBuilder.ToString());
            }

            return files;
        }

        private void TraverseInvocation(List<string> aggregates, StringBuilder stringBuilder, InvocationDescription invocation)
        {
            var containingType = types.FirstOrDefault(invocation.ContainingType);
            if (containingType.IsAggregateRoot())
            {
                if (!aggregates.Contains(containingType.Name))
                {
                    aggregates.Add(containingType.Name);
                    stringBuilder.AppendLine($"H->{containingType.Name} ++:{invocation.Name.FormatForDiagram()}");
                }
                else
                {
                    stringBuilder.AppendLine($"H->{containingType.Name}:{invocation.Name.FormatForDiagram()}");
                }

                foreach (var call in types.GetInvocationConsequences(invocation).Where(c => c.IsDomainEventCreation()))
                {
                    var eventType = types.FirstOrDefault(call.Arguments.First().Type);
                    stringBuilder.AppendLine($"{containingType.Name}-{eventType.Name.ArrowColor()}>DQ:{eventType.Name.FormatForDiagram()}");
                }
            }
        }
    }
}