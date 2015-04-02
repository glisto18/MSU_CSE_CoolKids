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

        /// <summary>
        /// Initializes DB with default DB
        /// </summary>
        public SalesBagRepository()
        {
            var db = new Database(Utility.TempSettings.DbPath);
            _database = db.GetAsyncConnection();
        }

        /// <summary>
        /// Initializes DB with specified DB
        /// </summary>
        /// <param name="database"></param>
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

        /// <summary>
        /// Gets the salesbag specified by the bagId
        /// </summary>
        /// <param name="bagId"></param>
        /// <returns></returns>
        public async Task<SalesBag> Get(Guid bagId)
        {
            return await _database.GetAsync<SalesBag>(bagId);
        }

        /// <summary>
        /// Returns true an entitie is found with the specified Id
        /// </summary>
        /// <param name="salesbagId"></param>
        /// <returns></returns>
        public async Task<bool> DoesExist(Guid bagId)
        {
            var salesbagIdCount = await _database.Table<SalesBag>().Where(x => x.ID == bagId).CountAsync();
            return salesbagIdCount > 0;
        }

        public async Task<bool> UpdateTitle(Guid salesbagId, string newTitle)
        {
            var salesbag = await Get(salesbagId);
            if (salesbag != null)
            {
                salesbag.Name = newTitle;
                await _database.UpdateAsync(salesbag);
                return true;
            }
            return false;

        }
    }
}
