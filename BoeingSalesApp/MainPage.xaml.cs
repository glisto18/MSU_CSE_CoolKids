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

            // Create empty PDF file.
            Windows.Storage.StorageFile file = null;
            try
            {
                file = await folder.CreateFileAsync(path, Windows.Storage.CreationCollisionOption.ReplaceExisting);
            }
            catch
            {
            }

            if (file != null)
            {
                await Windows.Storage.FileIO.WriteTextAsync(file, string.Empty);
            }

            // Open to PDF file for read/write.
            Windows.Storage.StorageFile sampleFile = await folder.GetFileAsync(path);
            var stream = await sampleFile.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);

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
            Chunk headerChunk = new Chunk("Exit Survey Summary",
                FontFactory.GetFont(FontFactory.TIMES_ROMAN, 26, Font.NORMAL, new BaseColor(0,0,0)));
            headerChunk.SetUnderline(0.5f, -1.5f);
            Paragraph header = new Paragraph();
            header.Add(headerChunk);
            header.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
            header.SpacingBefore = 15.0f;

            // Meeting duration
            Chunk location = new Chunk("Location: Detroit, MI\n",
                FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, Font.NORMAL, new BaseColor(0, 0, 0)));
            Chunk beginTime = new Chunk("Start Time: 5/5/15 10:03 AM CT\n",
                FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, Font.NORMAL, new BaseColor(0, 0, 0)));
            Chunk endTime = new Chunk("End Time:  5/5/15 11:00 AM CT\n",
                FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, Font.NORMAL, new BaseColor(0, 0, 0)));
            Chunk duration = new Chunk("Duration: 57 mins\n",
                FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, Font.NORMAL, new BaseColor(0, 0, 0)));
            Chunk commentChunk = new Chunk("\nComments:\n",
                FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, Font.NORMAL, new BaseColor(0, 0, 0)));
            Chunk contact = new Chunk("Primary Contact: John Doe\n",
                FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, Font.NORMAL, new BaseColor(0, 0, 0)));
            
            string text = @"Customer would like further information regarding Advanced Acoustics Services 
for anti-submiarine warfare.";
            Chunk comment = new Chunk(text,
                FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, Font.NORMAL, new BaseColor(0, 0, 0)));
            Paragraph commentPara = new Paragraph(comment);
            commentPara.SpacingBefore = 5.0f;

            Chunk rating = new Chunk("Event Rating --\n",
                FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, Font.NORMAL, new BaseColor(0, 0, 0)));
            Chunk assess = new Chunk("Overall Assessment --\n",
                FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, Font.NORMAL, new BaseColor(0, 0, 0)));
            
            Paragraph meetingData = new Paragraph();
            meetingData.SpacingBefore = 15.0f;
            meetingData.Add(location);
            meetingData.Add(beginTime);
            meetingData.Add(endTime);
            meetingData.Add(duration);
            meetingData.Add(contact);
            meetingData.Add(rating);
            meetingData.Add(assess);

            meetingData.Add(commentChunk);
            meetingData.Add(commentPara);

            //PdfPTable table = new PdfPTable(2);

            // Add element to document
            document.Add(logo);
            document.Add(header);
            document.Add(meetingData);

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
