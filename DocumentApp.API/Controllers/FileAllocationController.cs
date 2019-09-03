using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using DocumentApp.API.Models;
using DocumentApp.Core.Entities;
using DocumentApp.Core.Data;
using DocumentApp.Core.Services;
using AutoMapper;

namespace DocumentManagement.API.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    [RoutePrefix("api/FileAllocation")]
    public class FileAllocationController : ApiController
    {
        private readonly IFileAllocationService _fileAllocationService;
        private readonly IMapper _mapper;

        public FileAllocationController(IMapper mapper, IFileAllocationService fileAllocationService)
        {
            _fileAllocationService = fileAllocationService;
            _mapper = mapper;
        }

        // GET: api/FileAllocations
        public IHttpActionResult GetFileAllocations([FromUri] RackPagingViewModel pagingmodel)
        {
            IQueryable<FileAllocation> source = _fileAllocationService.GetAllQuerableAsync();

            int currentPage = 1;
            int pageSize = 5;
            int count = 0;

            if (pagingmodel != null && pagingmodel.PageSize > 0)
            {
                if (!string.IsNullOrEmpty(pagingmodel.FileName))
                {
                    source = source.Where(a => a.FileName == pagingmodel.FileName);
                }

                if (!string.IsNullOrEmpty(pagingmodel.FolderName))
                {
                    source = source.Where(a => a.FolderName == pagingmodel.FolderName);
                }

                if (!string.IsNullOrEmpty(pagingmodel.BlockNumber))
                {
                    source = source.Where(a => a.RackBlock.BlockNumber == pagingmodel.BlockNumber);
                }

                if (!string.IsNullOrEmpty(pagingmodel.RackNumber))
                {
                    source = source.Where(a => a.RackBlock.Rack.RackNumber == pagingmodel.RackNumber);
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

            var items = source.OrderBy(x => x.FileName).Skip((currentPage - 1) * pageSize).Take(pageSize)
                              .Select(x => new
                              {
                                  Id = x.Id,
                                  FileName = x.FileName,
                                  FolderName = x.FolderName,
                                  BlockNumber = x.RackBlock.BlockNumber,
                                  RackBlockId = x.RackBlockId,
                                  RackId = x.RackBlock.RackId,
                                  RackNumber = x.RackBlock.Rack.RackNumber,
                                  DocumentTypeId = x.DocumentTypeId,
                                  DocumentType = x.DocumentType.DocumentTypeName,
                                  Remark = x.Remark,
                                  Description = x.Description
                              }).ToList();

            if (HttpContext.Current != null)
                HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));

            return Ok(items);
        }

        // GET: api/FileAllocations/5
        [ResponseType(typeof(FileAllocation))]
        public IHttpActionResult GetFileAllocation(Guid id)
        {
            var fileAllocation = _fileAllocationService.GetByIdQuerableAsync(id).Include(x => x.RackBlock).Include(x => x.RackBlock.Rack).Include(x => x.DocumentType).FirstOrDefault();
            if (fileAllocation == null)
            {
                return NotFound();
            }

            var fileAllocationDto = new
            {
                Id = fileAllocation.Id,
                FileName = fileAllocation.FileName,
                FolderName = fileAllocation.FolderName,
                BlockNumber = fileAllocation.RackBlock.BlockNumber,
                RackBlockId = fileAllocation.RackBlockId,
                RackId = fileAllocation.RackBlock.RackId,
                RackNumber = fileAllocation.RackBlock.Rack.RackNumber,
                DocumentTypeId = fileAllocation.DocumentTypeId,
                DocumentType = fileAllocation.DocumentType.DocumentTypeName,
                Remark = fileAllocation.Remark,
                Description = fileAllocation.Description
            };

            return Ok(fileAllocationDto);
        }

        // PUT: api/FileAllocations/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFileAllocationAsync(Guid id, FileAllocation fileAllocation)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != fileAllocation.Id)
                return BadRequest();

            try
            {
                _fileAllocationService.Update(fileAllocation);
                await _fileAllocationService.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await FileAllocationExistsAsync(id))
                    return NotFound();
                else
                    throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/FileAllocations
        [ResponseType(typeof(FileAllocation))]
        public async Task<IHttpActionResult> PostFileAllocationAsync(FileAllocation fileAllocation)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _fileAllocationService.Add(fileAllocation);
                await _fileAllocationService.CommitAsync();
            }
            catch (DbUpdateException)
            {
                if (await FileAllocationExistsAsync(fileAllocation.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("ApiRoute", new { id = fileAllocation.Id }, fileAllocation);
        }

        // DELETE: api/FileAllocations/5
        [ResponseType(typeof(FileAllocation))]
        public async Task<IHttpActionResult> DeleteFileAllocationAsync(Guid id)
        {
            var fileAllocation = await _fileAllocationService.GetByIdAsync(id);
            if (fileAllocation == null)
            {
                return NotFound();
            }

            _fileAllocationService.Delete(fileAllocation);
            await _fileAllocationService.CommitAsync();

            return Ok(fileAllocation);
        }

        private async Task<bool> FileAllocationExistsAsync(Guid id)
        {
            return await _fileAllocationService.GetByIdAsync(id) != null;
        }
    }
}