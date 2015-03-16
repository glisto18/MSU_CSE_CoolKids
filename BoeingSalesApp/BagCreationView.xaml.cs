using BoeingSalesApp.Common;
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
using BoeingSalesApp.DataAccess.Repository;
using System.Threading.Tasks;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BoeingSalesApp
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class BagCreationView : Page
    {
        private SalesBag_CategoryRepository _salesBagCategoryRepository;
        private SalesBagRepository _salesBagRepository;

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public BagCreationView()
        {
            _salesBagCategoryRepository = new SalesBag_CategoryRepository();
            _salesBagRepository = new SalesBagRepository();

            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);

            //DR - Below, capture all categories and bind to grid
            CategoryRepository catRepo = new CategoryRepository();
            List<BoeingSalesApp.DataAccess.Entities.Category> catList = await catRepo.GetAllAsync();
            foreach(BoeingSalesApp.DataAccess.Entities.Category i in catList)
            {
                this.sourceGrid.Items.Add(i);
            }

            //DR - This could probably be set in the xaml for the grid itself
            this.sourceGrid.SelectionMode = ListViewSelectionMode.Multiple;

            //DR - Do we capture artifacts without categories?  Do those exist?
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void sourceGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //DR - The new salesbag to be added to the db
            DataAccess.Entities.SalesBag newbag = new DataAccess.Entities.SalesBag();
            newbag.Name = this.bagName.Text;
            newbag.Active = true;
            newbag.DateCreated = DateTime.Now;
            newbag.ID = Guid.NewGuid();

            //DR - This is the means of querying the database with an interface
            //  Found the code here https://msdn.microsoft.com/en-us/library/bb341406(v=vs.110).aspx
            IEnumerable<DataAccess.Entities.Category> query =
                this.sourceGrid.SelectedItems.Cast<DataAccess.Entities.Category>().Select(x => x);

            //DR - For each category, create a relationship tieing the new bag to each selected category
            foreach(DataAccess.Entities.Category i in query)
            {
                //DR - This is breaking as a nullexception on my end.  It should just create the relationship.
                await _salesBagCategoryRepository.AddCategoryToSalesBag(i, newbag);
            }

            //DR - Save the new bag to the database
            //await _salesBagRepository.SaveAsync(newbag);

            //After bag is created and saved to db, navigate back to the salesbag page.
            this.Frame.Navigate(typeof(SalesBagsView));
        }
    }
}
