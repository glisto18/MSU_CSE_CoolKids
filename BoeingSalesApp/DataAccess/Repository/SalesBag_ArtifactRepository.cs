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
        public async Task AddArtifactToSalesbag(Artifact artifact, SalesBag salesbag)
        {
            if (!await DoesExist(artifact, salesbag))
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

        /// <summary>
        /// Get all the Artifacts for a specified salesbag
        /// </summary>
        /// <param name="salesbag"></param>
        /// <returns></returns>
        public async Task<List<Artifact>> GetAllSalesBagArtifacts(SalesBag salesbag)
        {
            var relationships = await _database.Table<SalesBag_Artifact>().Where(x => x.SalesBag == salesbag.ID).ToListAsync();
            var artifacts = new List<Artifact>();
            foreach (var relationship in relationships)
            {
                artifacts.Add(await _database.FindAsync<Artifact>(relationship.Artifact));
            }

            return artifacts;
        }

        /// <summary>
        /// Remove an artifact from a salesbag
        /// </summary>
        /// <param name="salesbag"></param>
        /// <param name="salesbag"></param>
        /// <returns></returns>
        public async Task RemoveArtifactFromSalesBag(Artifact artifact, SalesBag salesbag)
        {
            var relationships = await _database.Table<SalesBag_Artifact>().Where(x => (x.Artifact == artifact.ID) && (x.SalesBag == salesbag.ID)).ToListAsync();

            foreach (var relationship in relationships)
            {
                await _database.DeleteAsync(relationship);
            }
        }

        /// <summary>
        /// Checks if the artifact is already contained withinthe salesbag.
        /// </summary>
        /// <param name="artifact"></param>
        /// <param name="salesbag"></param>
        /// <returns></returns>
        public async Task<bool> DoesExist(Artifact artifact, SalesBag salesbag)
        {
            var relationships = await _database.Table<SalesBag_Artifact>().Where(x => (x.Artifact == artifact.ID) && (x.SalesBag == salesbag.ID)).CountAsync();
            return relationships > 0;
        }

        public async Task DeleteAllArtifactReferences(Artifact artifact)
        {
            var relationships = await _database.Table<SalesBag_Artifact>().Where(x => (x.Artifact == artifact.ID)).ToListAsync();
            foreach (var relationship in relationships)
            {
                await _database.DeleteAsync(relationship);
            }
        }
    }
}
