using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoeingSalesApp.DataAccess.Entities;

namespace BoeingSalesApp.DataAccess.Repository
{
    interface IFolderTokenRepository
    {
        Task<FolderToken> Get();

        Task Put(string tokenId);

    }
}
