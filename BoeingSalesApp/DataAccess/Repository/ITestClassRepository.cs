using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoeingSalesApp.DataAccess.Entities;

namespace BoeingSalesApp.DataAccess.Repository
{
    interface ITestClassRepository
    {
        Task SaveAsync(TestClass testClass);

        Task DeleteAsync(TestClass testClass);

        Task<List<TestClass>> GetAllAsync();
    }
}
