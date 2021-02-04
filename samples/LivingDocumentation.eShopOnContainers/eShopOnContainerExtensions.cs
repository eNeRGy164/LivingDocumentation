using PlantUml.Builder;
using System;
using System.Linq;

namespace LivingDocumentation.eShopOnContainers
{
    public static class eShopOnContainersExtensions
    {
        private const string IntegrationEvent = "IntegrationEvent";
        private const string DomainEvent = "DomainEvent";
        private const string DomainEventHandler = "DomainEventHandler";
        private const string Command = "Command";
        private const string CommandHandler = "CommandHandler";

        public static Color ArrowColor(this string name)
        {
            if (name.EndsWith(IntegrationEvent))
            {
                return NamedColor.Green;
            }

            if (name.EndsWith(DomainEvent))
            {
                return NamedColor.OrangeRed;
            }

            if (name.EndsWith(Command))
            {
                return NamedColor.DodgerBlue;
            }

            return null;
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
            return type.ImplementsTypeStartsWith("MediatR.IRequest<");
        }

        public static bool IsDomainEvent(this TypeDescription type)
        {
            return type.ImplementsType("MediatR.INotification");
        }

        public static bool IsIntegrationEvent(this TypeDescription type)
        {
            return type.ImplementsType("Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events.IntegrationEvent");
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

        public static bool IsAggregateRoot(this TypeDescription type)
        {
            return type != null 
                && type.IsClass()
                && type.ImplementsType("Microsoft.eShopOnContainers.Services.Ordering.Domain.Seedwork.IAggregateRoot");
        }

        public static bool IsEnumeration(this TypeDescription type)
        {
            return type != null 
                && type.IsClass() 
                && type.ImplementsType("Microsoft.eShopOnContainers.Services.Ordering.Domain.SeedWork.Enumeration");
        }

        public static bool IsValueObject(this TypeDescription type)
        {
            return type != null 
                && type.IsClass() 
                && type.ImplementsType("Microsoft.eShopOnContainers.Services.Ordering.Domain.SeedWork.ValueObject");
        }

        public static bool IsEntity(this TypeDescription type)
        {
            return type != null 
                && !type.IsAggregateRoot() 
                && type.IsClass() 
                && type.ImplementsType("Microsoft.eShopOnContainers.Services.Ordering.Domain.Seedwork.Entity");
        }
    }
}
