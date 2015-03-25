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
using Core = Windows.ApplicationModel.Core;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

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

        private void genpdf(object sender, TappedRoutedEventArgs e)
        {

            string filename = "test.txt";
            string textToAdd = "Example text in file";
            
            /*FileStream stream = new FileStream(filename, FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(textToAdd);*/
            
            //String path = "Desktop" + "\\" + "my-first-pdf.pdf";
            
            /*StreamWriter outfile = new StreamWriter(path, true)
           
            
            // Create an instance of the document class which represents the PDF document itself.
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            //iTextSharp.text.Document
            // Create an instance to the PDF file by creating an instance of the PDF 
            // Writer class using the document and the filestream in the constructor.
            PdfWriter writer = PdfWriter.GetInstance(document,outfile);*/
        }

        /*
        private void theWindowMaker(object sender, RoutedEventArgs e)
        {
           Core.CoreApplicationView newView = Core.CoreApplication.CreateNewView();
        }*/
    }
}
