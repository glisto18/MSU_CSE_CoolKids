using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoeingSalesApp.DataAccess.Entities;

namespace BoeingSalesApp.Utility
{
    class DisplayMeeting : IDisplayItem
    {
        private Meeting _meeting;

        public Guid Id
        {
            get { return _meeting.ID; }
            set{}
        }

        public string DisplayName
        {
            get { return _meeting.Name; }
            set { }
        }

        public string DisplayInfo
        {
            get { return string.Format("{0}&#x0a;{1}&#x0a;{2}", _meeting.StartTime, _meeting.Subject, _meeting.Location); }
            set { }
        }

        public string DisplayTime
        {
            get { return _meeting.StartTime.ToString("M/dd/yy h:mm tt"); }
            set { }
        }

        public string DisplaySubject
        {
            get { return _meeting.Subject; }
            set { }
        }

        public string DisplayLocation
        {
            get { return _meeting.Location; }
            set { }
        }

        public Meeting GetMeeting()
        {
            return _meeting;
        }

        public string DisplayIcon { get; set; }

        public DisplayMeeting(Meeting meeting)
        {
            _meeting = meeting;
            DisplayIcon = "Assets/Meetings.png";
        }

        public async Task<bool> DoubleTap()
        {
            return false;
        }

        public async Task UpdateTitle(string name)
        {
            
        }
    
    }
}
