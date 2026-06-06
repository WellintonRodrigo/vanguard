namespace Vanguard.DataCollector.Models
{
    public class CommoditySourceCatalog
    {
        public static IReadOnlyCollection<CommoditySource> sources =>
            new List<CommoditySource>
            {
                new()
                {
                    Name ="NoticiasAgricolas",
                    Commodity = "Soja",
                    Url ="https://www.noticiasagricolas.com.br/cotacoes/soja",
                    Unit = "Saca 60kg",
                    Enabled = true,
                },

                new()
                {
                Name = "NoticiasAgricolas",
                Commodity = "Milho",
                Url = "https://www.noticiasagricolas.com.br/cotacoes/milho",
                Unit = "Saca 60kg",
                Enabled = true
                }
            };
    }
}
