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

namespace BoeingSalesApp
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class NewArtifactsView : Page
    {
        private enum PageState
        {
            All = 0,
            Category = 1,
            AllSalesBags = 2
        }

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private String categoryShown = "All";

        //DR - List for keeping track of selected artifacts between changing grid views
        private List<IDisplayItem> _selectedItems = new List<IDisplayItem>();

        private DataAccess.Repository.CategoryRepository _categoryRepo;
        private DataAccess.Repository.ArtifactRepository _artifactRepo;

        private bool _isInCategory = false;

        private PageState _currentState = PageState.All;


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
            if (_currentState == PageState.Category)
            {
                _currentState = PageState.All;
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
            //UxCategoryBox.ItemsSource = categories;

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

        private async Task FetchSalesBagContents(Guid salesBagId)
        {
            //DR - Fetch the category display items first and add them to the refreshed grid

            //DR - Fetch the artifact display items and add them to the grid after the categories
        }

        private async void Item_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var artifactPanel = (StackPanel) sender;
            var displayItem = (IDisplayItem) artifactPanel.DataContext;
            var doSomething = await displayItem.DoubleTap();

            if (doSomething)
            {
                //DR - It is the worst practice in the world to typecheck using string names, do not
                //  ever do this in the real world.  If anyone has any suggestions for how to typecheck these
                //  objects please let me know.  Cuz this makes me ashamed of myself.
                var foo = displayItem.GetType().Name;
                if (foo == "DisplayCategory")
                {
                    // if doSomething in this context is true, show the Category on the page
                    await FetchCategoryContents(displayItem.Id);
                    //_isInCategory = true;
                    _currentState = PageState.Category;
                }
                else if(foo == "DisplaySalesbag")
                {
                    await FetchSalesBagContents(displayItem.Id);
                }
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
            //UxCategoryBox.IsDropDownOpen = true;
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
            if (_currentState != PageState.AllSalesBags)
            {
                _selectedItems.Clear();
                foreach (IDisplayItem item in ArtifactsGridView.SelectedItems)
                {
                    _selectedItems.Add(item);
                }
            }
            

            if (ArtifactsGridView.SelectedItems.Count > 0 && _currentState == PageState.Category)
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
            //_isInCategory = true;
            _currentState = PageState.Category;
        }

        private async void AddToExistingSalesbag_Click(object sender, RoutedEventArgs e)
        {
            // if current state is not the salesbag, then we just want to view the existing salesbags
            if(_currentState != PageState.AllSalesBags)
            {
                _currentState = PageState.AllSalesBags;
                ArtifactsGridView.SelectedItems.Clear();
                
                var salesbagRepo = new SalesBagRepository();
                var displaySalesbags = DisplayConverter.ToDisplaySalebsag( await salesbagRepo.GetAllAsync());
                ArtifactsGridView.ItemsSource = displaySalesbags;
            }
            else // else means that we are already viewing the existing salesbags, and want to save the selected artifacts/categories to the selected salesbags.
            {
                _currentState = PageState.All;
                // save categories and artifacts to salesbags here.
                var categoriesToAdd = new List<DataAccess.Entities.Category>();
                var artifactsToAdd = new List<Artifact>();
                foreach (var item in _selectedItems)
                {
                    if (item.GetType() == typeof(DisplayCategory))
                    {
                        categoriesToAdd.Add(((DisplayCategory)item).GetCategory());
                    } 
                    else if(item.GetType() == typeof(DisplayArtifact))
                    {
                        artifactsToAdd.Add(((DisplayArtifact)item).GetArtifact());
                    }
                }

                var salesbagArtifactRepo = new SalesBag_ArtifactRepository();
                var salesbagCategoryRepo = new SalesBag_CategoryRepository();
                foreach(var item in ArtifactsGridView.SelectedItems)
                {
                    if (item.GetType() == typeof(DisplaySalesbag))
                    {
                        var bag = ((DisplaySalesbag)item).GetSalesbag();
                        
                        foreach (var artifact in artifactsToAdd)
                        {
                            await salesbagArtifactRepo.AddArtifactToSalesbag(artifact, bag);
                        }

                        foreach (var category in categoriesToAdd)
                        {
                            await salesbagCategoryRepo.AddCategoryToSalesBag(category, bag);
                        }
                    } 
                }

                
                await UpdateUi();
            }
            
        }

        private async void AddToNewSalesBag_Click(object sender, RoutedEventArgs e)
        {
            NewBagButton.Flyout.Hide();

            //DR - Create SalesBag object to be used for association
            //  If we wanted to we could put this stuff in the constructor of a salesbag
            //  I can't help but feel like it's bad practice to set it all manually
            SalesBag newBag = new SalesBag();
            newBag.Name = newBagName.Text;
            newBag.Active = true;
            newBag.DateCreated = DateTime.Now;
            newBag.ID = Guid.NewGuid();

            //DR - Save the new salesbag
            SalesBagRepository salesBagRepo = new SalesBagRepository();
            await salesBagRepo.SaveAsync(newBag);

            //DR - If there are no selected items, simply add the salesbag to the database and return
            if(ArtifactsGridView.SelectedItems == null)
            {
                return;
            }
            //DR - If there are selected items, iterate through them and associate them with the new salesbag
            else
            {
                //DR - We need these to create the artifact's and category's associations with the salesbag
                SalesBag_CategoryRepository bagCatRepo = new SalesBag_CategoryRepository();
                SalesBag_ArtifactRepository bagArtRepo = new SalesBag_ArtifactRepository();

                //DR - We need these to find the actual artifact and category objects to be used by the repo
                //  for adding to the db
                ArtifactRepository artRepo = new ArtifactRepository();
                CategoryRepository catRepo = new CategoryRepository();
                List<Artifact> artList = new List<Artifact>();

                //DR - Note, there are two Category classes, one in the BoeingSalesApp and one in the outlook plugin
                //  If we have time it would be convenient to refactor them so there is no ambiguity
                List<BoeingSalesApp.DataAccess.Entities.Category> catList = new List<DataAccess.Entities.Category>();

                //DR - This foreach iterates through each selected item on the grid and checks whether the item is a
                //  category or an artifact and adds the item to a list.  Those two lists will be used to associate
                //  objects with the salesbag.
                foreach(IDisplayItem i in ArtifactsGridView.SelectedItems)
                {
                    if(i.GetType() == typeof(DisplayArtifact))
                    {
                        artList.Add(((DisplayArtifact)i).GetArtifact());
                    }
                    else if(i.GetType() == typeof(DisplayCategory))
                    {
                        catList.Add(((DisplayCategory)i).GetCategory());
                    }
                }

                //DR - Iterate through the artifact list and add an association to the new Salesbag
                foreach(Artifact i in artList)
                {
                    await bagArtRepo.AddArtifactToSalesbag(i, newBag);
                }

                //DR - Iterate through the category list and add an association to the new salesbag
                foreach(BoeingSalesApp.DataAccess.Entities.Category i in catList)
                {
                    await bagCatRepo.AddCategoryToSalesBag(i, newBag);
                }
                return;
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            //DR - Bind the grid to just the salesbags
            ArtifactsGridView.SelectedItems.Clear();
            var salesbagRepo = new SalesBagRepository();
            var displaySalesbags = DisplayConverter.ToDisplaySalebsag(await salesbagRepo.GetAllAsync());
            ArtifactsGridView.ItemsSource = displaySalesbags;
        }
    }
}
