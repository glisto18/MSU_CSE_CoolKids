using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoeingSalesApp.DataAccess.Entities;

namespace BoeingSalesApp.DataAccess.Repository
{
    class ArtifactRepository : IArtifactRepository
    {

        private readonly SQLiteAsyncConnection _database;

        public ArtifactRepository(IDatabase database)
        {
            _database = database.GetAsyncConnection();
        }

        public async Task SaveAsync(Artifact artifact)
        {
            await _database.InsertAsync(artifact);
        }

        public async Task DeleteAsync(Artifact artifact)
        {
            await _database.DeleteAsync(artifact);
        }

        public async Task<List<Artifact>> GetAllAsync()
        {
            var artifacts = await _database.Table<Artifact>().ToListAsync();
            return artifacts;
        }

    }
}
