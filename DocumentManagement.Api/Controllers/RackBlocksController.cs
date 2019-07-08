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
using log4net;
using Newtonsoft.Json;
using DocumentManagement.Infrastructure.Interfaces;
using DocumentManagement.API.ViewModels;
using DocumentManagement.Core.Models;

namespace RackBlockMastersManagement.API.Controllers
{
    [RoutePrefix("api/RackBlocks")]
    public class RackBlocksController : ApiController
    {
        private readonly IRackBlockRepository _rackBlockRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RackBlocksController(IUnitOfWork unitOfWork, IRackBlockRepository rackBlockRepository)
        {
            _rackBlockRepository = rackBlockRepository;
            _unitOfWork = unitOfWork;
        }

        // GET: api/RackBlockMaster
        public async Task<IHttpActionResult> GetRackBlockMasterAsync([FromUri] RackPagingViewModel pagingmodel)
        {
            var source = await Task.FromResult(_rackBlockRepository.GetAll());

            if (!string.IsNullOrEmpty(pagingmodel.BlockNumber))
            {
                source = source.Where(a => a.BlockNumber == pagingmodel.BlockNumber);
            }

            if (!string.IsNullOrEmpty(pagingmodel.RackNumber))
            {
                source = source.Where(a => a.Rack.RackNumber == pagingmodel.RackNumber);
            }


            int count = source.Count();
            int CurrentPage = pagingmodel.pageNumber;
            int PageSize = pagingmodel.pageSize;
            int TotalCount = count;
            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            var items = source.OrderBy(x=>x.BlockNumber).Skip((CurrentPage - 1) * PageSize).Take(PageSize).Select(x => new { Id = x.Id, BlockNumber = x.BlockNumber, RackNumber = x.Rack.RackNumber }).ToList();
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

        // GET: api/RackBlockMaster/5
        [ResponseType(typeof(RackBlockMaster))]
        public async Task<IHttpActionResult> GetRackAsync(Guid id)
        {
            var rackBlockMaster = await _rackBlockRepository.GetByIdAsync(id);
            if (rackBlockMaster == null)
            {
                return NotFound();
            }

            return Ok(rackBlockMaster);
        }

        // PUT: api/RackBlockMaster/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRackAsync(Guid id, RackBlockMaster rackBlockMaster)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != rackBlockMaster.Id)
                return BadRequest();

            try
            {
                _rackBlockRepository.Edit(rackBlockMaster);
                await _unitOfWork.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await RackBlockMasterDetailExistsAsync(id))
                    return NotFound();
                else
                    throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/RackBlockMaster
        [ResponseType(typeof(RackBlockMaster))]
        public async Task<IHttpActionResult> PostRackAsync(RackBlockMaster rackBlockMaster)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _rackBlockRepository.Add(rackBlockMaster);
                await _unitOfWork.CommitAsync();
            }
            catch (DbUpdateException)
            {
                if (await RackBlockMasterDetailExistsAsync(rackBlockMaster.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = rackBlockMaster.Id }, rackBlockMaster);
        }

        // DELETE: api/RackBlockMaster/5
        [ResponseType(typeof(RackBlockMaster))]
        public async Task<IHttpActionResult> DeleteRackBlockMasterDetailAsync(Guid id)
        {
            var rackBlockMaster = await _rackBlockRepository.GetByIdAsync(id);
            if (rackBlockMaster == null)
            {
                return NotFound();
            }

            _rackBlockRepository.Delete(rackBlockMaster);
            await _unitOfWork.CommitAsync();

            return Ok(rackBlockMaster);
        }

        private async Task<bool> RackBlockMasterDetailExistsAsync(Guid id)
        {
            return await _rackBlockRepository.GetByIdAsync(id) != null;
        }
    }
}