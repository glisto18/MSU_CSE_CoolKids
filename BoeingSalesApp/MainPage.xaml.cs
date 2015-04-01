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

        private async void genpdf(object sender, TappedRoutedEventArgs e)
        {

            string path = "ExportPDF.pdf";
           

            var fileStore = new Utility.FileStore();
            //C:\Users\Team Boeing\Downloads\BoeingArtifactFolder
            Windows.Storage.StorageFolder folder = await fileStore.GetArtifactFolder();

            Windows.Storage.StorageFile file = null;
            try
            {
                file = await folder.CreateFileAsync(path, Windows.Storage.CreationCollisionOption.ReplaceExisting);
            }
            catch
            {
            }

            /*if (file != null)
            {
                await Windows.Storage.FileIO.WriteTextAsync(file, string.Empty);
            }*/

            // Open to PDF file for read/write.
            Windows.Storage.StorageFile samplefile = await folder.GetFileAsync(path);
            var stream = await samplefile.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);

            // Create an instance of the document class which represents the PDF document itself.
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);

            // Create an instance to the PDF file by creating an instance of the PDF 
            // Writer class using the document and the filestrem in the constructor.
            PdfWriter writer = PdfWriter.GetInstance(document, stream.AsStream());

            // Add meta information to the document
            document.AddSubject("Subject - Exit Survey");
            document.AddTitle("Title - Exit Survey");

            // Open the document to enable you to write to the document
            document.Open();

            // Create Boeing Logo
            var _folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            String path_to_logo = (await _folder.GetFileAsync("BoeingLogo.scale-100.png")).Path;
            
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(path_to_logo);
            logo.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            logo.ScalePercent(5f);

            // Create document header
            Font headerFont = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 26, Font.NORMAL, new BaseColor(0, 0, 0));
            Chunk headerChunk = new Chunk("Exit Survey Summary", headerFont);
            headerChunk.SetUnderline(0.5f, -1.5f);
            Paragraph header = new Paragraph();
            header.Add(headerChunk);
            header.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            header.SpacingBefore = 15.0f;

            Font font = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, Font.NORMAL, new BaseColor(0, 0, 0));
            // Table vertical headers
            Phrase header_loc = new Phrase("Location", font);
            Phrase loc = new Phrase("Detroit, MI", font);
            Phrase header_beginTime = new Phrase("Start Time", font);
            Phrase beginTime = new Phrase("5/5/15 10:03 AM CT", font);
            Phrase header_endTime = new Phrase("End Time", font);
            Phrase endTime = new Phrase("5/5/15 11:00 AM CT", font);
            Phrase header_duration = new Phrase("Duration", font);
            Phrase duration = new Phrase("57 mins", font);
            Phrase header_contact = new Phrase("Primary Contact", font);
            Phrase contact = new Phrase("John Doe", font);

            Phrase header_rating = new Phrase("Event Rating", font);
            Phrase rating = new Phrase("--", font);
            Phrase header_assess = new Phrase("Overall Assessment", font);
            Phrase assess = new Phrase("--", font);

            Phrase header_comment = new Phrase("Comments", font);
            string text = "Customer would like further information regarding Advanced Acoustics Services for anti-submiarine warfare.";
            Phrase comment = new Phrase(text, font);

            PdfPTable table = new PdfPTable(2);
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            table.SpacingBefore = 20.0f;
            int[] columnWidths = new int[] { 20, 60 };
            table.SetWidths(columnWidths);

            // Cells are added to table from left to right and top to bottom
            table.AddCell(header_loc);
            table.AddCell(loc);
            table.AddCell(header_beginTime);
            table.AddCell(beginTime);
            table.AddCell(header_endTime);
            table.AddCell(endTime);
            table.AddCell(header_duration);
            table.AddCell(duration);
            table.AddCell(header_contact);
            table.AddCell(contact);
            table.AddCell(header_rating);
            table.AddCell(rating);
            table.AddCell(header_assess);
            table.AddCell(assess);
            table.AddCell(header_comment);
            table.AddCell(comment);

            // Add element to document
            document.Add(logo);
            document.Add(header);
            document.Add(table);

            // Close the document
            document.Close();
            // Close the writer instance
            writer.Close();
        }

        /*
        private void theWindowMaker(object sender, RoutedEventArgs e)
        {
           Core.CoreApplicationView newView = Core.CoreApplication.CreateNewView();
        }*/
    }
}
