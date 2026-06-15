namespace Vanguard.DataCollector.Models
{
    public class CommoditySourceCatalog
    {
        public static IReadOnlyCollection<CommoditySource> Sources =>
            new List<CommoditySource>
            {
                new()
                {
                    SourceKey ="NoticiasAgricolas",
                    Commodity = "Soja",
                    Url ="https://www.noticiasagricolas.com.br/cotacoes/soja",
                    Unit = "Saca 60kg",
                    IsEnabled = true,
                },

                new()
                {
                SourceKey = "NoticiasAgricolas",
                Commodity = "Milho",
                Url = "https://www.noticiasagricolas.com.br/cotacoes/milho",
                Unit = "Saca 60kg",
                IsEnabled = true
                }
            };
    }
}
