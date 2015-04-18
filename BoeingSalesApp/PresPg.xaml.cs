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
using System.Threading.Tasks;

namespace BoeingSalesApp
{
    public sealed partial class PresPg : Page
    {
        private DataAccess.Repository.SalesBag_ArtifactRepository _asalesbagRepo;
        private DataAccess.Repository.SalesBagRepository _salesbagRepo;
        private DataAccess.Repository.MeetingRepository _meetingRepo;
        private DataAccess.Repository.ArtifactRepository _artRepo;
        private DataAccess.Repository.CategoryRepository _catRepo;
        private DataAccess.Entities.Meeting launchmeet;
        private DataAccess.Repository.SalesBag_CategoryRepository _categorySalesbagRepo;
        public PresPg()
        {
            this.InitializeComponent();
            _asalesbagRepo = new DataAccess.Repository.SalesBag_ArtifactRepository();
            _categorySalesbagRepo = new DataAccess.Repository.SalesBag_CategoryRepository();
            _salesbagRepo = new DataAccess.Repository.SalesBagRepository();
            _meetingRepo = new DataAccess.Repository.MeetingRepository();
            _artRepo = new DataAccess.Repository.ArtifactRepository();
            _catRepo = new DataAccess.Repository.CategoryRepository();
        }

        private async Task ResetUi()
        {
            try
            {
                DataAccess.Entities.SalesBag salesbagChosen = await _salesbagRepo.Get(launchmeet.SalesBag);
                var all = new List<Utility.IDisplayItem>();
                var displayArtifacts = Utility.DisplayConverter.ToDisplayArtifacts(await _asalesbagRepo.GetAllSalesBagArtifacts(salesbagChosen));
                var displayCategories = Utility.DisplayConverter.ToDisplayCategories(await _categorySalesbagRepo.GetAllSalesBagCategories(salesbagChosen));
                foreach (var category in displayCategories)
                {
                    await category.SetNumOfChildren();
                }
                all.AddRange(displayArtifacts);
                all.AddRange(displayCategories);
                ArtView.ItemsSource = all;
            }
            catch (NullReferenceException) { }
        }
        /********************************************************************
         * Note field is the meeting unique ID + ".txt"
         * Should display all artifacts from salesbag
         *********************************************************************/
        protected async override void OnNavigatedTo(NavigationEventArgs e) 
        {
            launchmeet = (DataAccess.Entities.Meeting)e.Parameter;
            launchmeet.Note = launchmeet.ID.ToString() + ".txt";
            await ResetUi();
        }
        /*****************************************************************************
         * Creates file from "Note" field if not existed
         * Update database by deleting and saving new meeting
         ***************************************************************************/
        private async void addNote(object sender, RoutedEventArgs e)
        {
            showFlyout(sender, e);
            try { await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync(launchmeet.Note, Windows.Storage.CreationCollisionOption.FailIfExists); }
            catch { }
            var newMeeting = launchmeet;
            await _meetingRepo.DeleteAsync(launchmeet);
            await _meetingRepo.SaveAsync(newMeeting);
        }
        /**************************************************************
         * Adds lines from textblock to file connected to meeting
         *****************************************************************/
        private async void addLines(object sender, RoutedEventArgs e)
        {
            if (launchmeet.Note == null)
                return;
            
            var notefile = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(launchmeet.Note);
            var listadd = await Windows.Storage.FileIO.ReadLinesAsync(notefile);
            listadd.Add(lines.Text);
            await Windows.Storage.FileIO.WriteLinesAsync(notefile, listadd);
            lines.Text = "";
            noteE.Hide();
        }
        /****************************************************************
         * Analytics done here
         * End meeting returns to meetings page
         *****************************************************************/
        private void doneMeet(object sender, RoutedEventArgs e)
        {
            // pass meeting name to survery here
            this.Frame.Navigate(typeof(SurveyView), launchmeet.Name);
        }
        private void showFlyout(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }
        /*******************************************************************
         * Displays all artifacts and categories based on any string
         *      currently in "magicmaker"
         **********************************************************************/
        private async void onFind(object sender, RoutedEventArgs e)
        {
            var fewarts = Utility.DisplayConverter.ToDisplayArtifacts(await _artRepo.Search(magicmaker.Text));
            var fewcat = Utility.DisplayConverter.ToDisplayCategories(await _catRepo.Search(magicmaker.Text));
            var dispitems = new List<Utility.IDisplayItem>();
            dispitems.AddRange(fewarts);
            dispitems.AddRange(fewcat);
            ArtView.ItemsSource = dispitems;
            backbut.Visibility = Visibility.Visible;
        }

        private async Task FetchCategoryContents(Guid categoryId)
        {
            var artifactCategoryRepo = new DataAccess.Repository.Artifact_CategoryRepository();
            var category = await _catRepo.Get(categoryId);
            var allArtifacts = await artifactCategoryRepo.GetAllDisplayArtifactsForCategory(category);

            ArtView.ItemsSource = allArtifacts;
        }

        private async void Item_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var artifactPanel = (StackPanel)sender;
            var displayItem = (Utility.IDisplayItem)artifactPanel.DataContext;
            var doSomething = await displayItem.DoubleTap();

            if (doSomething)
            {
                // if doSomething in this context is true, show the Category on the page
                await FetchCategoryContents(displayItem.Id);
                backbut.Visibility = Visibility.Visible;
            }
        }
        /**********************************************
         * initialize base case view and launch flyout
         ************************************************/
        private void onFindTap(object sender, RoutedEventArgs e)
        {
            onFind(sender, e);
            showFlyout(sender, e);
        }

        private async void onBack(object sender, RoutedEventArgs e)
        {
            backbut.Visibility = Visibility.Collapsed;
            await ResetUi();
        }

    }
}
