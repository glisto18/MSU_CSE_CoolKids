using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using BoeingSalesApp.DataAccess.Entities;

namespace BoeingSalesApp.DataAccess.Repository
{
    class MeetingRepository : IMeetingRepository
    {
        private readonly SQLiteAsyncConnection _database;

        public MeetingRepository(IDatabase database)
        {
            _database = database.GetAsyncConnection();
        }

        public async Task SaveAsync(Meeting meeting)
        {
            await _database.InsertAsync(meeting);
        }

        public async Task DeleteAsync(Meeting meetings)
        {
            await _database.DeleteAsync(meetings);
        }

        public async Task<List<Meeting>> GetAllAsync()
        {
            var meeting = await _database.Table<Meeting>().ToListAsync();
            return meeting;
        }
    }
}
