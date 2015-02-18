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
using BoeingSalesApp.DataAccess.Entities;
using BoeingSalesApp.DataAccess.Repository;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BoeingSalesApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SalesBagView : Page
    {
        private SalesBag _newSalesBag;

        private ISalesBagRepository _salesBagRepo;

        public SalesBagView()
        {
            this.InitializeComponent();
            Init();
        }

        private void Init()
        {
            _newSalesBag = new SalesBag();
            _salesBagRepo = new SalesBagRepository();
            uxNewSalesBagPanel.DataContext = _newSalesBag;
            
        }

        private async Task FetchSalesBags()
        {
            var bags = await _salesBagRepo.GetAllAsync();
            uxSalesBagGrid.ItemsSource = bags;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            _newSalesBag.DateCreated = DateTime.Now;
            _newSalesBag.Active = true;
            _salesBagRepo.SaveAsync(_newSalesBag);
            _newSalesBag = new SalesBag();
            uxNewSalesBagPanel.DataContext = _newSalesBag;
            tbNewName.Text = "";
        }


    }
}
