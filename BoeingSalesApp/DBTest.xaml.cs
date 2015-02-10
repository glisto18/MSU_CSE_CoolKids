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
        private Category _newCategory;
        private ICategoryRepository _categoryRepository;

        public DBTest()
        {
            this.InitializeComponent();
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            _newCategory = new Category();
            uxCategoryPanel.DataContext = _newCategory;

            await InitializeDatabase();

            FetchCategories();

            
        }

        private async Task FetchCategories()
        {
            uxCategoryList.ItemsSource = await _categoryRepository.GetAllAsync();
            var foo = uxCategoryList.Items;
        }

        private async Task InitializeDatabase()
        {
            string databasePath = Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\Categories.db";
            Database database = new Database(databasePath);
            await database.Initialize();
            _categoryRepository = new CategoryRepository(database);
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            await _categoryRepository.SaveAsync(_newCategory);
            await FetchCategories();

            Status.Text = string.Format("{0} has been saved to your database.", _newCategory.Name);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CategoryList_OnSelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
