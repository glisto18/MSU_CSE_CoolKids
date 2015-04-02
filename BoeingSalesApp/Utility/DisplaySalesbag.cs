using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoeingSalesApp.DataAccess.Entities;
using BoeingSalesApp.DataAccess.Repository;

namespace BoeingSalesApp.Utility
{
    class DisplaySalesbag : IDisplayItem
    {
        private SalesBag _salesbag;

        public Guid Id
        {
            get { return _salesbag.ID; }
            set { }
        }

        public string DisplayIcon { get; set; }

        public string DisplayName
        {
            get { return _salesbag.Name; }
            set { }
        }

        public string DisplayInfo
        {
            get { return ""; }
            set{}
        }

        public SalesBag GetSalesbag()
        {
            return _salesbag;
        }

        public DisplaySalesbag(SalesBag salesbag)
        {
            _salesbag = salesbag;
            DisplayIcon = "Assets/SalesBag.scale-100.png";
        }

        public async Task<bool> DoubleTap()
        {
            return true;
        }

        public async Task UpdateTitle(string newName)
        {
            SalesBagRepository bagRepo = new SalesBagRepository();
            await bagRepo.UpdateTitle(_salesbag.ID, newName);
        }
    }
}
