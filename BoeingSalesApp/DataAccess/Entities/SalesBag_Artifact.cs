using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BoeingSalesApp.DataAccess.Entities
{
    [Table("SalesBag_Artifact")]
    class SalesBag_Artifact
    {
        [PrimaryKey, Unique, AutoIncrement]
        public Guid ID { get; set; }

        public Guid Artifact { get; set; }

        public Guid SalesBag { get; set; }
    }
}
