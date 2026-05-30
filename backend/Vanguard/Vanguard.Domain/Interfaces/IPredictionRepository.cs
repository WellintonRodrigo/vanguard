using System;
using System.Collections.Generic;
using System.Text;
using Vanguard.Domain.Entities;

namespace Vanguard.Domain.Interfaces
{
    public interface IPredictionRepository
    {
        Task CreateAsync(Prediction prediction);
        Task<Prediction?> GetByIdAsync(string id);
        Task<List<Prediction>> GetAllAsync();
        Task UpdateAsync(Prediction prediction);
        Task DeleteAsync(string id);
    }
}
