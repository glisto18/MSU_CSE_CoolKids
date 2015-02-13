using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BoeingSalesApp.DataAccess.Entities
{
    [Table("SalesBag")]
    class SalesBag
    {
        [PrimaryKey, Unique, AutoIncrement]
        public Guid ID { get; set; }

        public DateTime DateCreated { get; set; }

        public bool Active { get; set; }

        public string Name { get; set; }

        public DateTime DateRemoved { get; set; }
    }
}
