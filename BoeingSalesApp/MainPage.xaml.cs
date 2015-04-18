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
using Windows.UI.Xaml.Media.Imaging;
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
        private SolidColorBrush white = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 255, 255));
        private SolidColorBrush blue = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 93, 171));

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void artifactsPointed(object sender, RoutedEventArgs e)
        {
            //theWindowMaker(sender, e);
            uxArtifactsImageAfter.Visibility = Visibility.Visible;
            uxArtifactsImageBefore.Visibility = Visibility.Collapsed;

            ArtifactsButton.Background = white;
            ArtifactsLabel.Foreground = blue;
        }

        private void artifactsNotPointed(object sender, RoutedEventArgs e)
        {
            //theWindowMaker(sender, e);
            uxArtifactsImageAfter.Visibility = Visibility.Collapsed;
            uxArtifactsImageBefore.Visibility = Visibility.Visible;

            ArtifactsButton.Background = blue;
            ArtifactsLabel.Foreground = white;
        }

        private void meetingsPointed(object sender, RoutedEventArgs e)
        {
            //theWindowMaker(sender, e);
            uxMeetingImageAfter.Visibility = Visibility.Visible;
            uxMeetingImageBefore.Visibility = Visibility.Collapsed;

            MeetingsButton.Background = white;
            MeetingsLabel.Foreground = blue;
        }

        private void meetingsNotPointed(object sender, RoutedEventArgs e)
        {
            //theWindowMaker(sender, e);
            uxMeetingImageAfter.Visibility = Visibility.Collapsed;
            uxMeetingImageBefore.Visibility = Visibility.Visible;

            MeetingsButton.Background = blue;
            MeetingsLabel.Foreground = white;

        }

        private void bagsPointed(object sender, RoutedEventArgs e)
        {
            //theWindowMaker(sender, e);
            uxBagImageAfter.Visibility = Visibility.Visible;
            uxBagImageBefore.Visibility = Visibility.Collapsed;

            BagsButton.Background = white;
            BagsLabel.Foreground = blue;
        }

        private void bagsNotPointed(object sender, RoutedEventArgs e)
        {
            //theWindowMaker(sender, e);
            uxBagImageAfter.Visibility = Visibility.Collapsed;
            uxBagImageBefore.Visibility = Visibility.Visible;

            BagsButton.Background = blue;
            BagsLabel.Foreground = white;
        }

        private void onMeetings(object sender, RoutedEventArgs e)
        {
            //theWindowMaker(sender, e);

            this.Frame.Navigate(typeof(TESTMeetingsView));
        }

        private void onArtifacts(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(ArtifactsView));
            this.Frame.Navigate(typeof(NewArtifactsView));
            ;

        }

        private void onSalesBags(object sender, RoutedEventArgs e)
        {
            object x = 5;
            this.Frame.Navigate(typeof(NewArtifactsView), x);
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


        private void goToSurvey(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SurveyView));
        }


        /*
        private void theWindowMaker(object sender, RoutedEventArgs e)
        {
           Core.CoreApplicationView newView = Core.CoreApplication.CreateNewView();
           int viewID = 0;
           await newView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
           {
               var frame = new Frame();
               frame.Navigate(typeof(TESTMeetingsView), null);
               Window.Current.Content = frame;
               viewID = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().Id;
           });
           bool viewShown = await Windows.UI.ViewManagement.ApplicationViewSwitcher.TryShowAsStandaloneAsync(viewID);
        }
         * */
    }
}
