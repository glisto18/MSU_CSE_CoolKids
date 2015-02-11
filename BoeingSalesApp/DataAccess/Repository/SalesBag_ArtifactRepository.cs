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

        /// <summary>
        /// Initializes DB with default DB
        /// </summary>
        public SalesBag_ArtifactRepository()
        {
            var db = new Database(BoeingSalesApp.Utility.TempSettings.DbPath);
            _database = db.GetAsyncConnection();
        }

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

        /// <summary>
        /// Maps the relationship between an artifact and a salesbag
        /// </summary>
        /// <param name="artifact"></param>
        /// <param name="salesbag"></param>
        /// <returns></returns>
        public async Task AddRelationship(Artifact artifact, SalesBag salesbag)
        {
            var artifactRepo = new ArtifactRepository();
            var salesbagRepo = new SalesBagRepository();

            if (!await artifactRepo.DoesExist(artifact.ID))
            {
                await artifactRepo.SaveAsync(artifact);
            }
            if (!await salesbagRepo.DoesExist(salesbag.ID))
            {
                await salesbagRepo.SaveAsync(salesbag);
            }

            var newRelationship = new SalesBag_Artifact
            {
                Artifact = artifact.ID,
                SalesBag = salesbag.ID
            };

            await SaveAsync(newRelationship);
           
        }
    }
}
