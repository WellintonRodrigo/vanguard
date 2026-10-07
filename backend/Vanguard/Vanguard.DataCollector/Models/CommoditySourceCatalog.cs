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
                },

                new()
                {
                    SourceKey = "NoticiasAgricolas",
                    Commodity = "Arroz",
                    Url = "https://www.noticiasagricolas.com.br/cotacoes/arroz",
                    Unit = "Saca 50kg",
                    IsEnabled = true
                 },
             new()
               {
                 SourceKey = "NoticiasAgricolas",
                 Commodity = "Feijao",
                 Url = "https://www.noticiasagricolas.com.br/cotacoes/feijao",
                Unit = "Saca 60kg",
                IsEnabled = false
                },
             new()
             
             {
                 SourceKey = "NoticiasAgricolas",
                 Commodity = "Boi-Gordo",
                 Url = "https://www.noticiasagricolas.com.br/cotacoes/boi-gordo",
                Unit = "Saca 60kg",
                IsEnabled = true
             }

            };
    }
}
