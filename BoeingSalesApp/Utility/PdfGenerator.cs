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
        public async Task gen(String survey_rating, String survey_comment, String survey_contact)
        {
            string path = "ExportPDF.pdf";

            var fileStore = new Utility.FileStore();
            
            Windows.Storage.StorageFolder folder = await fileStore.GetArtifactFolder();
            Windows.Storage.StorageFile file = null;

            bool fileExists = false;

            try
            {
                file = await folder.GetFileAsync(path);
                fileExists = true;
            }
            catch (System.IO.FileNotFoundException)
            {
            }
            if (!fileExists)
                file = await folder.CreateFileAsync(path);

            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);

            Document document = new Document(PageSize.A4);

            // Create an instance to the PDF file by creating an instance of the PDF 
            // Writer class using the document and the filestrem in the constructor.
            PdfWriter writer = PdfWriter.GetInstance(document, stream.AsStream());

    
            // Open the document to enable you to write to the document
            document.Open();

            // Create Boeing Logo
            var _folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            String path_to_logo = (await _folder.GetFileAsync("BoeingLogo.scale-100.png")).Path;

            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(path_to_logo);
            logo.Alignment = iTextSharp.text.Element.ALIGN_LEFT;//iTextSharp.text.Element.ALIGN_CENTER;
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
            Phrase header_beginTime = new Phrase("Start Time", font);
            Phrase header_endTime = new Phrase("End Time", font);
            Phrase header_duration = new Phrase("Duration", font);
            Phrase header_contact = new Phrase("Primary Contact", font);
            Phrase header_rating = new Phrase("Event Rating", font);
            Phrase header_assess = new Phrase("Overall Assessment", font);
            Phrase header_comment = new Phrase("Comments", font);
            Phrase header_takeaways = new Phrase("Key Takeaways", font);
            Phrase header_actionItems = new Phrase("Action Items", font);

            // Content of cell
            Phrase loc = new Phrase("St.Louis, MO", font);
            Phrase beginTime = new Phrase("5/5/15 10:03 AM CT", font);
            Phrase endTime = new Phrase("5/5/15 11:00 AM CT", font);
            Phrase duration = new Phrase("57 mins", font);
            Phrase contact = new Phrase(survey_contact, font);
            Phrase meetingRating = new Phrase(survey_rating, font);
            Phrase assess = new Phrase("--", font);
            Phrase comment = new Phrase(survey_comment, font);
            Phrase takeaways = new Phrase("--", font);
            Phrase actionItems = new Phrase("--", font);

            // Create table with two cols
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
            table.AddCell(meetingRating);
            table.AddCell(header_assess);
            table.AddCell(assess);
            table.AddCell(header_comment);
            table.AddCell(comment);
            table.AddCell(header_takeaways);
            table.AddCell(takeaways);
            table.AddCell(header_actionItems);
            table.AddCell(actionItems);

            // Add element to document
            document.Add(logo);
            document.Add(header);
            document.Add(table);

            // Close the document
            document.Close();

            // Close the writer instance
            writer.Close();
        }
    }
}
