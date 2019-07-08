using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using DocumentManagement.Core.Models;
using DocumentManagement.Infrastructure.Interfaces;
using DocumentManagement.API.ViewModels;
using log4net;
using Newtonsoft.Json;

namespace DocumentManagement.API.Controllers
{

    [RoutePrefix("api/DocumentTypes")]
    public class DocumentTypesController : ApiController
    {
        private readonly IDocumentTypeRepository _documentsTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DocumentTypesController(IUnitOfWork unitOfWork, IDocumentTypeRepository documentsTypeRepository)
        {
            _documentsTypeRepository = documentsTypeRepository;
            _unitOfWork = unitOfWork;
        }

        // GET: api/DocumentsTypes
        public async Task<IHttpActionResult> GetDocumentsTypesAsync([FromUri] DocumentsTypePagingViewModel pagingmodel)
        {
            var source = await Task.FromResult(_documentsTypeRepository.GetAll());

            if (!string.IsNullOrEmpty(pagingmodel.DocumentType))
            {
                source = source.Where(a => a.DocumentType == pagingmodel.DocumentType);
            }

            int count = source.Count();
            int CurrentPage = pagingmodel.pageNumber;
            int PageSize = pagingmodel.pageSize;
            int TotalCount = count;
            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            var items = source.OrderBy(x => x.DocumentType).Skip((CurrentPage - 1) * PageSize).Take(PageSize).Select(x => new {
                Id = x.Id,
                DocumentType = x.DocumentType,
                CreatedDate = x.CreatedDate
            }).ToList();

            var previousPage = CurrentPage > 1 ? "Yes" : "No";
            var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

            // Object which we are going to send in header   
            var paginationMetadata = new
            {
                totalCount = TotalCount,
                pageSize = PageSize,
                currentPage = CurrentPage,
                totalPages = TotalPages,
                previousPage,
                nextPage
            };

            if (HttpContext.Current != null)
                HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
            return Ok(items);
        }

        // GET: api/DocumentsTypes/5
        [ResponseType(typeof(DocumentsType))]
        public async Task<IHttpActionResult> GetDocumentsTypeAsync(Guid id)
        {
            var familyDocumentsType = await _documentsTypeRepository.GetByIdAsync(id);
            if (familyDocumentsType == null)
            {
                return NotFound();
            }

            return Ok(familyDocumentsType);
        }

        // PUT: api/DocumentsTypes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDocumentsTypeAsync(Guid id, DocumentsType documentsType)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != documentsType.Id)
                return BadRequest();

            try
            {
                _documentsTypeRepository.Edit(documentsType);
                await _unitOfWork.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await DocumentsTypeExistsAsync(id))
                    return NotFound();
                else
                    throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/DocumentsTypes
        [ResponseType(typeof(DocumentsType))]
        public async Task<IHttpActionResult> PostDocumentsTypeAsync(DocumentsType documentsType)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _documentsTypeRepository.Add(documentsType);
                await _unitOfWork.CommitAsync();
            }
            catch (DbUpdateException)
            {
                if (await DocumentsTypeExistsAsync(documentsType.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = documentsType.Id }, documentsType);
        }

        // DELETE: api/DocumentsTypes/5
        [ResponseType(typeof(DocumentsType))]
        public async Task<IHttpActionResult> DeleteDocumentsTypeAsync(Guid id)
        {
            var familyDocumentsType = await _documentsTypeRepository.GetByIdAsync(id);
            if (familyDocumentsType == null)
            {
                return NotFound();
            }

            _documentsTypeRepository.Delete(familyDocumentsType);
            await _unitOfWork.CommitAsync();

            return Ok(familyDocumentsType);
        }

        private async Task<bool> DocumentsTypeExistsAsync(Guid id)
        {
            return await _documentsTypeRepository.GetByIdAsync(id) != null;
        }
    }
}