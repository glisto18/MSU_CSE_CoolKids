using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using BoeingSalesApp.DataAccess.Entities;

namespace BoeingSalesApp.DataAccess.Repository
{
    class SalesBag_CategoryRepository : ISalesBag_CategoryRepository
    {
        private readonly SQLiteAsyncConnection _database;

        public SalesBag_CategoryRepository()
        {
            var db = new Database(BoeingSalesApp.Utility.TempSettings.DbPath);
            _database = db.GetAsyncConnection();
        }

        public SalesBag_CategoryRepository(IDatabase database)
        {
            _database = database.GetAsyncConnection();
        }

        public async Task SaveAsync(SalesBag_Category salesbagCategory)
        {
            await _database.InsertAsync(salesbagCategory);
        }

        public async Task DeleteAsync(SalesBag_Category salesbagCategory)
        {
            await _database.DeleteAsync(salesbagCategory);
        }

        public async Task<List<SalesBag_Category>> GetAllAsync()
        {
            var salesbagCategories = await _database.Table<SalesBag_Category>().ToListAsync();
            return salesbagCategories;
        }

        public async Task AddCategoryToSalesBag(Category category, SalesBag salesbag)
        {
            if(!await DoesExist(category, salesbag))
            {
                var categoryRepo = new CategoryRepository();
                var salesbagRepo = new SalesBagRepository();

                if(!await categoryRepo.DoesExist(category.ID))
                {
                    await categoryRepo.SaveAsync(category);
                }
                if(!await salesbagRepo.DoesExist(salesbag.ID))
                {
                    await salesbagRepo.SaveAsync(salesbag);
                }

                var NewRelationship = new SalesBag_Category
                {
                    Category = category.ID,
                    SalesBag = salesbag.ID
                };

                await SaveAsync(NewRelationship);
            }
        }

        public async Task<List<Category>> GetAllSalesBagCategories(SalesBag salesbag)
        {
            var relatinoships = await _database.Table<SalesBag_Category>().Where(x => x.SalesBag == salesbag.ID).ToListAsync();
            var categories = new List<Category>();
            foreach (var relationship in relatinoships)
            {
                categories.Add(await _database.FindAsync<Category>(relationship.Category));
            }

            return categories;
        }

        public async Task RemoveCategoryFromSalesBag(Category category, SalesBag salesbag)
        {
            var relationships = await _database.Table<SalesBag_Category>().Where(x => (x.Category == category.ID) && (x.SalesBag == salesbag.ID)).ToListAsync();

            foreach(var relationship in relationships)
            {
                await _database.DeleteAsync(relationship);
            }
        }

        public async Task RemoveAllCategories(SalesBag salesbag)
        {
            List<Category> allSalesbags = await this.GetAllSalesBagCategories(salesbag);
            foreach(var cat in allSalesbags)
            {
                await this.RemoveCategoryFromSalesBag(cat, salesbag);
            }
        }

        public async Task<bool> DoesExist(Category category, SalesBag salesbag)
        {
            var relationships = await _database.Table<SalesBag_Category>().Where(x => (x.Category == category.ID) && (x.SalesBag == salesbag.ID)).CountAsync();
            return relationships > 0;
        }
    }
}
