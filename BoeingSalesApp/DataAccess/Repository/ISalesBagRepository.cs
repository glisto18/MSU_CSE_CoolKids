using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoeingSalesApp.DataAccess.Entities;

namespace BoeingSalesApp.DataAccess.Repository
{
    interface ISalesBagRepository
    {
        Task SaveAsync(SalesBag bag);

        Task DeleteAsync(SalesBag bag);

        Task<List<SalesBag>> GetAllAsync();

        Task<bool> DoesExist(Guid bagId);

        Task<SalesBag> Get(Guid bagId);
    }
}
