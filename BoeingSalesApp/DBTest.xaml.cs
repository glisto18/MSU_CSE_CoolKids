﻿using System;
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


            var seeder = new BoeingSalesApp.Utility.FakeSeeder();
            //await seeder.CreateArtifactCategory();
            //Status.Text = "done creating acr table";
            await seeder.CreateTestArtifactSalesBagRelationship();
            Status.Text = "Artifact Salesbag relationship should have been added";

            //await seeder.FakeSeedArtifacts();
            //await FetchCategories();
            //await seeder.FakeSeedCategories();
            //await FetchCategories();


            var acr = new Artifact_CategoryRepository();
            var foo = await acr.GetAllAsync();

            var bar = 1;
            //await _categoryRepository.SaveAsync(_category);
            //await FetchCategories();

            //Status.Text = string.Format("Name: {0} has been saved to your database.", _category.Name);
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            await _categoryRepository.DeleteAsync(_category);
            await FetchCategories();
            InitializeCategory();
            Status.Text = "category has been deleted to your database.";
        }

        private void CategoryList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0) return;
            
            _category = e.AddedItems[0] as Category;
            uxCategoryPanel.DataContext = _category;
            
        }
    }
}
