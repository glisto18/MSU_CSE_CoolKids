using BoeingSalesApp.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Windows.SystemParameters;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using BoeingSalesApp.DataAccess.Entities;
using BoeingSalesApp.DataAccess.Repository;
using BoeingSalesApp.Utility;
using Microsoft.Office.Interop.Outlook;


// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

//This page needs to be here

namespace BoeingSalesApp
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class NewArtifactsView : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private String categoryShown = "All";

        private DataAccess.Repository.CategoryRepository _categoryRepo;
        private DataAccess.Repository.ArtifactRepository _artifactRepo;

        private bool _isInCategory = false;


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

        private async void onBack(object sender, RoutedEventArgs e)
        {
            if (_isInCategory)
            {
                _isInCategory = false;
                lblCurrentCategory.Text = "All";
                await UpdateUi();
            }
            else
            {
                Frame.GoBack();
            }
            
        }


        public NewArtifactsView()
        {
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
        /// <see cref="Common.NavigationHelper.LoadState"/>
        /// and <see cref="Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            _categoryRepo = new DataAccess.Repository.CategoryRepository();
            _artifactRepo = new DataAccess.Repository.ArtifactRepository();


            var foo = await _categoryRepo.Search("cat");

            navigationHelper.OnNavigatedTo(e);

            await UpdateUi();

            await CheckForNewArtifacts();
        }

        private async Task UpdateUi()
        {
            // added ahl
            
            var displayItems = new List<IDisplayItem>();
            var allCategories = await _categoryRepo.GetAllDisPlayCategoriesAsync();
            var allArtifacts = await _artifactRepo.GetAllUncategorizedArtifacts();

            displayItems.AddRange(allCategories);
            displayItems.AddRange(allArtifacts);
            ArtifactsGridView.ItemsSource = displayItems;

            SetCategoryCombobox(allCategories);
        }

        private async Task CheckForNewArtifacts()
        {
            // added ahl - check for new artifacts to upload
            var fileStore = new Utility.FileStore();
            var newArtifacts = await fileStore.CheckForNewArtifacts();
            if (newArtifacts.Count > 0)
            {
                // added ahl 2/25
                var msg = new Windows.UI.Popups.MessageDialog("New artifacts found.");
                msg.Commands.Add(new Windows.UI.Popups.UICommand(
                    "Edit New Artifacts.", null
                    ));
                msg.Commands.Add(new Windows.UI.Popups.UICommand(
                    "Use Defualt Artifacts Attributes.", null
                    ));
                await msg.ShowAsync();
            }
        }

        private void SetCategoryCombobox(List<DisplayCategory> categories)
        {
            // add each category to the combobox
            UxCategoryBox.ItemsSource = categories;

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void newCat_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //newCategoryPopup.
            /*if (myPopup != null) 
            {

                double height = System.Windows.SystemParameters.FullPrimaryScreenHeight;
                double screenWidth = System.Windows.SystemParameters.FullPrimaryScreenWidth;

                
                //myPopup.IsOpen = true;
            }*/
                
        }

        private async Task FetchCategoryContents(Guid categoryId)
        {
            var artifactCategoryRepo = new DataAccess.Repository.Artifact_CategoryRepository();
            var category = await _categoryRepo.Get(categoryId);
            var allArtifacts = await artifactCategoryRepo.GetAllDisplayArtifactsForCategory(category);

            ArtifactsGridView.ItemsSource = allArtifacts;
            lblCurrentCategory.Text = category.Name;
        }

        private async void Item_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var artifactPanel = (StackPanel) sender;
            var displayItem = (IDisplayItem) artifactPanel.DataContext;
            var doSomething = await displayItem.DoubleTap();

            if (doSomething)
            {
                // if doSomething in this context is true, show the Category on the page
                await FetchCategoryContents(displayItem.Id);
                _isInCategory = true;
            }


            //var fileStore = new Utility.FileStore();
            //var artifact = await fileStore.GetArtifact(artifactContext.FileName);

            //await Windows.System.Launcher.LaunchFileAsync(artifact);
        }

        private void Item_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            //if (ArtifactsGridView.SelectedItems.Count > 0)
            //{
            //    uxAppBar.
            //    uxAppBar.IsOpen = true;
            //}
            //else
            //{
            //    uxAppBar.IsOpen = false;
            //}
            
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //DR - On button click, we want to hide and display the listbox
            //  If the listbox is visible, collapse it.  Otherwise...
            if (this.SalesBagComboBox.Visibility == Windows.UI.Xaml.Visibility.Visible)
                this.SalesBagComboBox.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            else
            {
                //DR - This is a bit hacky but in order to display a "New SalesBag" item in the listbox
                //  we need to make a salesbag with the name "New SalesBag".  Since the bag isn't saved into
                //  the database we can do this every time the listbox is made visible.
                var emptyNewBag = new SalesBag();
                emptyNewBag.Name = "[New SalesBag]";

                //DR - emptySalesbag is made, make combobox visible, get the list of all salesbags
                this.SalesBagComboBox.Visibility = Windows.UI.Xaml.Visibility.Visible;
                List<SalesBag> salesBagList = await GetSalesBags();

                //DR - Add the empty bag to the list and set as the box's item source
                //  badabing badaboom
                salesBagList.Add(emptyNewBag);
                this.SalesBagComboBox.ItemsSource = salesBagList;
            }
        }

        private async Task<List<SalesBag>> GetSalesBags()
        {
            var foo = new DataAccess.Repository.SalesBagRepository();
            List<SalesBag> bar = await foo.GetAllAsync();
            return bar;
        }

        private void launchme(object sender, RoutedEventArgs e)
        {
            Windows.UI.Xaml.Controls.Primitives.FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private async void titleChange(object sender, RoutedEventArgs e)
        {
            if (ArtifactsGridView.SelectedItems.Count == 1)
            {
                string arttit = newtitle.Text;
                var selectItem = ((IDisplayItem)ArtifactsGridView.SelectedItem).Id;
                await _artifactRepo.UpdateTitle(selectItem, arttit);
            }
            newtitle.Text = "";
            titBox.Hide();
            await UpdateUi();
        }

        private void UxCategoryBox_OnDragOver(object sender, DragEventArgs e)
        {
            UxCategoryBox.IsDropDownOpen = true;
        }

        private async void Item_OnDrop(object sender, DragEventArgs e)
        {
            var selectedItems = ArtifactsGridView.SelectedItems;
            var destinationItem = (IDisplayItem)((StackPanel) sender).DataContext;
            if (destinationItem.GetType() != typeof (DisplayCategory))
            {
                return;
            }
            var destinationCategory = ((DisplayCategory)destinationItem).GetCategory();
            var artifactCategoryRepo = new Artifact_CategoryRepository();
            foreach (IDisplayItem item in selectedItems)
            {
                if (item.GetType() != typeof (DisplayArtifact))
                {
                    return;
                }
                var artifact = ((DisplayArtifact)item).GetArtifact();
                await artifactCategoryRepo.AddRelationship(artifact, destinationCategory);
            }
            
            await UpdateUi();
        }

        private void ArtifactsGridView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ArtifactsGridView.SelectedItems.Count > 0 && _isInCategory == true)
            {
                uxRemoveFromCategory.Visibility = Visibility.Visible;
            }
            else
            {
                uxRemoveFromCategory.Visibility = Visibility.Collapsed;
            }
        }

        private async void onFind(object sender, RoutedEventArgs e)
        {
            var allarts = await _artifactRepo.GetAllAsync();
            ArtifactsGridView.ItemsSource = allarts;
            var fewarts = Utility.DisplayConverter.ToDisplayArtifacts(await _artifactRepo.Search(magicmaker.Text));
            var fewcat = Utility.DisplayConverter.ToDisplayCategories(await _categoryRepo.Search(magicmaker.Text));
            var dispitems = new List<Utility.IDisplayItem>();
            dispitems.AddRange(fewarts);
            dispitems.AddRange(fewcat);
            ArtifactsGridView.ItemsSource = dispitems;
            lblCurrentCategory.Text = "Search";
            _isInCategory = true;
        }
    }
}
