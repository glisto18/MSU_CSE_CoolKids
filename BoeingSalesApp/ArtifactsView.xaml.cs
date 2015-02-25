using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using BoeingSalesApp.Common;
using BoeingSalesApp.DataAccess;
using BoeingSalesApp.DataAccess.Entities;
using BoeingSalesApp.DataAccess.Repository;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace BoeingSalesApp
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class ArtifactsView : Page
    {
        //
        // Member variables for DB interaction
        //
        private CategoryRepository _categoryRepository;
        private ArtifactRepository _artifactsRepository;

        //
        // I'm not sure what these members do yet ... - JW
        //
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
            get
            {
                return this.navigationHelper;
            }
        }

        public ArtifactsView()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;

            _artifactsRepository = new ArtifactRepository();
            _categoryRepository = new CategoryRepository();

            CreateCategoryButton.Flyout = myFlyout;
        }

        /// <summary>
        /// Called when user clicks "Create category" icon 
        /// </summary>
        private void showFlyout(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        /// <summary>
        /// Get categories from backend and bind to frontend
        /// </summary>
        /// <returns></returns>
        private async Task FetchArtifacts()
        {
            var artifacts = await _artifactsRepository.GetAllAsync();
            GridView gridView = (GridView)this.FindName("ArtifactsGridView");
            gridView.ItemsSource = artifacts;
        }

        /// <summary>
        /// Get categories from backend and bind to frontend
        /// </summary>
        /// <returns></returns>
        private async Task FetchCategories()
        {
            
            var categories = await _categoryRepository.GetAllAsync();
            ListView listView = (ListView)this.FindName("CategoryList");
            listView.ItemsSource = categories;
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Assign a collection of bindable groups to this.DefaultViewModel["Groups"]
        }

        private async void onCreateCategory(object sender, RoutedEventArgs e)
        {
            CreateCategoryButton.Flyout.Hide();
            
            Category category = new Category();
            category.Name = categoryInput.Text;
            category.Active = true;
            await _categoryRepository.SaveAsync(category);

            categoryInput.Text = "";

            await FetchCategories();
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


            navigationHelper.OnNavigatedTo(e);
            await FetchCategories();
            await FetchArtifacts();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void Artifact_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var artifactPanel = (StackPanel)sender;
            var artifactContext = (Artifact)artifactPanel.DataContext;
            var fileStore = new BoeingSalesApp.Utility.FileStore();
            var artifact = await fileStore.GetArtifact(artifactContext.FileName);

            await Windows.System.Launcher.LaunchFileAsync(artifact);
        }


        private async void TextBlock_Drop(object sender, DragEventArgs e)
        {
            var selectedArtifacts = this.ArtifactsGridView.SelectedItems;
            TextBlock destTextblock = (TextBlock)sender;
            Category roo = (Category)destTextblock.DataContext;

            foreach(Artifact i in selectedArtifacts)
            {
                Artifact_CategoryRepository bar = new Artifact_CategoryRepository();
                await bar.AddRelationship(i, roo);
            }
        }
    }
}
