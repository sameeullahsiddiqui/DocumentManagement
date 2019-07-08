using DocumentManagement.API.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DocumentManagement.API.Controllers
{
    [RoutePrefix("api/Documents")]
    public class DocumentsController : ApiController
    {
        [HttpPost]
        [Route("{fileAllocationId}")]
        public HttpResponseMessage UploadFile(Guid fileAllocationId)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var ext = Path.GetExtension(postedFile.FileName);

                    var filePath = HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + fileAllocationId +ext);
                    
                    postedFile.SaveAs(filePath);
                }
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            return response;
        }

        [HttpGet]
        public IHttpActionResult Get([FromUri]string fileName)
        {
            var strFileUrl = HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + fileName);
            return new FileResult(strFileUrl);
        }

        //[HttpGet]
        //[Route("{fileName}")]
        //public HttpResponseMessage Download(string fileName)
        //{
        //    var strFileUrl = HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + fileName);

        //    try
        //    {
        //        if (fileName != "")
        //        {
        //            FileInfo file = new FileInfo(strFileUrl);

        //            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

        //            var stream = new FileStream(file.FullName, FileMode.Open);
        //            response.Content = new StreamContent(stream);
        //            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(file.Extension.ToLower()));
        //            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
        //            {
        //                FileName = file.Name
        //            };
        //            response.Content.Headers.ContentDisposition.FileName = file.Name;
        //            response.Content.Headers.ContentLength = file.Length;
        //            response.Headers.Add("fileName", file.Name);

        //            return response;
        //        }
        //    }
        //    catch (FileNotFoundException)
        //    {
        //        return new HttpResponseMessage(HttpStatusCode.NotFound);
        //    }

        //    return new HttpResponseMessage(HttpStatusCode.NotFound);
        //}


    }

}
