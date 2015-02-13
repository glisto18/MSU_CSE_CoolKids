using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoeingSalesApp.DataAccess.Entities;

namespace BoeingSalesApp.DataAccess.Repository
{
    interface IMeetingRepository
    {
        Task SaveAsync(Meeting meeting);

        Task DeleteAsync(Meeting meeting);

        Task<List<Meeting>> GetAllAsync();
    }
}
