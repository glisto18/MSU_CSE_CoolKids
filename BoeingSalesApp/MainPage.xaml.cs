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
using SQLite;
using System.Threading.Tasks;
using Core = Windows.ApplicationModel.Core;


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
        

        private void onMeetings(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(TESTMeetingsView));
        }

        private void onArtifacts(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(ArtifactsView));
            this.Frame.Navigate(typeof(NewArtifactsView));
        }

        private void onSalesBags(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SalesBagsView));
        }

        private void DBTestLink_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DBTest));
        }

        private void SalesBagLink_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SalesBagsView));
        }
        /*
        private void theWindowMaker(object sender, RoutedEventArgs e)
        {
           Core.CoreApplicationView newView = Core.CoreApplication.CreateNewView();
        }*/

        private async void genpdf(object sender, TappedRoutedEventArgs e)
        {
            var generator = new Utility.PdfGenerator();
            await generator.gen();
        }

        /*
        private void theWindowMaker(object sender, RoutedEventArgs e)
        {
           Core.CoreApplicationView newView = Core.CoreApplication.CreateNewView();
        }*/
    }
}
