using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoeingSalesApp.Utility
{
    class DisplayCategory : IDisplayItem
    {
        private DataAccess.Entities.Category _category;
        public string DisplayName
        {
            get { return _category.Name; }
            set {  }
        }

        public string DisplayInfo
        {
            get { return string.Format("{0} Artifacts.", _numOfChildren); }
            set { }
        }

        // number of child entities contained within this category, for now children can only be artifacts.
        private int _numOfChildren = 0;

        public DisplayCategory(DataAccess.Entities.Category category)
        {
            _category = category;

            // set _numOfChildren here
        }
    }
}
