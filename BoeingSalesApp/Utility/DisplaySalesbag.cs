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
        private int size;

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
            get 
            {
                return size.ToString() + " Artifacts"; 
            }
            set{}
        }

        public SalesBag GetSalesbag()
        {
            return _salesbag;
        }

        public DisplaySalesbag(SalesBag salesbag)
        {
            _salesbag = salesbag;
            DisplayIcon = "Assets/SalesBag_blue.png";
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

        public async Task getArtNum()
        {
            SalesBag_ArtifactRepository bagArtRepo = new SalesBag_ArtifactRepository();
            List<Artifact> artList = await bagArtRepo.GetAllSalesBagArtifacts(_salesbag);
            size = artList.Count;
            Artifact_CategoryRepository artCatRepo = new Artifact_CategoryRepository();
            SalesBag_CategoryRepository bagCatRepo = new SalesBag_CategoryRepository();
            List<Category> catList = await bagCatRepo.GetAllSalesBagCategories(_salesbag);
            foreach(var salesCategory in catList)
            {
                artList = await artCatRepo.GetAllArtifactsForCategory(salesCategory);
                size += artList.Count;
            }
        }
    }
}
