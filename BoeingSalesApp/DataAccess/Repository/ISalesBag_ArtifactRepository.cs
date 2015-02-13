using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using BoeingSalesApp.DataAccess.Entities;

namespace BoeingSalesApp.DataAccess.Repository
{
    interface ISalesBag_ArtifactRepository
    {
        Task SaveAsync(SalesBag_Artifact salesbagArtifact);

        Task DeleteAsync(SalesBag_Artifact salesbagArtifact);

        Task<List<SalesBag_Artifact>> GetAllAsync();

        Task AddArtifactToSalesbag(Artifact artifact, SalesBag salesbag);

        Task<List<Artifact>> GetAllSalesBagArtifacts(SalesBag salesbag);

        Task RemoveArtifactFromSalesBag(Artifact artifact, SalesBag salesbag);

        Task<bool> DoesExist(Artifact artifact, SalesBag salesbag);
    }
}
