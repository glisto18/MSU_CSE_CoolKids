using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using BoeingSalesApp.DataAccess.Entities;

namespace BoeingSalesApp.DataAccess
{
    class Database : IDatabase
    {
        private readonly SQLiteAsyncConnection _dbConnection;

        public Database(string databasePath)
        {
            _dbConnection = new SQLiteAsyncConnection(databasePath);
        }

        public async Task Initialize()
        {
            await _dbConnection.CreateTableAsync<TestClass>();
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            return _dbConnection;
        }
    }
}
