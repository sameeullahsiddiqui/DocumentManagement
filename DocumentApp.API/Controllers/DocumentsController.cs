using AutoMapper;
using DocumentApp.API.Results;
using DocumentApp.Core.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DocumentManagement.API.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/Documents")]
    public class DocumentsController : ApiController
    {
        private readonly IDocumentTypeService _documentTypeService;

        public DocumentsController(IDocumentTypeService documentTypeService)
        {
            _documentTypeService = documentTypeService;
        }


        [HttpPost]
        [Route("{fileAllocationId}")]
        public async Task<IHttpActionResult> UploadFile(Guid fileAllocationId)
        {
            var httpRequest = HttpContext.Current.Request;
            string fileName = "";
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var ext = Path.GetExtension(postedFile.FileName);

                    fileName = fileAllocationId + ext;
                    var filePath = HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + fileName);

                    postedFile.SaveAs(filePath);
                }

                var documentType = await _documentTypeService.GetByIdAsync(fileAllocationId);

                if (documentType != null)
                {
                    documentType.FileName = fileName;

                    _documentTypeService.Update(documentType);
                    await _documentTypeService.CommitAsync();
                }
            }
            else
            {
                return BadRequest("Please attach file.");
            }

            return Ok(fileName);
        }

        [HttpGet]
        public IHttpActionResult Get([FromUri]string fileName)
        {
            var strFileUrl = HttpContext.Current.Server.MapPath("~/UploadedDocuments/" + fileName);
            return new FileResult(strFileUrl);
        }

        private bool DocumentTypeExists(Guid id)
        {
            return _documentTypeService.GetByIdAsync(id) != null;
        }
    }

}
