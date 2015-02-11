using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoeingSalesApp.DataAccess.Entities;

namespace BoeingSalesApp.DataAccess.Repository
{
    interface IArtifact_CategoryRepository
    {
        Task SaveAsync(Artifact_Category artifact_category);

        Task DeleteAsync(Artifact_Category artifact_category);

        Task<List<Artifact_Category>> GetAllAsync();

        
    }
}
