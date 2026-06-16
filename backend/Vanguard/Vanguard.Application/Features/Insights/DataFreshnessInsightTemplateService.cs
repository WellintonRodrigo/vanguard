using System.Text.Json;
using Vanguard.Domain.Enums;

namespace Vanguard.Application.Features.Insights
{
    public class DataFreshnessInsightTemplateService
    {
        private readonly Dictionary<string, List<string>> _templates;

        public DataFreshnessInsightTemplateService() 
        {
            var filePath = Path.Combine(
            AppContext.BaseDirectory,

            "Resources",
            "Insights",
            "data-freshness-insights.json");

            Console.WriteLine(filePath);

            if (!File.Exists(filePath))
                throw new FileNotFoundException(
                 $"O arquivo de templates de freshness não foi encontrado: {filePath}");

            var json = File.ReadAllText(filePath);

            _templates = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json)
                ?? new Dictionary<string, List<string>>();

        }

        public string Generate(DataFreshnessStatus status, int daysWithoutUpdate)
        {
            var Key = status.ToString().ToLower();

            if(!_templates.TryGetValue(Key, out var phrases) || phrases.Count == 0)
                return "Insight de atualidade indisponível.";

            var selectedPhrase = phrases[Random.Shared.Next(phrases.Count)];

            return selectedPhrase
                .Replace("{days}", daysWithoutUpdate.ToString());
        }
    }
}
