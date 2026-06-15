using Vanguard.Domain.Entities;

namespace Vanguard.Domain.Interfaces
{
    public interface ICommodityRepository
    {
        Task CreateAsync(
       CommodityPrice commodityPrice);

        Task<List<CommodityPrice>> GetByProductAsync(
            string product);

        Task<List<CommodityPrice>> GetByRegionAsync(
            string region);
    }
}
