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
using BoeingSalesApp.DataAccess.Repository;
using BoeingSalesApp.DataAccess.Entities;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BoeingSalesApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SalesBagsView : Page
    {

        private SalesBagRepository _salesBagRepository;

        public SalesBagsView()
        {
            this.InitializeComponent();
            newSalesBagButton.Flyout = myFlyout;

            _salesBagRepository = new SalesBagRepository();
        }

        private void onBack(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private async Task FetchSalesBags()
        {
            var bags = await _salesBagRepository.GetAllAsync();
            SalesBagsGridView.ItemsSource = bags;
        }


        /// <summary>
        /// Called when user clicks "Plus Sign" icon 
        /// </summary>
        private void showFlyout(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement) sender);
        }

        /// <summary>
        /// Called when user clicks "Create" button in the flyout 
        /// </summary>
        private async void onCreate(object sender, RoutedEventArgs e)
        {
            newSalesBagButton.Flyout.Hide();

            SalesBag newBag = new SalesBag();
            newBag.Name = NameInput.Text;
            newBag.DateCreated = DateTime.Now;
            newBag.Active = true;
            await _salesBagRepository.SaveAsync(newBag);

            NameInput.Text = "";
            await FetchSalesBags();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            //navigationHelper.OnNavigatedTo(e);
            await FetchSalesBags();
        }
    }
}
