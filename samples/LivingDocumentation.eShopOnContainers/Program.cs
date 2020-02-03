using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LivingDocumentation.eShopOnContainers
{
    public class Program
    {
        public static List<TypeDescription> Types;

        public static async Task Main(string[] args)
        {
            // Read analysis 
            var result = await File.ReadAllTextAsync("analysis.json");

            Types = JsonConvert.DeserializeObject<List<TypeDescription>>(result, JsonDefaults.DeserializerSettings());
            
            var aggregateFiles = new AggregateRenderer().Render();
            var commandHandlerFiles = new CommandHandlerRenderer().Render();
            var eventHandlerFiles = new EventHandlerRenderer().Render();

            new AsciiDocRenderer(Types, aggregateFiles, commandHandlerFiles, eventHandlerFiles).Render();
        }
    }
}
