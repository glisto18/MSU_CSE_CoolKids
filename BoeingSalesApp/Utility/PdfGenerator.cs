using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BoeingSalesApp.Utility
{
    class PdfGenerator
    {
        BaseColor _BoeingBlue = new BaseColor(1, 84, 160);

        //
        // Vertical headers for the table
        //
        List<String> _HEADERS = new List<String>(new String[] {
       "Location", "Start Time", "End Time", "Duration", "Primary Contact", "Event Rating",
        "Overall Assessment", "Comments", "Key Takeaways", "Action Items"
        });

        //
        // Content to write to pdf
        //
        List<String> _CONTENT = new List<String>();

        //
        // Boeing logo to draw in the pdf
        //
        Image _BoeingLogo = null;

        //
        // Path to pdf file
        //
        string _Path = "ExportPDF.pdf";

        //
        // File object used to open file stream 
        //
        Windows.Storage.StorageFile _File = null;


        public PdfGenerator(String survey_rating, String survey_comment, String survey_contact)
        {
            this.logo();
            
            // move args from gen to here, then set _CONTENT
            _CONTENT.Add("St.Louis, MO");
            _CONTENT.Add("5/5/15 10:03 AM CT");
            _CONTENT.Add("5/5/15 11:00 AM CT");
            _CONTENT.Add("57 mins");
            _CONTENT.Add(survey_contact);
            _CONTENT.Add(survey_rating);
            _CONTENT.Add("--");
            _CONTENT.Add(survey_comment);
            _CONTENT.Add("--");
            _CONTENT.Add("--");

       
            // Generate pathname from meeting time and date (will need to pass as args)
        }


        // Get PDF file if it exists, else create one
        private async Task file()
        {
            var fileStore = new Utility.FileStore();

            Windows.Storage.StorageFolder folder = await fileStore.GetArtifactFolder();

            bool fileExists = false;

            try
            {
                _File = await folder.GetFileAsync(_Path);
                fileExists = true;
            }
            catch (System.IO.FileNotFoundException)
            {
            }
            if (!fileExists)
                _File = await folder.CreateFileAsync(_Path);

            return;
        }

        // Get Boeing Logo from PNG in "Assets" folder
        private async void logo()
        {
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            String path_to_logo = (await folder.GetFileAsync("BoeingLogo.scale-100.png")).Path;

            _BoeingLogo = iTextSharp.text.Image.GetInstance(path_to_logo);
            _BoeingLogo.Alignment = Element.ALIGN_LEFT;
            _BoeingLogo.ScalePercent(2f);
        }

        public async Task gen()
        {
            await this.file();

            var stream = await _File.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);

            Document document = new Document(PageSize.A4);

            // Create an instance to the PDF file by creating an instance of the PDF 
            // Writer class using the document and the filestrem in the constructor.
            PdfWriter writer = PdfWriter.GetInstance(document, stream.AsStream());

            // Open the document to enable you to write to the document
            document.Open();

            // Create document header
            Font headerFont = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 26, Font.BOLD, BaseColor.WHITE);
            Phrase headerContent = new Phrase("Meeting Summary", headerFont);
            PdfPTable docHeader = new PdfPTable(1);
            docHeader.SpacingBefore = 15.0f;
            docHeader.DefaultCell.Border = Rectangle.NO_BORDER;
            PdfPCell cell = new PdfPCell(headerContent);
            cell.FixedHeight = 35f;
            cell.BackgroundColor = _BoeingBlue;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.PaddingBottom = 5f;
            docHeader.AddCell(cell);
            
           
            // Create table with two columns
            PdfPTable table = new PdfPTable(2); 
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            table.SpacingBefore = 20.0f;
            int[] columnWidths = new int[] { 20, 60 };
            table.SetWidths(columnWidths);

            this.populateTable(ref table);

            // Add element to document
            document.Add(_BoeingLogo);
            document.Add(docHeader);
            document.Add(table);

            // Close the document
            document.Close();

            // Close the writer instance
            writer.Close();
        }

        
        private void populateTable(ref PdfPTable table)
        {
            for (int i = 0; i < _HEADERS.Count; i++)
            {
                Phrase header = new Phrase(_HEADERS[i]);
                Phrase content = new Phrase(_CONTENT[i]);

                table.AddCell(header);
                table.AddCell(content);
            }
        }
    }
}
