using Vanguard.DataCollector.Helpers;

namespace Vanguard.DataCollector.Normalizers
{
    public static class MarketNormalizer
    {
        public static string Normalize(string title, string commodity)
        {
            var market = title;

            market = market.Replace("Indicador da", "", StringComparison.OrdinalIgnoreCase);
            market = market.Replace("Indicador do", "", StringComparison.OrdinalIgnoreCase);
            market = market.Replace("Indicador de", "", StringComparison.OrdinalIgnoreCase);

            market = market.Replace(commodity, "", StringComparison.OrdinalIgnoreCase);

            market = TextHelper.Clean(market);

            return market.Trim();
        }
    }
}
