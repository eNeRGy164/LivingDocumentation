using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LivingDocumentation.Uml;
using PlantUml.Builder;
using PlantUml.Builder.ClassDiagrams;

namespace LivingDocumentation.eShopOnContainers
{
    public class AggregateRenderer
    {
        public IReadOnlyDictionary<string, string> Render()
        {
            var files = new Dictionary<string, string>();

            var aggregates = Program.Types.Where(t => t.IsAggregateRoot()).ToList();

            foreach (var aggregate in aggregates)
            {
                var aggregateName = aggregate.Name;

                var stringBuilder = new StringBuilder();
                stringBuilder.UmlDiagramStart();
                stringBuilder.SkinParameter(SkinParameter.MinClassWidth, "160");
                stringBuilder.SkinParameter(SkinParameter.Linetype, "ortho");
                stringBuilder.NamespaceStart(aggregateName, stereotype: "aggregate");

                var rootBuilder = this.RenderClass(aggregate);
                stringBuilder.Append(rootBuilder);

                stringBuilder.NamespaceEnd();
                stringBuilder.UmlDiagramEnd();

                var fileName = $"aggregate.{aggregateName.ToLowerInvariant()}.puml";
                files.Add(aggregate.FullName, fileName);

                File.WriteAllText(fileName, stringBuilder.ToString());
            }

            return files;
        }

        private StringBuilder RenderClass(TypeDescription type)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (type.IsEnumeration())
            {
                stringBuilder.EnumStart(type.Name, stereotype: "enumeration");

                foreach (var field in type.Fields)
                {
                    stringBuilder.InlineClassMember(new ClassMember(field.Name));
                }

                stringBuilder.EnumEnd();
            }
            else
            {
                if (type.IsAggregateRoot())
                {
                    stringBuilder.ClassStart(type.Name, isAbstract: type.IsAbstract(), stereotype: "root", customSpot: new CustomSpot('R', "LightBlue"));
                }

                if (type.IsValueObject())
                {
                    stringBuilder.ClassStart(type.Name, isAbstract: type.IsAbstract(), stereotype: "value object", customSpot: new CustomSpot('O', "Wheat"));
                }

                if (type.IsEntity())
                {
                    stringBuilder.ClassStart(type.Name, isAbstract: type.IsAbstract(), stereotype: "entity");
                }

                foreach (var property in type.Properties.Where(p => !p.IsPrivate()))
                {
                    stringBuilder.InlineClassMember(new ClassMember(property.Name, isAbstract: property.IsAbstract(), isStatic: property.IsStatic(), visibility: property.ToUmlVisibility()));
                }

                foreach (var method in type.Methods.Where(m => !m.IsPrivate()))
                {
                    var fullMethod = $"{method.Name}({string.Join(", ", method.Parameters.Select(s => s.Name))})";
                    stringBuilder.InlineClassMember(new ClassMember(fullMethod, isAbstract: method.IsAbstract(), isStatic: method.IsStatic(), visibility: method.ToUmlVisibility()));
                }

                stringBuilder.ClassEnd();
            }

            foreach (var propertyDescription in type.Properties)
            {
                var property = Program.Types.FirstOrDefault(t => string.Equals(t.FullName, propertyDescription.Type) || (propertyDescription.Type.IsEnumerable() && string.Equals(t.FullName, propertyDescription.Type.GenericTypes().First())));
                if (property != null)
                {
                    var classBuilder = this.RenderClass(property);
                    stringBuilder.Append(classBuilder);

                    // Relation
                    if (propertyDescription.Type.IsEnumerable())
                    {
                        stringBuilder.Relationship(type.Name, "--", property.Name, label: "1..*");
                    }
                    else
                    {
                        stringBuilder.Relationship(type.Name, "--", property.Name);
                    }
                }
            }

            return stringBuilder;
        }
    }
}
