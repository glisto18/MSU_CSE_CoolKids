using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BoeingSalesApp.DataAccess.Entities
{
    [Table("Note")]
    class Note
    {
        [PrimaryKey, Unique, AutoIncrement]
        public Guid ID { get; set; }

        public Guid Meeting { get; set; }

        public string Content { get; set; }

        public DateTime DateCreated { get; set; }

        public int Position { get; set; }
    }
}
