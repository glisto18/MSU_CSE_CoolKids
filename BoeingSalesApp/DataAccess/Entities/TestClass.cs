using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

// ahl test

namespace BoeingSalesApp.DataAccess.Entities
{
    [Table("TestClass")]
    class TestClass
    {
        //[] 
        public int mTestID { get; set; }

        public string mTest_Name { get; set; }

        public override string ToString()
        {
            return mTest_Name + " " + mTestID;
        }
    }
}
