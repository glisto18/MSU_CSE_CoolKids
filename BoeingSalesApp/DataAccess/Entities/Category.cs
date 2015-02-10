using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BoeingSalesApp.DataAccess.Entities
{
    [Table("Category")]
    class Category
    {
        [PrimaryKey, Unique, AutoIncrement]
        public Guid ID { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }
    }
}
