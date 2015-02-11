using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using BoeingSalesApp.DataAccess.Entities;

namespace BoeingSalesApp.DataAccess.Repository
{
    class SalesBag_ArtifactRepository : ISalesBag_ArtifactRepository
    {
        private readonly SQLiteAsyncConnection _database;

        public SalesBag_ArtifactRepository(IDatabase database)
        {
            _database = database.GetAsyncConnection();
        }

        public async Task SaveAsync(SalesBag_Artifact salesbagArtifact)
        {
            await _database.InsertAsync(salesbagArtifact);
        }

        public async Task DeleteAsync(SalesBag_Artifact salesbagArtifact)
        {
            await _database.DeleteAsync(salesbagArtifact);
        }

        public async Task<List<SalesBag_Artifact>> GetAllAsync()
        {
            var salesbagArtifacts = await _database.Table<SalesBag_Artifact>().ToListAsync();
            return salesbagArtifacts;
        }
    }
}
