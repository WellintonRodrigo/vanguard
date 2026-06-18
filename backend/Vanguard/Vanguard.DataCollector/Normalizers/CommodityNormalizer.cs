namespace Vanguard.DataCollector.Normalizers
{
    public static class CommodityNormalizer
    {
        public static string Normalizers(string title)
        {
            if (title.Contains("soja", StringComparison.OrdinalIgnoreCase))
                return "Soja";

            if (title.Contains("milho", StringComparison.OrdinalIgnoreCase))
                return "Milho";

            if (title.Contains("café", StringComparison.OrdinalIgnoreCase))
                return "Café";

            if (title.Contains("algodão", StringComparison.OrdinalIgnoreCase))
                return "Algodão";

            return title;
        }
    }
}
