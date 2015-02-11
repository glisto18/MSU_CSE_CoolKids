using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BoeingSalesApp.DataAccess.Entities
{
    [Table("Meeting")]
    class Meeting
    {
        [PrimaryKey, Unique, AutoIncrement]
        public Guid ID { get; set; }

        public string Name { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateRestored { get; set; }

        public DateTime DateRemoved { get; set; }

        public bool Active { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public Guid Company { get; set; }

        public Guid CompanyContact { get; set; }

        public DateTime DateOfMeeting { get; set; }

        public Guid SalesBag { get; set; }

        public DateTime TimeOfMeeting { get; set; }

        public TimeZoneInfo TimeZone { get; set; }
    }
}
