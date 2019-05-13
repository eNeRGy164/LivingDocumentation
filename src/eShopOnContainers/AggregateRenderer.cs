using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace roslyn_uml.eShopOnContainers
{
    public class AggregateRenderer
    {
        private readonly IList<TypeDescription> types;

        public AggregateRenderer(IList<TypeDescription> types)
        {
            this.types = types;
        }

        public IReadOnlyDictionary<string, string> Render()
        {
            var files = new Dictionary<string, string>();

            var aggregates = this.types.Where(t => t.IsAggregateRoot()).ToList();

            foreach (var aggregate in aggregates)
            {
                var aggregateName = aggregate.Name;

                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("@startuml");
                stringBuilder.AppendLine($"namespace {aggregateName} <<aggregate>> {{");

                var rootBuilder = this.RenderClass(aggregate);
                stringBuilder.Append(rootBuilder);

                stringBuilder.AppendLine("}");
                stringBuilder.AppendLine("@enduml");

                var fileName = $"aggregate.{aggregateName.ToLowerInvariant()}.puml";
                files.Add(aggregateName, fileName);

                File.WriteAllText(fileName, stringBuilder.ToString());
            }

            return files;
        }

        private StringBuilder RenderClass(TypeDescription type)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (type.IsAbstract) stringBuilder.Append("abstract ");
            if (type.IsEnumeration())
            {
                stringBuilder.Append("enum");
            }
            else
            {
                stringBuilder.Append("class");
            }
            stringBuilder.Append($" {type.Name} ");
            type.RenderStereoType(stringBuilder);
            stringBuilder.AppendLine("{");

            if (type.IsEnumeration())
            {
                foreach (var field in type.Fields)
                {
                    stringBuilder.AppendLine(field.Name);
                }
            }
            else
            {
                foreach (var property in type.Properties.Where(p => !p.IsPrivate))
                {
                    property.RenderProperty(stringBuilder);
                }

                foreach (var method in type.Methods.Where(m => !m.IsPrivate))
                {
                    method.RenderMethod(stringBuilder);
                }
            }

            stringBuilder.AppendLine("}");

            foreach (var propertyDescription in type.Properties)
            {
                var property = this.types.FirstOrDefault(t => string.Equals(t.FullName, propertyDescription.Type) || (propertyDescription.Type.IsEnumerable() && string.Equals(t.FullName, propertyDescription.Type.GenericTypes().First())));
                if (property != null)
                {
                    var classBuilder = this.RenderClass(property);
                    stringBuilder.Append(classBuilder);

                    // Relation
                    stringBuilder.Append($"{type.Name} -- {property.Name}");
                    if (propertyDescription.Type.IsEnumerable()) stringBuilder.Append(" : 1..*");
                    stringBuilder.AppendLine();
                }
            }

            return stringBuilder;
        }
    }
}