using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Diagnostics;
using System.IO;
using System.Web.Mvc;

namespace MVC_PDF.Controllers
{
    public class PDFController : Controller
    {
        //These method will make the user to see the content in browser
        public FileStreamResult GetPDF()
        {
            string path = Server.MapPath(@"MVC Interview.pdf");
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            return File(fs, "application/pdf");
        }
        //These method will make the user to see the content in browser
        public FileResult OpenPDF()
        {
            string s =Server.MapPath("~");
            return File(Server.MapPath(@"MVC Interview.pdf"), "application/pdf");
        }
        //These method will make the user to DOWNLOAD the content in browser
        public FileResult DownloadPDF()

        {
            //C:\Users\AGSPL95\Source\repos\MVC_PDF\MVC_PDF\pdf\MVC Interview.pdf
            string filepath = Server.MapPath(@"MVC Interview.pdf");
            byte[] pdfByte = GetBytesFromFile(filepath);
            return File(pdfByte, "application/pdf", "test.pdf");
        }


        public byte[] GetBytesFromFile(string fullFilePath)
        {
            // this method is limited to 2^32 byte files (4.2 GB)
            FileStream fs = null;
            try
            {
                fs = System.IO.File.OpenRead(fullFilePath);
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                return bytes;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }

        public ActionResult PreparePDF()
        {
            FileStreamResult file = null;
            using (MemoryStream stream = new MemoryStream())
            {
                PdfDocument document = new PdfDocument();
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Verdana", 20, XFontStyle.Bold);
                gfx.DrawString("Hello, World!", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormat.Center);
                document.Save(stream, false);
                file= File(stream, "application/pdf", "HelloWorld.pdf");
            }
            return file;
        }
    }
}