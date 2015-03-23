using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoeingSalesApp.DataAccess.Entities;

namespace BoeingSalesApp.DataAccess.Repository
{
    class ArtifactRepository : IArtifactRepository
    {

        private readonly SQLiteAsyncConnection _database;

        public ArtifactRepository()
        {
            var db = new Database(BoeingSalesApp.Utility.TempSettings.DbPath);
            _database = db.GetAsyncConnection();
        }

        public ArtifactRepository(IDatabase database)
        {
            _database = database.GetAsyncConnection();
        }

        public async Task<int> SaveAsync(Artifact artifact)
        {
            return await _database.InsertAsync(artifact);
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

        public async Task<Artifact> Get(int artifactId)
        {
            return await _database.GetAsync<Artifact>(artifactId);
        }

        public async Task<Artifact> Get(Guid artifactId)
        {
            return await _database.GetAsync<Artifact>(artifactId);
        }

        public async Task<bool> DoesExist(Guid artifactId)
        {
            var artifactIdCount = await _database.Table<Artifact>().Where(x => x.ID == artifactId).CountAsync();
            return artifactIdCount > 0;
        }

        public async Task<List<Artifact>> GetArtifactsByTitle(string title)
        {
            return await _database.Table<Artifact>().Where(x => x.Title  == title).ToListAsync();
        }

        public async Task<bool> DoesExist(string fileName)
        {
            var artifactCount = await _database.Table<Artifact>().Where(x => x.FileName == fileName).CountAsync();
            return artifactCount > 0;
        }

        public async Task<Artifact> GetArtifactByFileName(string fileName)
        {
            return await _database.Table<Artifact>().Where(x => x.FileName == fileName).FirstAsync();
        }

        public async Task<List<Utility.DisplayArtifact>> GetAllDisplayArtifactsAsync()
        {
            var artifacts = await GetAllAsync();
            var displayArtifacts = artifacts.Select(x => new Utility.DisplayArtifact(x)).ToList();
            return displayArtifacts;
        }

        public async Task<List<Utility.DisplayArtifact>> GetAllUncategorizedArtifacts()
        {
            var allArtifacts = await GetAllAsync();
            var artifactCategoryRepo = new Artifact_CategoryRepository();
            var uncategorizedArtifacts = new List<Utility.DisplayArtifact>();
            foreach (var artifact in allArtifacts)
            {
                if (!await artifactCategoryRepo.DoesArtifactHaveCategory(artifact))
                {
                    uncategorizedArtifacts.Add(new Utility.DisplayArtifact(artifact));
                }
            }

            return uncategorizedArtifacts;
        }

        /// <summary>
        /// Returns true on update, false on no update
        /// </summary>
        /// <param name="artifactId"></param>
        /// <param name="newTitle"></param>
        /// <returns></returns>
        public async Task<bool> UpdateTitle(Guid artifactId, string newTitle)
        {
            var artifact = await Get(artifactId);
            if (artifact != null)
            {
                artifact.Title = newTitle;
                await _database.UpdateAsync(artifact);
                return true;
            }
            return false;

        }
    }
}
