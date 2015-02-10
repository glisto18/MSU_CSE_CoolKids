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
        

        private void onMeetingsRect_Tapped(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MeetingsView));
        }

        private void onArt_Tap(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ArtifactsCatView));
        }

        private void DBTestLink_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(DBTest));
        }
    }
}
