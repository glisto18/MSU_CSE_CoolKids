using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoeingSalesApp.DataAccess;
using BoeingSalesApp.DataAccess.Repository;
using BoeingSalesApp.DataAccess.Entities;

namespace BoeingSalesApp.Utility
{
    // class to used to insert fake data into db until we get data stored
    class FakeSeeder
    {
        private string _dbPath;
        private Database _db;

        public FakeSeeder()
        {
            var test = Utility.TempSettings.DbPath;
        }

        private async Task InitDb()
        {
            _db = new Database(TempSettings.DbPath);
            await _db.Initialize();
        }

        public async Task CreateArtifactCategory()
        {
            await InitDb();
            var acr = new Artifact_CategoryRepository();
            var newRelationship = new Artifact_Category();
            await acr.SaveAsync(newRelationship);
        }
       
        public async Task FakeSeedCategories()
        {
            await InitDb();
            var categoryRepository = new CategoryRepository(_db);

            var categories = new List<Category>();
            categories.Add(new Category
            {
                Name = "Lancer",
                Active = true
            });


            categories.Add(new Category
            {
                Name = "GPS IIF",
                Active = true
            });

            categories.Add(new Category
            {
                Name = "Software",
                Active = true
            });

            categories.Add(new Category
            {
                Name = "Training",
                Active = true
            });

            foreach (Category category in categories)
            {
                await categoryRepository.SaveAsync(category);
            }
        }

        public async Task FakeSeedArtifacts()
        {
            await InitDb();

            var artifactRepository = new ArtifactRepository(_db);

            var artifacts = new List<Artifact>();
            artifacts.Add(new Artifact
            {
                Path = "CoolPlanePath",
                Title = "F16",
                FileName = "F16.pdf",
                FileType = "pdf",
                DateAdded = DateTime.Now,
                Active = true
            });


            artifacts.Add(new Artifact
            {
                Path = "satellite",
                Title = "702 MP Satellite",
                FileName = "mpsatellite.pdf",
                FileType = "pdf",
                DateAdded = DateTime.Now,
                Active = true
            });

            artifacts.Add(new Artifact
            {
                Path = "Brochure",
                Title = "Brochure",
                FileName = "Brochure.pdf",
                FileType = "pdf",
                DateAdded = DateTime.Now,
                Active = true
            });

            artifacts.Add(new Artifact
            {
                Path = "MSF13",
                Title = "MSF13-0126",
                FileName = "msf13.png",
                FileType = "png",
                DateAdded = DateTime.Now,
                Active = true
            });

            foreach (Artifact artifact in artifacts)
            {
                await artifactRepository.SaveAsync(artifact);
            }

        }

        public async Task CreateTestArtifactSalesBagRelationship()
        {
            await InitDb();

            var artifact = new Artifact
            {
                Path = "test salesbag relationship",
                Title = "artfact for salesbag",
                FileName = "something.pdf",
                FileType = "pdf",
                DateAdded = DateTime.Now,
                Active = true
            };

            var salesbag = new SalesBag
            {
                Name = "Im a salesbag!"
            };

            var salesbagArtifact = new SalesBag_ArtifactRepository();
            //add relationship here
            
        }

        public async Task CreateTestArtifactCategoryRelationsip()
        {
            await InitDb();

            var artifact = new Artifact
            {
                Path = "testRelationshipPath",
                Title = "testRelationshipArtifact",
                FileName = "testrel.pdf",
                FileType = "pdf",
                DateAdded = DateTime.Now,
                Active = true
            };

            var category = new Category
            {
                Name = "TestRelationship Category",
                Active = true
            };

            var artifactCategory = new Artifact_CategoryRepository(_db);
            await artifactCategory.AddRelationship(artifact, category);
        }
    }
}
