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

         public Artifact_CategoryRepository()
         {
             var db = new Database(BoeingSalesApp.Utility.TempSettings.DbPath);
             _database = db.GetAsyncConnection(); 
         }

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

        public async Task<bool> DoesExist(Artifact artifact, Category category)
        {
            var relationships = await _database.Table<Artifact_Category>().Where(x => (x.ArtifactID == artifact.ID) && (x.CategoryID == category.ID)).CountAsync();
            return relationships > 0;
        }

        public async Task AddRelationship(Artifact artifact, Category category)
        {
            // need to check if relationship already exists...
            if (!await DoesExist(artifact, category)) {
                var artifactRepo = new ArtifactRepository();
                var categoryRepo = new CategoryRepository();
                if (!await artifactRepo.DoesExist(artifact.ID))
                {
                    await artifactRepo.SaveAsync(artifact);
                }

                if (!await categoryRepo.DoesExist(category.ID))
                {
                    await categoryRepo.SaveAsync(category);
                }

                var newRelationship = new Artifact_Category
                {
                    ArtifactID = artifact.ID,
                    CategoryID = category.ID
                };

                await SaveAsync(newRelationship);
            }
            
        }

        /// <summary>
        /// Remove a specified artifact from a category
        /// </summary>
        /// <param name="artifact"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task RemoveArtifactFromCategory(Artifact artifact, Category category)
        {
            var relationships = await _database.Table<Artifact_Category>().Where(x => (x.ArtifactID == artifact.ID) && (x.CategoryID == category.ID)).ToListAsync();

            foreach(var relationship in relationships)
            {
                await _database.DeleteAsync(relationship);
            }
        }


        /// <summary>
        /// Get the Artifacts for a specific category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task<List<Artifact>> GetAllArtifactsForCategory(Category category)
        {
            var relationships = await _database.Table<Artifact_Category>().Where(x => x.CategoryID == category.ID).ToListAsync();
            var artifacts = new List<Artifact>();
            foreach (var relationship in relationships)
            {
                artifacts.Add(await _database.FindAsync<Artifact>(relationship.ArtifactID));
            }

            return artifacts;
        }


    }
}
