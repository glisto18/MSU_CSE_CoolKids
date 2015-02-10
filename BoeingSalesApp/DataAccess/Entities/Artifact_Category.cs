using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BoeingSalesApp.DataAccess.Entities
{
    [Table("Artifact_Category")]
    class Artifact_Category
    {
        [PrimaryKey, Unique, AutoIncrement]
        public Guid ID { get; set; }

        public Guid CategoryID { get; set; }

        public Guid ArtifactID { get; set; }
    }
}
