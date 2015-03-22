using BoeingSalesApp.DataAccess.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoeingSalesApp.Utility;

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

        public async Task<List<Utility.DisplayCategory>> GetAllDisPlayCategoriesAsync()
        {
            var categories = await GetAllAsync();
            //var displayCategories = categories.Select(x => new Utility.DisplayCategory(x)).ToList();
            var displayCategories = new List<DisplayCategory>();
            foreach (var category in categories)
            {
                var displayCategory = new DisplayCategory(category);
                displayCategory.SetNumOfChildren();
                displayCategories.Add(displayCategory);
            }
            return displayCategories;
        }

        public async Task<Category> Get(Guid categoryId)
        {
            return await _database.GetAsync<Category>(categoryId);
        }

        public async Task<Category> Get(string name)
        {
            return await _database.Table<Category>().Where(x => x.Name == name).FirstAsync();
        }

       public async Task<bool> DoesExist(Guid categoryId)
        {
            var countById = await _database.Table<Category>().Where(x => x.ID == categoryId).CountAsync();
            return countById > 0;
        }

       public async Task<List<Category>> GetCategoriesByName(string title)
       {
           return await _database.Table<Category>().Where(x => x.Name == title).ToListAsync();
       }

       public async Task<bool> DoesExist(string name)
       {
           var count = await _database.Table<Category>().Where(x => x.Name == name).CountAsync();
           return count > 0;
       }

    }
}
