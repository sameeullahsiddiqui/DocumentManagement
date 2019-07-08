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
    //[Authorize(Roles = "Admin")]
    [RoutePrefix("api/FileAllocation")]
    public class FileAllocationController : ApiController
    {
        private readonly IFileAllocationRepository _fileAllocationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FileAllocationController(IUnitOfWork unitOfWork, IFileAllocationRepository fileAllocationRepository)
        {
            _fileAllocationRepository = fileAllocationRepository;
            _unitOfWork = unitOfWork;
        }

        // GET: api/FileAllocations
        public async Task<IHttpActionResult> GetFileAllocationsAsync([FromUri] RackPagingViewModel pagingmodel)
        {
            var source = await Task.FromResult(_fileAllocationRepository.GetAll());

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


            int count = source.Count();
            int CurrentPage = pagingmodel.pageNumber;
            int PageSize = pagingmodel.pageSize;
            int TotalCount = count;
            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            var items = source.OrderBy(x => x.FileName).Skip((CurrentPage - 1) * PageSize).Take(PageSize)
            .Select(x => new
            {
                Id = x.Id,
                FileName = x.FileName,
                FolderName = x.FolderName,
                BlockNumber = x.RackBlock.BlockNumber,
                RackNumber = x.RackBlock.Rack.RackNumber,
                DocumentType = x.DocumentType.DocumentType
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

        // GET: api/FileAllocations/5
        [ResponseType(typeof(FileAllocation))]
        public async Task<IHttpActionResult> GetFileAllocationAsync(Guid id)
        {
            var fileAllocation = await _fileAllocationRepository.GetByIdAsync(id);
            if (fileAllocation == null)
            {
                return NotFound();
            }

            return Ok(fileAllocation);
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
                _fileAllocationRepository.Edit(fileAllocation);
                await _unitOfWork.CommitAsync();
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
                _fileAllocationRepository.Add(fileAllocation);
                await _unitOfWork.CommitAsync();
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

            return CreatedAtRoute("DefaultApi", new { id = fileAllocation.Id }, fileAllocation);
        }

        // DELETE: api/FileAllocations/5
        [ResponseType(typeof(FileAllocation))]
        public async Task<IHttpActionResult> DeleteFileAllocationAsync(Guid id)
        {
            var fileAllocation = await _fileAllocationRepository.GetByIdAsync(id);
            if (fileAllocation == null)
            {
                return NotFound();
            }

            _fileAllocationRepository.Delete(fileAllocation);
            await _unitOfWork.CommitAsync();

            return Ok(fileAllocation);
        }

        private async Task<bool> FileAllocationExistsAsync(Guid id)
        {
            return await _fileAllocationRepository.GetByIdAsync(id) != null;
        }
    }
}