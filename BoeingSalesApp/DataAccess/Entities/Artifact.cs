using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoeingSalesApp.DataAccess.Entities
{
    [Table("Artifact")]
    class Artifact
    {
        [PrimaryKey, Unique, AutoIncrement]
        public Guid ID { get; set; }

        public string Path { get; set; }

        public string Title { get; set; }

        public string FileType { get; set; }

        public string FileName { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateDeleted { get; set; }

        public DateTime DateRestored { get; set; }

        public bool Active { get; set; }
   
    }
}
