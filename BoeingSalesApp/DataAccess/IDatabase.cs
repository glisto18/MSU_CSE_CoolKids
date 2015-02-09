using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BoeingSalesApp.DataAccess
{
    public interface IDatabase
    {
        Task Initialize();
        SQLiteAsyncConnection GetAsyncConnection();
    }
}
