using DocumentApp.API.Models;
using DocumentApp.Core.Entities;
using DocumentApp.Core.Services;
using DocumentApp.Dto.Dtos;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace RackBlockMastersManagement.API.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/RackBlocks")]
    public class RackBlocksController : ApiController
    {
        private readonly IRackBlockMasterService _rackBlockMasterService;

        public RackBlocksController(IRackBlockMasterService rackBlockMasterService)
        {
            _rackBlockMasterService = rackBlockMasterService;
        }

        // GET: api/RackBlockMaster
        public IHttpActionResult GetRackBlockMaster([FromUri] RackPagingViewModel pagingmodel)
        {
            IQueryable<RackBlockMaster> source = _rackBlockMasterService.GetAllQuerableAsync();

            int currentPage = 1;
            int pageSize = 5;
            int count = 0;
            if (pagingmodel != null && pagingmodel.PageSize > 0)
            {
                currentPage = pagingmodel.pageNumber;
                pageSize = pagingmodel.pageSize;

                if (!string.IsNullOrEmpty(pagingmodel.BlockNumber))
                {
                    source = source.Where(a => a.BlockNumber == pagingmodel.BlockNumber);
                }

                if (!string.IsNullOrEmpty(pagingmodel.RackNumber))
                {
                    source = source.Where(a => a.Rack.RackNumber == pagingmodel.RackNumber);
                }

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

            var items = source.OrderBy(x => x.BlockNumber).Skip((currentPage - 1) * pageSize).Take(pageSize)
                              .Select(x => new RackBlockMasterDto
                              {
                                  Id = x.Id,
                                  BlockNumber = x.BlockNumber,
                                  RackNumber = x.Rack.RackNumber,
                                  Remark = x.Remark,
                                  RackId = x.RackId,
                                  Description = x.Description
                              }).ToList();


            if (HttpContext.Current != null)
                HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
            return Ok(items);

        }

        // GET: api/RackBlockMaster/5
        [ResponseType(typeof(RackBlockMaster))]
        public IHttpActionResult GetRackAsync(Guid id)
        {
            var rackBlockMaster = _rackBlockMasterService.GetByIdQuerableAsync(id).Include(x=>x.Rack).FirstOrDefault();
            if (rackBlockMaster == null)
            {
                return NotFound();
            }

            var rackBlockDTO = new RackBlockMasterDto
            {
                Id = rackBlockMaster.Id,
                BlockNumber = rackBlockMaster.BlockNumber,
                RackNumber = rackBlockMaster.Rack.RackNumber,
                Remark = rackBlockMaster.Remark,
                RackId = rackBlockMaster.RackId,
                Description = rackBlockMaster.Description
            };

            return Ok(rackBlockDTO);
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
                _rackBlockMasterService.Update(rackBlockMaster);
                await _rackBlockMasterService.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await RackBlockMasterDetailExistsAsync(id))
                    return NotFound();
                else
                    throw;
            }

            return Ok();
        }

        // POST: api/RackBlockMaster
        [ResponseType(typeof(RackBlockMaster))]
        public async Task<IHttpActionResult> PostRackAsync(RackBlockMaster rackBlockMaster)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _rackBlockMasterService.Add(rackBlockMaster);
                await _rackBlockMasterService.CommitAsync();
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

            return CreatedAtRoute("ApiRoute", new { id = rackBlockMaster.Id }, rackBlockMaster);
        }

        // DELETE: api/RackBlockMaster/5
        [ResponseType(typeof(RackBlockMaster))]
        public async Task<IHttpActionResult> DeleteRackBlockMasterDetailAsync(Guid id)
        {
            var rackBlockMaster = await _rackBlockMasterService.GetByIdAsync(id);
            if (rackBlockMaster == null)
            {
                return NotFound();
            }

            _rackBlockMasterService.Delete(rackBlockMaster);
            await _rackBlockMasterService.CommitAsync();

            return Ok(rackBlockMaster);
        }

        private async Task<bool> RackBlockMasterDetailExistsAsync(Guid id)
        {
            return await _rackBlockMasterService.GetByIdAsync(id) != null;
        }

       
    }
}