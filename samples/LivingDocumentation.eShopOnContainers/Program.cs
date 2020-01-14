using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LivingDocumentation.eShopOnContainers
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Read analysis 
            var result = await File.ReadAllTextAsync("analysis.json");

            var types = JsonConvert.DeserializeObject<List<TypeDescription>>(result, JsonDefaults.DeserializerSettings());
            
            var aggregateFiles = new AggregateRenderer(types).Render();
            var commandHandlerFiles = new CommandHandlerRenderer(types).Render();
            var eventHandlerFiles = new EventHandlerRenderer(types).Render();

            new AsciiDocRenderer(types, aggregateFiles, commandHandlerFiles, eventHandlerFiles).Render();
        }
    }
}
