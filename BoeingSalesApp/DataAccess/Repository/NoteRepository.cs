using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using BoeingSalesApp.DataAccess.Entities;

namespace BoeingSalesApp.DataAccess.Repository
{
    class NoteRepository : INoteRepository
    {
        private readonly SQLiteAsyncConnection _database;

        public NoteRepository(IDatabase database)
        {
            _database = database.GetAsyncConnection();
        }

        public async Task SaveAsync(Note note)
        {
            await _database.InsertAsync(note);
        }

        public async Task DeleteAsync(Note note)
        {
            await _database.DeleteAsync(note);
        }

        public async Task<List<Note>> GetAllAsync()
        {
            var note = await _database.Table<Note>().ToListAsync();
            return note;
        }
    }
}
