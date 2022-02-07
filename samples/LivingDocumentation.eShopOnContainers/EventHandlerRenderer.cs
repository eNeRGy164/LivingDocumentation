using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PlantUml.Builder;
using PlantUml.Builder.SequenceDiagrams;

namespace LivingDocumentation.eShopOnContainers
{
    public class EventHandlerRenderer
    {
        public IReadOnlyDictionary<string, string> Render()
        {
            var files = new Dictionary<string, string>();

            var eventHandlers = Program.Types.Where(t => t.IsDomainEventHandler()).ToList();

            foreach (var eventHandler in eventHandlers)
            {
                var eventHandlerName = eventHandler.Name;

                var message = eventHandler.GetDomainEventHandlerDeclaration().GenericTypes().First();
                var messageType = Program.Types.FirstOrDefault(message);

                var aggregates = new List<string>();
                var flowBuilder = new StringBuilder();

                flowBuilder.Arrow("DQ", Arrow.AsyncRight.Color(messageType.Name.ArrowColor()), "H", messageType.Name.FormatForDiagram(), LifeLineEvents.ActivateTarget);

                var handlingMethod = eventHandler.HandlingMethod(message);

                foreach (var statement in handlingMethod.Statements)
                {
                    this.TraverseInvocation(aggregates, flowBuilder, statement);
                }

                flowBuilder.Deactivate("H");

                var stringBuilder = new StringBuilder();
                stringBuilder.UmlDiagramStart();
                stringBuilder.SkinParameter(SkinParameter.LifelineStrategy, "solid");
                stringBuilder.SkinParameter(SkinParameter.SequenceArrowThickness, "2");
                stringBuilder.SkinParameter(SkinParameter.SequenceBoxBackgroundColor, "SeaShell");
                stringBuilder.SkinParameter(SkinParameter.SequenceLifeLineBorderColor, "Black");
                stringBuilder.SkinParameter(SkinParameter.SequenceLifeLineBorderThickness, "2");
                stringBuilder.SkinParameter(SkinParameter.BoxPadding, "20");
                stringBuilder.SkinParameter(SkinParameter.ParticipantPadding, "20");
                stringBuilder.SkinParameter(SkinParameter.ArrowColor, "Black");
                stringBuilder.SkinParameter(SkinParameter.SequenceMessageAlignment, "ReverseDirection");
                stringBuilder.Queue("DQ", displayName: "Domain");
                stringBuilder.BoxStart(eventHandlerName.FormatForDiagram());
                stringBuilder.Participant("H", displayName: "Handle");

                foreach (var aggregate in aggregates)
                {
                    stringBuilder.Entity(aggregate);
                }

                stringBuilder.BoxEnd();
                stringBuilder.Space(5);

                stringBuilder.Append(flowBuilder);

                foreach (var aggregate in aggregates)
                {
                    stringBuilder.Deactivate(aggregate);
                }

                stringBuilder.Space(5);
                stringBuilder.UmlDiagramEnd();

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
                            this.TraverseInvocation(aggregates, sectionBuilder, invokedMethod);
                        }

                        if (sectionBuilder.Length > 0)
                        {
                            if (switchBuilder.Length == 0)
                            {
                                switchBuilder.Space(5);
                                switchBuilder.AltStart(string.Join(',', section.Labels));
                            }
                            else
                            {
                                switchBuilder.ElseStart();
                            }

                            switchBuilder.Space(5);
                            switchBuilder.Append(sectionBuilder);
                            switchBuilder.Space(5);
                        }
                    }

                    if (switchBuilder.Length > 0)
                    {
                        stringBuilder.Append(switchBuilder);
                        stringBuilder.GroupEnd();
                        stringBuilder.Space(5);
                    }
                    break;

                case If ifStatement:
                    var ifBuilder = new StringBuilder();

                    foreach (IfElseSection section in ifStatement.Sections)
                    {
                        var sectionBuilder = new StringBuilder();

                        foreach (var invokedMethod in section.Statements.OfType<InvocationDescription>())
                        {
                            this.TraverseInvocation(aggregates, sectionBuilder, invokedMethod);
                        }

                        if (sectionBuilder.Length > 0)
                        {
                            if (ifBuilder.Length == 0)
                            {
                                ifBuilder.Space(5);
                                ifBuilder.GroupStart(label: "if", text: section.Condition);
                            }
                            else
                            {
                                ifBuilder.ElseStart(section.Condition);
                            }
                            
                            ifBuilder.Space(5);
                            ifBuilder.Append(sectionBuilder);
                            ifBuilder.Space(5);
                        }
                    }

                    if (ifBuilder.Length > 0)
                    {
                        stringBuilder.Append(ifBuilder);
                        stringBuilder.GroupEnd();
                        stringBuilder.Space(5);
                    }
                    break;

                case InvocationDescription invokedMethod:
                    {
                        var containingType = Program.Types.FirstOrDefault(invokedMethod.ContainingType);
                        if (containingType.IsAggregateRoot())
                        {
                            if (!aggregates.Contains(containingType.Name))
                            {
                                aggregates.Add(containingType.Name);
                                var prefix = (containingType.Name == invokedMethod.Name) ? "new " : string.Empty;
                                stringBuilder.Arrow("H", Arrow.AsyncRight, containingType.Name, prefix + invokedMethod.Name.FormatForDiagram(), LifeLineEvents.ActivateTarget);
                            }
                            else
                            {
                                stringBuilder.Arrow("H", Arrow.AsyncRight, containingType.Name, invokedMethod.Name.FormatForDiagram());
                            }

                            foreach (var call in Program.Types.GetInvocationConsequenceStatements(invokedMethod).Where(s => s != invokedMethod))
                            {
                                this.TraverseInvocation(aggregates, stringBuilder, call);
                            }
                        }
                        else
                        {
                            foreach (var call in Program.Types.GetInvocationConsequences(invokedMethod).Where(c => c.IsDomainEventCreation()))
                            {
                                var eventType = Program.Types.FirstOrDefault(call.Arguments.First().Type);
                                stringBuilder.Arrow("H", Arrow.AsyncRight.Color(eventType.Name.ArrowColor()), "DQ", eventType.Name.FormatForDiagram());
                            }
                        }
                    }

                    break;
            }
        }
    }
}
