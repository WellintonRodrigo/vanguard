using System;
using System.Collections.Generic;
using System.Text;
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
