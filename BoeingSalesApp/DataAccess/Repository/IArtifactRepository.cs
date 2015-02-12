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
        Task SaveAsync(Artifact artifact);

        Task DeleteAsync(Artifact artifact);

        Task<List<Artifact>> GetAllAsync();

        Task<Artifact> Get(Guid artifactId);

        Task<bool> DoesExist(Guid artifactId);

        Task<List<Artifact>> GetArtifactsByTitle(string title);
    }
}
