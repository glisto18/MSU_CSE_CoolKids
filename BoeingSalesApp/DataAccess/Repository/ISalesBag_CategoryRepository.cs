using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using BoeingSalesApp.DataAccess.Entities;

namespace BoeingSalesApp.DataAccess.Repository
{
    interface ISalesBag_CategoryRepository
    {
        Task SaveAsync(SalesBag_Category salesBagCategory);

        Task DeleteAsync(SalesBag_Category salesBagCategory);

        Task<List<SalesBag_Category>> GetAllAsync();

        Task AddCategoryToSalesBag(Category category, SalesBag salesbag);

        Task<List<Category>> GetAllSalesBagCategories(SalesBag salesbag);

        Task RemoveCategoryFromSalesBag(Category category, SalesBag salesbag);

        Task<bool> DoesExist(Category category, SalesBag salesbag);
    }
}
