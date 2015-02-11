using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using BoeingSalesApp.DataAccess.Entities;

namespace BoeingSalesApp.DataAccess.Repository
{
    class SalesBagRepository : ISalesBagRepository
    {
        private readonly SQLiteAsyncConnection _database;

        public SalesBagRepository(IDatabase database)
        {
            _database = database.GetAsyncConnection();
        }

        public async Task SaveAsync(SalesBag bag)
        {
            await _database.InsertAsync(bag);
        }

        public async Task DeleteAsync(SalesBag bag)
        {
            await _database.DeleteAsync(bag);
        }

        public async Task<List<SalesBag>> GetAllAsync()
        {
            var bags = await _database.Table<SalesBag>().ToListAsync();
            return bags;
        }
    }
}
