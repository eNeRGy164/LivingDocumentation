using System;
using System.Linq;
using System.Text;

namespace roslyn_uml.eShopOnContainers
{
    public static class eShopOnContainersExtensions
    {
        const string IntegrationEvent = "IntegrationEvent";
        const string DomainEvent = "DomainEvent";
        const string DomainEventHandler = "DomainEventHandler";
        const string Command = "Command";
        const string CommandHandler = "CommandHandler";

        public static string ArrowColor(this string name)
        {
            if (name.EndsWith(IntegrationEvent))
            {
                return "[#Green]";
            }

            if (name.EndsWith(DomainEvent))
            {
                return "[#OrangeRed]";
            }

            if (name.EndsWith(Command))
            {
                return "[#DodgerBlue]";
            }

            return string.Empty;
        }

        public static string FormatForDiagram(this string name)
        {
            if (name.EndsWith(IntegrationEvent))
            {
                return name.Substring(0, name.Length - IntegrationEvent.Length);
            }

            if (name.EndsWith(DomainEvent))
            {
                return name.Substring(0, name.Length - DomainEvent.Length);
            }

            if (name.EndsWith(Command))
            {
                return name.Substring(0, name.Length - Command.Length);
            }

            if (name.EndsWith(DomainEventHandler))
            {
                return name.Substring(0, name.Length - DomainEventHandler.Length) + "\\n//<<DomainEventHandler>>//";
            }

            if (name.EndsWith(CommandHandler))
            {
                return name.Substring(0, name.Length - CommandHandler.Length) + "\\n//<<CommandHandler>>//";
            }

            return name;
        }

        public static bool IsCommandHandler(this TypeDescription type)
        {
            return !type.FullName.Contains(".IdentifiedCommandHandler") && type.GetCommandHandlerDeclaration() != null;
        }

        public static bool IsDomainEventHandler(this TypeDescription type)
        {
            return type.GetDomainEventHandlerDeclaration() != null;
        }

        public static bool IsCommand(this TypeDescription type)
        {
            return type.BaseTypes.Any(bt => bt.StartsWith("MediatR.IRequest<", StringComparison.Ordinal));
        }

        public static bool IsDomainEvent(this TypeDescription type)
        {
            return type.BaseTypes.Contains("MediatR.INotification");
        }

        public static bool IsIntegrationEvent(this TypeDescription type)
        {
            return type.BaseTypes.Contains("Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events.IntegrationEvent");
        }

        public static string GetCommandHandlerDeclaration(this TypeDescription type)
        {
            return type.BaseTypes.FirstOrDefault(bt => bt.StartsWith("MediatR.IRequestHandler", StringComparison.Ordinal));
        }

        public static string GetDomainEventHandlerDeclaration(this TypeDescription type)
        {
            return type.BaseTypes.FirstOrDefault(bt => bt.StartsWith("MediatR.INotificationHandler", StringComparison.Ordinal));
        }

        public static MethodDescription HandlingMethod(this TypeDescription type, string messageType)
        {
            return type.Methods.FirstOrDefault(m => string.Equals(m.Name, "Handle") && string.Equals(m.Parameters.First().Type, messageType));
        }

        public static bool IsDomainEventCreation(this InvocationDescription invocation)
        {
            if (string.Equals(invocation.ContainingType, "Microsoft.eShopOnContainers.Services.Ordering.Domain.Seedwork.Entity")
                && string.Equals(invocation.Name, "AddDomainEvent"))
            {
                return true;
            }

            if (string.Equals(invocation.ContainingType, "Ordering.API.Application.IntegrationEvents.IOrderingIntegrationEventService")
                && string.Equals(invocation.Name, "AddAndSaveEventAsync"))
            {
                return true;
            }

            return false;
        }

        public static void RenderProperty(this PropertyDescription property, StringBuilder stringBuilder)
        {
            if (property.IsStatic) stringBuilder.Append("{static} ");
            stringBuilder.Append(property.ToUmlVisibility());
            stringBuilder.AppendLine(property.Name);
        }

        public static void RenderStereoType(this TypeDescription type, StringBuilder stringBuilder)
        {
            stringBuilder.Append("<<");
            if (type.IsEnumeration()) stringBuilder.Append("enumeration");
            if (type.IsAggregateRoot()) stringBuilder.Append("(R, LightBlue)root");
            if (type.IsValueObject()) stringBuilder.Append("(O, Wheat)value object");
            if (type.IsEntity()) stringBuilder.Append("entity");
            stringBuilder.Append(">>");
        }

        public static void RenderMethod(this MethodDescription method, StringBuilder stringBuilder)
        {
            if (method.IsStatic) stringBuilder.Append("{static} ");
            stringBuilder.Append(method.ToUmlVisibility());
            stringBuilder.Append(method.Name);
            stringBuilder.Append('(');
            stringBuilder.AppendJoin(", ", method.Parameters.Select(s => s.Name));
            stringBuilder.AppendLine(")");
        }

        public static bool IsAggregateRoot(this TypeDescription type)
        {
            return type != null && type.Type == TypeType.Class && type.BaseTypes.Contains("Microsoft.eShopOnContainers.Services.Ordering.Domain.Seedwork.IAggregateRoot");
        }

        public static bool IsEnumeration(this TypeDescription type)
        {
            return type != null && type.Type == TypeType.Class && type.BaseTypes.Contains("Microsoft.eShopOnContainers.Services.Ordering.Domain.SeedWork.Enumeration");
        }

        public static bool IsValueObject(this TypeDescription type)
        {
            return type != null && type.Type == TypeType.Class && type.BaseTypes.Contains("Microsoft.eShopOnContainers.Services.Ordering.Domain.SeedWork.ValueObject");
        }

        public static bool IsEntity(this TypeDescription type)
        {
            return type != null && !type.IsAggregateRoot() && type.Type == TypeType.Class && type.BaseTypes.Contains("Microsoft.eShopOnContainers.Services.Ordering.Domain.Seedwork.Entity");
        }
    }
}
