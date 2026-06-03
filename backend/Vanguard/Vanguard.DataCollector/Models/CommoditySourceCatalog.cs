namespace Vanguard.DataCollector.Models
{
    public class CommoditySourceCatalog
    {
        public static IReadOnlyCollection<CommoditySource> sources =>
            new List<CommoditySource>
            {
                new()
                {
                    Name ="NoticiasAgriculas",
                    Commodity = "Soja",
                    Url ="https://www.noticiasagricolas.com.br/cotacoes/soja/soja-indicador-cepea-esalq-porto-paranagua",
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
