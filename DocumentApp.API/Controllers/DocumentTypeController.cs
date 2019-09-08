using AutoMapper;
using DocumentApp.API.Models;
using DocumentApp.Core.Entities;
using DocumentApp.Core.Services;
using DocumentApp.Dto.Dtos;
using Newtonsoft.Json;
using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace DocumentApp.API.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    [RoutePrefix("api/documenttypes")]
    public class DocumentTypeController : ApiController
    {
        private readonly IDocumentTypeService _documentTypeService;
        private readonly IMapper _mapper;

        public DocumentTypeController(IDocumentTypeService documentTypeService, IMapper mapper)
        {
            _documentTypeService = documentTypeService;
            _mapper = mapper;
        }

        [Route]
        [HttpGet]
        public IHttpActionResult GetDocumentTypes([FromUri] DocumentsTypePagingViewModel pagingmodel)
        {
            IQueryable<DocumentType> source = _documentTypeService.GetAllQuerableAsync();

            int currentPage = 1;
            int pageSize = 5;
            int count = 0;

            if (pagingmodel != null && pagingmodel.PageSize > 0)
            {
                if (!string.IsNullOrEmpty(pagingmodel.DocumentTypeName))
                {
                    source = source.Where(a => a.DocumentTypeName == pagingmodel.DocumentTypeName);
                }

                currentPage = pagingmodel.PageNumber;
                pageSize = pagingmodel.PageSize == -1 ? source.Count() : pagingmodel.PageSize;
                count = source.Count();
            }
            else
            {
                pageSize = source.Count();
                count = pageSize;
            }

            int totalCount = count;
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            var previousPage = currentPage > 1 ? "Yes" : "No";
            var nextPage = currentPage < totalPages ? "Yes" : "No";
            var paginationMetadata = new { totalCount, pageSize, currentPage, totalPages, previousPage, nextPage };

            var documentTypeDtos = source.OrderBy(x => x.DocumentTypeName).Skip((currentPage - 1) * pageSize).Take(pageSize)
                             .Select(x => new DocumentTypeDto
                             {
                                 Id = x.Id,
                                 CreatedBy=x.CreatedBy,
                                 CreatedDate=x.CreatedDate,
                                 DocumentType = x.DocumentTypeName,
                                 Remark = x.Remark,
                                 Description = x.Description,
                             }).ToList();


            if (HttpContext.Current != null)
                HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));

            return Ok(documentTypeDtos);
        }

        [Route("{id:Guid}")]
        public IHttpActionResult GetDocumentType(Guid id)
        {
            var documentType = _documentTypeService.GetByIdQuerableAsync(id).FirstOrDefault();
            if (documentType == null)
            {
                return NotFound();
            }

            //DocumentTypeDto documentTypeDto = _mapper.Map<DocumentType, DocumentTypeDto>(documentType);
            DocumentTypeDto documentTypeDto = new DocumentTypeDto
            {
                Id = documentType.Id,
                CreatedBy = documentType.CreatedBy,
                CreatedDate = documentType.CreatedDate,
                DocumentType = documentType.DocumentTypeName,
                Remark = documentType.Remark,
                Description = documentType.Description,
            };


            return Ok(documentTypeDto);
        }

        [Route("{id:Guid}")]
        [HttpPut]
        public async Task<IHttpActionResult> PutDocumentType(Guid id, DocumentTypeDto documentTypeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != documentTypeDto.Id)
                return BadRequest();

            try
            {
                var documentType = await _documentTypeService.GetByIdAsync(id);

                if (documentType != null)
                {

                    documentType.DocumentTypeName = documentTypeDto.DocumentType;
                    documentType.Remark = documentTypeDto.Remark;
                    documentType.Description = documentTypeDto.Description;


                    _documentTypeService.Update(documentType);
                    await _documentTypeService.CommitAsync();

                    return Ok(documentTypeDto);
                }
                else
                    return StatusCode(HttpStatusCode.NoContent);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }


        [Route]
        [HttpPost]
        public async Task<IHttpActionResult> PostDocumentType(DocumentTypeDto documentTypeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //DocumentType documentType = _mapper.Map<DocumentTypeDto, DocumentType>(documentTypeDto);

            var documentType = new DocumentType
            {
                DocumentTypeName = documentTypeDto.DocumentType,
                Remark = documentTypeDto.Remark,
                Description = documentTypeDto.Description,
            };


            documentType = _documentTypeService.Add(documentType);
            await _documentTypeService.CommitAsync();

            //documentTypeDto = _mapper.Map(documentType, documentTypeDto);

            documentTypeDto = new DocumentTypeDto
            {
                Id = documentType.Id,
                CreatedBy = documentType.CreatedBy,
                CreatedDate = documentType.CreatedDate,
                DocumentType = documentType.DocumentTypeName,
                Remark = documentType.Remark,
                Description = documentType.Description,
            };

            return Ok(documentTypeDto);
        }

        [Route("{id:Guid}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteDocumentType(Guid id)
        {
            var documentType = await _documentTypeService.GetByIdAsync(id);
            if (documentType == null)
            {
                return NotFound();
            }

            _documentTypeService.Delete(documentType);
            await _documentTypeService.CommitAsync();

            var documentTypeDto = _mapper.Map<DocumentType, DocumentTypeDto>(documentType);
            return Ok(documentTypeDto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _documentTypeService.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DocumentTypeExists(Guid id)
        {
            return _documentTypeService.GetByIdAsync(id) != null;
        }
    }
}