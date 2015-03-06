using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoeingSalesApp.DataAccess.Entities;

namespace BoeingSalesApp.DataAccess.Repository
{
    interface IArtifactRepository
    {
        Task<int> SaveAsync(Artifact artifact);

        Task DeleteAsync(Artifact artifact);

        Task<List<Artifact>> GetAllAsync();

        Task<Artifact> Get(int artifactId);

        Task<Artifact> Get(Guid artifactId);

        Task<bool> DoesExist(Guid artifactId);

        Task<bool> DoesExist(string fileName);

        Task<List<Artifact>> GetArtifactsByTitle(string title);

        Task<Artifact> GetArtifactByFileName(string fileName);
    }
}
