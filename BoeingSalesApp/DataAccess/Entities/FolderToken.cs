using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BoeingSalesApp.DataAccess.Entities
{
    [Table("FolderToken")]
    class FolderToken
    {
        [PrimaryKey, Unique, AutoIncrement]
        public Guid ID {get; set;}

        public DateTime DateCreated { get; set; }

        public string Token { get; set; }

        public override string ToString()
        {
            return Token;
        }
    }
}
