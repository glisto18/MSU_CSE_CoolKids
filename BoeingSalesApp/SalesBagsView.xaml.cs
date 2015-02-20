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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BoeingSalesApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SalesBagsView : Page
    {
        public SalesBagsView()
        {
            this.InitializeComponent();
            newButton.Flyout = zzz;
        }

        private void onBack(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void gridTapped(object sender, RoutedEventArgs e)
        {
            /*FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
            return;*/
        }

        private void onCreateNew(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement) sender);
            
            return;
        }
    }
}
