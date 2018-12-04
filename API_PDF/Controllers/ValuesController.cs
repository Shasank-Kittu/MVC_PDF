using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Hosting;
using System.Web.Http;

namespace API_PDF.Controllers
{
    //HostingEnvironent will get application path in web api
    public class ValuesController : ApiController
    {
        [HttpGet]
        [Route("Ebook/Getpdf/")]
        //These method will make the pdf to download on the user system
        public HttpResponseMessage Getbook()
        {
            string reqBook = HostingEnvironment.MapPath(@"/pdf/MVC Interview.pdf");
            var dataBytes = File.OpenRead(reqBook);
           
            HttpRequestMessage httpRequestMessage= Request;
            HttpResponseMessage httpResponseMessage=null;
            httpResponseMessage = httpRequestMessage.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(dataBytes);
             httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            //Name of the file while dowmloaing and used for serilization of content
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue(System.Net.Mime.DispositionTypeNames.Inline)
            {
                FileName = "file.pdf"
            };
            // below httpactionresult will make the pdf to download 
            return httpResponseMessage;
        }
        [HttpGet]
        [Route("Ebook/OpenPDF/")]
        //openpdf will make the user to open the pdf file in browser.
        public HttpResponseMessage OpenPDF()
        {
            string path = HostingEnvironment.MapPath(@"/pdf/MVC Interview.pdf");
           
           var buffer = File.OpenRead(path);
            //content length for use in header
            var contentLength = buffer.Length;
            //200
            //successful
            var statuscode = HttpStatusCode.OK;
           var response = Request.CreateResponse(statuscode);
            response.Content = new StreamContent((buffer));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/Pdf");
            response.Content.Headers.ContentLength = contentLength;
           
            return response;
        }
    }

    public class eBookResult : IHttpActionResult
    {
        MemoryStream bookStuff;
        string PdfFileName;
        HttpRequestMessage httpRequestMessage;
        HttpResponseMessage httpResponseMessage;
        public eBookResult(MemoryStream data, HttpRequestMessage request, string filename)
        {
            bookStuff = data;
            httpRequestMessage = request;
            PdfFileName = filename;
        }
        public System.Threading.Tasks.Task<HttpResponseMessage> ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            httpResponseMessage = httpRequestMessage.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(bookStuff);
            //httpResponseMessage.Content = new ByteArrayContent(bookStuff.ToArray());  
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = PdfFileName;
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

            return System.Threading.Tasks.Task.FromResult(httpResponseMessage);
        }
    }
}
