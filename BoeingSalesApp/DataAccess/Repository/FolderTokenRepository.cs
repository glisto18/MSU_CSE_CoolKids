using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using BoeingSalesApp.DataAccess.Entities;

namespace BoeingSalesApp.DataAccess.Repository
{
    class FolderTokenRepository : IFolderTokenRepository
    {
        private readonly SQLiteAsyncConnection _database;

        public FolderTokenRepository()
        {
            var db = new Database(BoeingSalesApp.Utility.TempSettings.DbPath);
            _database = db.GetAsyncConnection();
        }

        public async Task<FolderToken> Get()
        {

            var tokenList = await _database.Table<FolderToken>().ToListAsync();
            if (tokenList.Count < 1)
            {
                return null;
            }
            return tokenList[0];
        }

        public async Task Put(string tokenId)
        {
            var token = new FolderToken
            {
                DateCreated = DateTime.Now,
                Token = tokenId
            };

            await _database.InsertAsync(token);
        }

    }
}
