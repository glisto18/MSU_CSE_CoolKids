using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoeingSalesApp.DataAccess.Entities;
using SQLite;

namespace BoeingSalesApp.DataAccess.Repository
{
    class Artifact_CategoryRepository : IArtifact_CategoryRepository
    {
         private readonly SQLiteAsyncConnection _database;

         public Artifact_CategoryRepository(IDatabase database)
        {
            _database = database.GetAsyncConnection();
        }

        public async Task SaveAsync(Artifact_Category artifact_category)
        {
            await _database.InsertAsync(artifact_category);
        }

        public async Task DeleteAsync(Artifact_Category artifact_category)
        {
            await _database.DeleteAsync(artifact_category);
        }

        public async Task<List<Artifact_Category>> GetAllAsync()
        {
            var artifact_categories = await _database.Table<Artifact_Category>().ToListAsync();
            return artifact_categories;
        }

    }
}
