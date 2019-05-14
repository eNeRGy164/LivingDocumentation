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

                flowBuilder.AppendLine("Q->H ++:" + messageType.Name.FormatForDiagram());

                var handlingMethod = commandHandler.HandlingMethod(message);

                foreach (var invokedMethod in handlingMethod.InvokedMethods)
                {
                    var containingType = types.FirstOrDefault(invokedMethod.ContainingType);
                    if (containingType.IsAggregateRoot())
                    {
                        if (!aggregates.Contains(containingType.Name))
                        {
                            aggregates.Add(containingType.Name);
                            flowBuilder.AppendLine($"H->{containingType.Name} ++:{invokedMethod.Name.FormatForDiagram()}");
                        }
                        else
                        {
                            flowBuilder.AppendLine($"H->{containingType.Name}:{invokedMethod.Name.FormatForDiagram()}");
                        }

                        foreach (var call in types.GetInvocationConsequences(invokedMethod).Where(c => c.IsDomainEventCreation()))
                        {
                            var eventType = types.FirstOrDefault(call.Arguments.First().Type);
                            flowBuilder.AppendLine($"{containingType.Name}->DQ:{eventType.Name.FormatForDiagram()}");
                        }
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
                stringBuilder.AppendLine("skinparam ArrowColor DodgerBlue");
                stringBuilder.AppendLine("skinparam SequenceMessageAlignment ReverseDirection");
                stringBuilder.AppendLine("queue Queue as Q");
                stringBuilder.AppendLine($"box {commandHandlerName.FormatForDiagram()}");
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
    }
}