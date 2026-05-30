using System;
using System.Collections.Generic;
using System.Text;
using Vanguard.Domain.Entities;
using Vanguard.Domain.Interfaces;

namespace Vanguard.Infrastructure.Persistence.Repositories
{
    public class PredictionRepository : IPredictionRepository
    {
        Task IPredictionRepository.CreateAsync(Prediction prediction)
        {
            throw new NotImplementedException();
        }

        Task IPredictionRepository.DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        Task<List<Prediction>> IPredictionRepository.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<Prediction?> IPredictionRepository.GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        Task IPredictionRepository.UpdateAsync(Prediction prediction)
        {
            throw new NotImplementedException();
        }
    }
}
