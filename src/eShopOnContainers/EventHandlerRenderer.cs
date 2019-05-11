using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace roslyn_uml.eShopOnContainers
{
    public class EventHandlerRenderer
    {
        private readonly List<TypeDescription> types;

        public EventHandlerRenderer(List<TypeDescription> types)
        {
            this.types = types;
        }

        public void Render()
        {
            var eventHandlers = types.Where(t => t.IsDomainEventHandler()).ToList();
            foreach (var eventHandler in eventHandlers)
            {
                var message = eventHandler.GetDomainEventHandlerDeclaration().GenericTypes().First();
                var messageType = types.FirstOrDefault(message);

                var aggregates = new List<string>();
                var flowBuilder = new StringBuilder();

                flowBuilder.AppendLine("DQ->H ++:" + messageType.Name.FormatForDiagram());

                var handlingMethod = eventHandler.HandlingMethod(message);

                foreach (var invokedMethod in handlingMethod.InvokedMethods)
                {
                    foreach (var call in types.GetInvocationConsequences(invokedMethod).Where(c => c.IsDomainEventCreation()))
                    {
                        var eventType = types.FirstOrDefault(call.Arguments.First().Type);
                        flowBuilder.AppendLine($"H->DQ:{eventType.Name.FormatForDiagram()}");
                    }

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
                            var eventType = types.First(t => string.Equals(t.FullName, call.Arguments.First().Type));
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
                stringBuilder.AppendLine("queue Domain as DQ");
                stringBuilder.AppendLine($"box {eventHandler.Name.FormatForDiagram()}");
                stringBuilder.AppendLine($"participant Handle as H");

                foreach (var aggregate in aggregates)
                {
                    stringBuilder.AppendLine($"entity {aggregate}");
                }

                stringBuilder.AppendLine("end box");
                stringBuilder.AppendLine("|||");

                stringBuilder.Append(flowBuilder);

                stringBuilder.AppendLine("|||");
                stringBuilder.AppendLine("@enduml");

                File.WriteAllText("eventhandler." + eventHandler.Name.ToLowerInvariant() + ".puml", stringBuilder.ToString());
            }
        }
    }
}