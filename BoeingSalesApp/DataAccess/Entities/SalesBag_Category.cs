using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BoeingSalesApp.DataAccess.Entities
{
    [Table("SalesBag_Category")]
    class SalesBag_Category
    {
        [PrimaryKey, Unique, AutoIncrement]
        public Guid ID { get; set; }

        public Guid Category { get; set; }

        public Guid SalesBag { get; set; }
    }
}
