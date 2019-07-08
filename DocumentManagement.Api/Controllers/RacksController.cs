using System;
using System.Linq;
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

namespace RackMastersManagement.API.Controllers
{
    [RoutePrefix("api/Racks")]
    public class RacksController : ApiController
    {
        private readonly IRackRepository _rackRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RacksController(IUnitOfWork unitOfWork, IRackRepository rackRepository)
        {
            _rackRepository = rackRepository;
            _unitOfWork = unitOfWork;
        }

        // GET: api/RackMaster
        public async Task<IHttpActionResult> GetRackMasterAsync([FromUri] RackPagingViewModel pagingmodel)
        {
            var source = await Task.FromResult(_rackRepository.GetAll());

            if (!string.IsNullOrEmpty(pagingmodel.RackNumber))
            {
                source = source.Where(a => a.RackNumber == pagingmodel.RackNumber);
            }

            int count = source.Count();
            int CurrentPage = pagingmodel.pageNumber;
            int PageSize = pagingmodel.pageSize;
            int TotalCount = count;
            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            var items = source.OrderBy(x=>x.RackNumber).Skip((CurrentPage - 1) * PageSize).Take(PageSize).Select(x => new { x.Id, x.RackNumber,x.Remark,x.CreatedDate,x.CreatedBy }).ToList();
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

            if(HttpContext.Current!=null)
                HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
            return Ok(items);
        }

        // GET: api/RackMaster/5
        [ResponseType(typeof(RackMaster))]
        public async Task<IHttpActionResult> GetRackAsync(Guid id)
        {
            var rackMaster = await _rackRepository.GetByIdAsync(id);
            if (rackMaster == null)
            {
                return NotFound();
            }

            return Ok(rackMaster);
        }

        // PUT: api/RackMaster/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRackAsync(Guid id, RackMaster rackMaster)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != rackMaster.Id)
                return BadRequest();

            try
            {
                _rackRepository.Edit(rackMaster);
                await _unitOfWork.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await RackMasterDetailExistsAsync(id))
                    return NotFound();
                else
                    throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/RackMaster
        [ResponseType(typeof(RackMaster))]
        public async Task<IHttpActionResult> PostRackAsync(RackMaster rackMaster)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _rackRepository.Add(rackMaster);
                await _unitOfWork.CommitAsync();
            }
            catch (DbUpdateException)
            {
                if (await RackMasterDetailExistsAsync(rackMaster.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = rackMaster.Id }, rackMaster);
        }

        // DELETE: api/RackMaster/5
        [ResponseType(typeof(RackMaster))]
        public async Task<IHttpActionResult> DeleteRackMasterDetailAsync(Guid id)
        {
            var rackMaster = await _rackRepository.GetByIdAsync(id);
            if (rackMaster == null)
            {
                return NotFound();
            }

            _rackRepository.Delete(rackMaster);
            await _unitOfWork.CommitAsync();

            return Ok(rackMaster);
        }

        private async Task<bool> RackMasterDetailExistsAsync(Guid id)
        {
            return await _rackRepository.GetByIdAsync(id) != null;
        }
    }
}