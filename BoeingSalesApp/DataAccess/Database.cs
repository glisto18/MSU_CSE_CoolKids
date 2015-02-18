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

            await _dbConnection.CreateTableAsync<Category>();
            await _dbConnection.CreateTableAsync<Artifact>();
            await _dbConnection.CreateTableAsync<Artifact_Category>();
            await _dbConnection.CreateTableAsync<Meeting>();
            await _dbConnection.CreateTableAsync<Note>();
            await _dbConnection.CreateTableAsync<SalesBag>();
            await _dbConnection.CreateTableAsync<SalesBag_Artifact>();
            await _dbConnection.CreateTableAsync<FolderToken>();
            
        }

        public SQLiteAsyncConnection GetAsyncConnection()
        {
            return _dbConnection;
        }
    }
}
