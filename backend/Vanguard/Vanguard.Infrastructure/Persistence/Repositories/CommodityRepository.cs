using System;
using System.Collections.Generic;
using System.Text;
using Vanguard.Domain.Entities;
using Vanguard.Domain.Interfaces;

namespace Vanguard.Infrastructure.Persistence.Repositories
{
    public class CommodityRepository : ICommodityRepository
    {
        Task ICommodityRepository.CreateAsync(CommodityPrice commodityPrice)
        {
            throw new NotImplementedException();
        }

        Task<List<CommodityPrice>> ICommodityRepository.GetByProductAsync(string product)
        {
            throw new NotImplementedException();
        }

        Task<List<CommodityPrice>> ICommodityRepository.GetByRegionAsync(string region)
        {
            throw new NotImplementedException();
        }
    }
}
