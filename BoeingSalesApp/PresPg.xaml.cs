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

namespace BoeingSalesApp
{
    public sealed partial class PresPg : Page
    {
        private DataAccess.Repository.SalesBag_ArtifactRepository _asalesbagRepo;
        private DataAccess.Repository.SalesBagRepository _salesbagRepo;
        private DataAccess.Repository.MeetingRepository _meetingRepo;
        private DataAccess.Entities.Meeting launchmeet;
        public PresPg()
        {
            this.InitializeComponent();
            _asalesbagRepo = new DataAccess.Repository.SalesBag_ArtifactRepository();
            _salesbagRepo = new DataAccess.Repository.SalesBagRepository();
            _meetingRepo = new DataAccess.Repository.MeetingRepository();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e) 
        {
            //DataAccess.Entities.SalesBag salesbagChosen = (DataAccess.Entities.SalesBag)e.Parameter;
            launchmeet = (DataAccess.Entities.Meeting)e.Parameter;
            try 
            {
                DataAccess.Entities.SalesBag salesbagChosen = await _salesbagRepo.Get(launchmeet.SalesBag);
                List<DataAccess.Entities.Artifact> sa = await _asalesbagRepo.GetAllSalesBagArtifacts(salesbagChosen); 
                ArtView.ItemsSource = sa;
            }
            catch (NullReferenceException) { }
        }
        /*****************************************************************************
         * If current meeting has no note associated: asks user to enter note name
         * Opens file (associated with meeting) for writing to
         ***************************************************************************/
        private async void addNote(object sender, RoutedEventArgs e)
        {
            if (launchmeet.Note == null || launchmeet.Note == "")
            {
                showFlyout(sender, e);
                var newMeeting = launchmeet;
                newMeeting.Note = nt.Text;
                launchmeet.Note = nt.Text;
                await _meetingRepo.DeleteAsync(launchmeet);
                await _meetingRepo.SaveAsync(newMeeting);
            }
            else
                noteE.ShowAt(ArtPanel);
        }
        /**************************************************************
         * Adds lines from textblock to file connected to meeting
         *****************************************************************/
        private async void addLines(object sender, RoutedEventArgs e)
        {
            if (launchmeet.Note == null)
                return;
            string filename = launchmeet.Note + ".txt";
            var notefile = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(filename);
            string toadd = lines.Text + "\n";
            await Windows.Storage.FileIO.AppendTextAsync(notefile, toadd);
            noteE.Hide();
        }
        /****************************************************************
         * Analytics done here
         * End meeting returns to meetings page
         *****************************************************************/
        private void doneMeet(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(TESTMeetingsView));
        }
        private void showFlyout(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }
        private async void closeFlyout(object sender, RoutedEventArgs e)
        {
            noteT.Hide();
            string andext = nt.Text + ".txt";
            jonny.Text = andext;
            try { await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync(andext, Windows.Storage.CreationCollisionOption.FailIfExists); }
            catch { }
            showFlyout(sender, e);
        }
    }
}
