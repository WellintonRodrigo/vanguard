using System.Text.Json;

namespace Vanguard.Application.Features.Insights
{
    public class InsightTemplateService
    {
        private readonly Dictionary<string, List<string>> _templates;

        public InsightTemplateService()
        {
            var filePath = Path.Combine(
            AppContext.BaseDirectory,

            "Resources",
            "insights",
            "agriculture-insights.json");

            Console.WriteLine( filePath );
             
            if(!File.Exists(filePath))
            
                throw new FileNotFoundException(
                 $"O arquivo de templates de insights não foi encontrado: {filePath}");
            
            var json = File.ReadAllText(filePath);

            _templates = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json)
                ?? new Dictionary<string, List<string>>();
        }

        public string Generate(string key, string product, string location)
        {
            if (!_templates.TryGetValue(key, out var phrases) || phrases.Count == 0)
                return $"Insight indisponível para {product} em {location}.";
            
            var selectedPhrase = phrases[new Random().Next(phrases.Count)];

            return selectedPhrase
                .Replace("{product}", product)
                .Replace("{location}", location);

        }
    }
}
