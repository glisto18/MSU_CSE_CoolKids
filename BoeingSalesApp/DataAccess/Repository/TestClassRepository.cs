using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoeingSalesApp.DataAccess.Entities;
using SQLite;

namespace BoeingSalesApp.DataAccess.Repository
{
    class TestClassRepository : IRepository
    {
        private readonly SQLiteAsyncConnection _database;

        public TestClassRepository(IDatabase database)
        {
            _database = database.GetAsyncConnection();
        }

        public async Task SaveAsync(TestClass testClass)
        {
    //        if (testClass.mTestID == 0)
                await _database.InsertAsync(testClass);
//            else
  //              await _database.UpdateAsync(testClass);
        }

        public async Task DeleteAsync(TestClass testClass)
        {
            await _database.DeleteAsync(testClass);
        }

        public async Task<List<TestClass>> GetAllAsync()
        {
            var testClasses = await _database.Table<TestClass>().ToListAsync();
            return testClasses;
        }
    }
}
