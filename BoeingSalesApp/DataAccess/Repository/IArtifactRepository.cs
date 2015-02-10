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
    }
}
