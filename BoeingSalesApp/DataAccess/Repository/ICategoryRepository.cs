using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoeingSalesApp.DataAccess.Entities;

namespace BoeingSalesApp.DataAccess.Repository
{
    interface ICategoryRepository
    {
        Task SaveAsync(Category category);

        Task DeleteAsync(Category category);

        Task<List<Category>> GetAllAsync();

        Task<Category> Get(Guid categoryId);

        Task<Category> Get(string name);

        Task<bool> DoesExist(Guid categoryId);

        Task<bool> DoesExist(string name);

        Task<List<Category>> GetCategoriesByName(string title);
    }
}
