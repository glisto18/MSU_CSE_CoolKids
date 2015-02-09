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
using SQLite;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
// hello world

namespace BoeingSalesApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        
        private TestClass _testClass;
        private ITestClassRepository _testClassRepository;

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            InitializeTestClass();
            await InitializeDatabase();
            await UpdateTestClasses();
        }

        private void InitializeTestClass()
        {
            _testClass = new TestClass();
            CurrentTestClass.DataContext = _testClass;
        }

        private async Task InitializeDatabase()
        {
            string databasePath = Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\testClasses.db";
            Database database = new Database(databasePath);
            await database.Initialize();
            _testClassRepository = new TestClassRepository(database);
        }

        private async Task UpdateTestClasses()
        {
            TestClasses.ItemsSource = await _testClassRepository.GetAllAsync();
            var foo = TestClasses.Items;
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            await _testClassRepository.SaveAsync(_testClass);
            await UpdateTestClasses();
            Status.Text = string.Format("{0} has been saved to your database.", _testClass);
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            string mTest_Name = _testClass.ToString();
            await _testClassRepository.DeleteAsync(_testClass);
            await UpdateTestClasses();
            InitializeTestClass();

            Status.Text = string.Format("{0} has been removed from your contacts.", mTest_Name);
        }

        private void TestClasses_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0) return;
            _testClass = e.AddedItems[0] as TestClass;
            CurrentTestClass.DataContext = _testClass;
        }
    }


}
