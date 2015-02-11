using BoeingSalesApp.DataAccess.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoeingSalesApp.DataAccess.Repository
{
    class CategoryRepository: ICategoryRepository
    {
        private readonly SQLiteAsyncConnection _database;

        public CategoryRepository() 
        {   
            var db = new Database(BoeingSalesApp.Utility.TempSettings.DbPath);
             _database = db.GetAsyncConnection(); 
        }

        public CategoryRepository(IDatabase database)
        {
            _database = database.GetAsyncConnection();
        }

        public async Task SaveAsync(Category category)
        {
            await _database.InsertAsync(category);
        }

        public async Task DeleteAsync(Category category)
        {
            await _database.DeleteAsync(category);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            var categories = await _database.Table<Category>().ToListAsync();
            return categories;
        }

        public async Task<Category> Get(Guid categoryId)
        {
            return await _database.GetAsync<Category>(categoryId);
        }

       public async Task<bool> DoesExist(Guid categoryId)
        {
            var countById = await _database.Table<Category>().Where(x => x.ID == categoryId).CountAsync();
            return countById > 0;
        }
    }
}
