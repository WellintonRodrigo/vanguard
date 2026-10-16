namespace Vanguard.Domain.Entities
{
    public class WeatherLog
    {
        
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Location { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public decimal PrecipitationMm { get; set; }

        public decimal TemperatureMin { get; set; }

        public decimal TemperatureMax { get; set; }

        public bool ExtremeEvent { get; set; }
    }
}
