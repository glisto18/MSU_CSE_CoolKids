using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoeingSalesApp.DataAccess.Entities;
using BoeingSalesApp.DataAccess.Repository;

namespace BoeingSalesApp.Utility
{
    class DisplayCategory : IDisplayItem
    {
        private Category _category;

        public Guid Id
        {
            get { return _category.ID; }
            set { }
        }
        public string DisplayName
        {
            get { return _category.Name; }
            set {  }
        }

        public string DisplayIcon { get; set; }

        public string DisplayInfo
        {
            get { return string.Format("{0} Artifacts.", _numOfChildren); }
            set { }
        }

        public Category GetCategory()
        {
            return _category;
        }

        // number of child entities contained within this category, for now children can only be artifacts.
        private int _numOfChildren = 0;

        public DisplayCategory(DataAccess.Entities.Category category)
        {
            _category = category;
            DisplayIcon = "Assets/Artifacts.png";
        }

        public async Task SetNumOfChildren()
        {
            var artifactCategoryRepo = new DataAccess.Repository.Artifact_CategoryRepository();
            var artifactsInCategory = await artifactCategoryRepo.GetAllArtifactsForCategory(_category);
            _numOfChildren = artifactsInCategory.Count;
        }

        public async Task<bool> DoubleTap()
        {
            return true;
        }

        public async Task UpdateTitle(string newName)
        {
            CategoryRepository catRepo = new CategoryRepository();
            await catRepo.UpdateTitle(_category.ID, newName);
        }
    }
}
