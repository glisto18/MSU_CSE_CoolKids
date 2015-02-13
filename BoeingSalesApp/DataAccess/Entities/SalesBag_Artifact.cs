using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoeingSalesApp.DataAccess.Entities
{
    class SalesBag_Artifact
    {
        public Guid ID { get; set; }

        public Guid Artifact { get; set; }

        public Guid SalesBag { get; set; }
    }
}
