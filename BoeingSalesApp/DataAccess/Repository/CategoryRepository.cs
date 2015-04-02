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
                await displayCategory.SetNumOfChildren();
                displayCategories.Add(displayCategory);
            }
            return displayCategories;
        }

        public async Task<Category> Get(Guid categoryId)
        {
            Category category;
            try
            {
                category = await _database.GetAsync<Category>(categoryId);
            }
            catch(InvalidOperationException e)
            {
                return null;
            }
            return category;

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

       public async Task<bool> UpdateTitle(Guid categoryId, string newTitle)
       {
           var category = await Get(categoryId);
           if (category != null)
           {
               category.Name = newTitle;
               await _database.UpdateAsync(category);
               return true;
           }
           return false;

       }

       public async Task<List<Category>> Search(string searchTerm)
       {
           var query = string.Format(@"SELECT * 
                    FROM Category
                    WHERE LOWER(Name) LIKE '%{0}%' ", searchTerm.ToLower());
           var results = await _database.QueryAsync<Category>(query);

           return results.ToList();
       }

    }
}
