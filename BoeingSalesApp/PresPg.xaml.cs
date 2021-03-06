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
        public PresPg()
        {
            this.InitializeComponent();
            _asalesbagRepo = new DataAccess.Repository.SalesBag_ArtifactRepository();
            _salesbagRepo = new DataAccess.Repository.SalesBagRepository();
            _meetingRepo = new DataAccess.Repository.MeetingRepository();
            _artRepo = new DataAccess.Repository.ArtifactRepository();
            _catRepo = new DataAccess.Repository.CategoryRepository();
        }
        /********************************************************************
         * Note field is the meeting unique ID + ".txt"
         * Should display all artifacts from salesbag
         *********************************************************************/
        protected async override void OnNavigatedTo(NavigationEventArgs e) 
        {
            launchmeet = (DataAccess.Entities.Meeting)e.Parameter;
            launchmeet.Note = launchmeet.ID.ToString() + ".txt";
            try 
            {
                DataAccess.Entities.SalesBag salesbagChosen = await _salesbagRepo.Get(launchmeet.SalesBag);
                List<DataAccess.Entities.Artifact> sa = await _asalesbagRepo.GetAllSalesBagArtifacts(salesbagChosen); 
                ArtView.ItemsSource = sa;
            }
            catch (NullReferenceException) { }
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
            this.Frame.Navigate(typeof(MeetingsView));
        }
        private void showFlyout(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }
        
        private async void onFind(object sender, RoutedEventArgs e)
        {
            //botBar.Visibility = Visibility.Collapsed;
            var allarts = await _artRepo.GetAllAsync();
            ArtView.ItemsSource = allarts;
            var fewarts = Utility.DisplayConverter.ToDisplayArtifacts(await _artRepo.Search(magicmaker.Text));
            var fewcat = Utility.DisplayConverter.ToDisplayCategories(await _catRepo.Search(magicmaker.Text));
            var dispitems = new List<Utility.IDisplayItem>();
            dispitems.AddRange(fewarts);
            dispitems.AddRange(fewcat);
            ArtView.ItemsSource = dispitems;
        }
    }
}
