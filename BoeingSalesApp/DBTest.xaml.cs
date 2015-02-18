using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BoeingSalesApp.DataAccess;
using BoeingSalesApp.DataAccess.Entities;
using BoeingSalesApp.DataAccess.Repository;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BoeingSalesApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DBTest : Page
    {
        private Category _category;
        private ICategoryRepository _categoryRepository;
        private IArtifactRepository _artifactsRepository;
        private IDatabase _database;

        public DBTest()
        {
            this.InitializeComponent();
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            InitializeCategory();

            await InitializeDatabase();

            await FetchCategories();

            
        }

        private void InitializeCategory()
        {
            _category = new Category();
            uxCategoryPanel.DataContext = _category;

           
        }

        private async Task FetchCategories()
        {
            uxCategoryList.ItemsSource = await _categoryRepository.GetAllAsync();
            var foo = uxCategoryList.Items;


            uxArtifactsList.ItemsSource = await _artifactsRepository.GetAllAsync();
            var d = uxArtifactsList.Items;



            var artifacts = await _artifactsRepository.GetAllAsync();
            uxTestGrid.ItemsSource = artifacts;
        }

        private async Task InitializeDatabase()
        {
            string databasePath = Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + BoeingSalesApp.Utility.TempSettings.DbName;
            _database = new Database(databasePath);
            await _database.Initialize();
            _categoryRepository = new CategoryRepository(_database);
            _artifactsRepository = new ArtifactRepository(_database);
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            //var artifactRepo = new ArtifactRepository(_database);
            //var newArtifact = new Artifact{
            //    Active = true,
            //    Title = "a new artifact",
            //    DateAdded = DateTime.Now
            //};
            //await artifactRepo.SaveAsync(newArtifact);
            //var test = await artifactRepo.Get(newArtifact.ID);
            
            //Status.Text = test.Title;


           // var seeder = new BoeingSalesApp.Utility.FakeSeeder();
            //await seeder.FakeSeedArtifacts();
            //await seeder.FakeSeedCategories();
            //await seeder.CreateTestArtifactSalesBagRelationship();
            //await seeder.CreateTestArtifactCategoryRelationsip();


            //await seeder.CreateArtifactCategory();
            //Status.Text = "done creating acr table";
            //await seeder.CreateTestArtifactSalesBagRelationship();
            //Status.Text = "Artifact Salesbag relationship should have been added";

            //await seeder.FakeSeedArtifacts();
            //await FetchCategories();
            //await seeder.FakeSeedCategories();
            //await FetchCategories();

            //Status.Text = "Seeded artifacts and categories";


            //var artifactRepo = new ArtifactRepository();
            //var duckDocs = await artifactRepo.GetArtifactsByTitle("Duck");
            //var duckDoc = duckDocs[0];
            //var blueTrucks = await artifactRepo.GetArtifactsByTitle("Blue Truck");
            //var blueTruck = blueTrucks[0];

            //var categoryRepo = new CategoryRepository();
            //var planeCats = await categoryRepo.GetCategoriesByName("Planes");
            //var planCat = planeCats[0];
            //Status.Text = string.Format("Name: {0} ", planCat.Name);

            //var artifactsCategories = new Artifact_CategoryRepository();
            ////await artifactsCategories.AddRelationship(duckDoc, planCat);
            ////await artifactsCategories.AddRelationship(blueTruck, planCat);
            //await artifactsCategories.RemoveArtifactFromCategory(duckDoc, planCat);

            //var planeArtifacts = await artifactsCategories.GetAllArtifactsForCategory(planCat);

            //foreach (var artifact in planeArtifacts)
            //{
            //    Status.Text += "  Title: " + artifact.Title;
            //}

            //await _categoryRepository.SaveAsync(_category);
            //await FetchCategories();

            //Status.Text = string.Format("Name: {0} has been saved to your database.", _category.Name);

            var fileUploader = new Utility.FileStore();
            await fileUploader.CheckForNewArtifacts();
            Status.Text = "done";
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            await _categoryRepository.DeleteAsync(_category);
            await FetchCategories();
            InitializeCategory();
            Status.Text = "category has been deleted from your database.";
        }

        private void CategoryList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0) return;
            
            _category = e.AddedItems[0] as Category;
            uxCategoryPanel.DataContext = _category;
            
        }
    }
}
