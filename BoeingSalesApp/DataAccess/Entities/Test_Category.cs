using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BoeingSalesApp.DataAccess.Entities
{
    [Table("Test_Category")]
    class Test_Category
    {
        public Guid ID { get; set; }

        public Guid TestClassID { get; set; }

        public Guid CategoryID { get; set; }
    }
}
