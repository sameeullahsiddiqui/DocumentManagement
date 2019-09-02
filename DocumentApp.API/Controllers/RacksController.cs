using System;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using DocumentApp.Core.Services;
using DocumentApp.API.Models;
using DocumentApp.Core.Entities;
using DocumentApp.Dto.Dtos;
using System.Collections.Generic;
using AutoMapper;

namespace RackMastersManagement.API.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/Racks")]
    public class RacksController : ApiController
    {
        private readonly IRackMasterService _rackMasterService;
        private readonly IMapper _mapper;

        public RacksController(IRackMasterService rackMasterService, IMapper mapper)
        {
            _rackMasterService = rackMasterService;
            _mapper = mapper;
        }


        // GET: api/RackMaster
        public async Task<IHttpActionResult> GetRackMasterAsync([FromUri] RackPagingViewModel pagingmodel)
        {
            var rack = await _rackMasterService.GetAllAsync();
            IQueryable<RackMaster> source = rack.AsQueryable();

            int currentPage = 1;
            int pageSize = 5;

            if (pagingmodel != null)
            {
                if (!string.IsNullOrEmpty(pagingmodel.RackNumber))
                {
                    source = source.Where(a => a.RackNumber == pagingmodel.RackNumber);
                }

                currentPage = pagingmodel.PageNumber;
                pageSize = pagingmodel.PageSize == -1 ? source.Count() : pagingmodel.PageSize;
            }
            else
            {
                pageSize = source.Count();
            }

            int count = source.Count();

            int totalCount = count;
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            var previousPage = currentPage > 1 ? "Yes" : "No";
            var nextPage = currentPage < totalPages ? "Yes" : "No";
            var paginationMetadata = new{totalCount,pageSize,currentPage,totalPages,previousPage,nextPage};

            var items = source.OrderBy(x => x.RackNumber).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            var rackMasterDtos = _mapper.Map<List<RackMaster>, List<RackMasterDto>>(items);

            if (HttpContext.Current != null)
                HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));

            return Ok(rackMasterDtos);
        }

        // GET: api/RackMaster/5
        [ResponseType(typeof(RackMaster))]
        public async Task<IHttpActionResult> GetRackAsync(Guid id)
        {
            var rackMaster = await _rackMasterService.GetByIdAsync(id);
            if (rackMaster == null)
            {
                return NotFound();
            }

            var rackMasterDto = _mapper.Map<RackMaster, RackMasterDto>(rackMaster);
            return CreatedAtRoute("ApiRoute", new { controller = "Racks", id = rackMasterDto.Id }, rackMasterDto);
        }

        // PUT: api/RackMaster/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutRackAsync(Guid id, RackMasterDto rackMasterDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != rackMasterDto.Id)
                return BadRequest();

            try
            {
                var rackMaster = _mapper.Map<RackMasterDto, RackMaster>(rackMasterDto);
                _rackMasterService.Update(rackMaster);
                await _rackMasterService.CommitAsync();

                return CreatedAtRoute("ApiRoute", new { id = rackMasterDto.Id }, rackMasterDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await RackMasterDetailExistsAsync(id))
                    return NotFound();
                else
                    throw;
            }
        }

        // POST: api/RackMaster
        [ResponseType(typeof(RackMaster))]
        public async Task<IHttpActionResult> PostRackAsync(RackMaster rackMaster)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _rackMasterService.Add(rackMaster);
                await _rackMasterService.CommitAsync();
                var rackMasterDto = _mapper.Map<RackMaster, RackMasterDto>(rackMaster);
                return CreatedAtRoute("ApiRoute", new { controller = "Racks", id = rackMasterDto.Id }, rackMasterDto);
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

            
        }

        // DELETE: api/RackMaster/5
        [ResponseType(typeof(RackMaster))]
        public async Task<IHttpActionResult> DeleteRackMasterDetailAsync(Guid id)
        {
            var rackMaster = await _rackMasterService.GetByIdAsync(id);
            if (rackMaster == null)
            {
                return NotFound();
            }

            _rackMasterService.Delete(rackMaster);
            await _rackMasterService.CommitAsync();

            var rackMasterDto = _mapper.Map<RackMaster, RackMasterDto>(rackMaster);

            return CreatedAtRoute("ApiRoute", new { controller = "Racks", id = rackMasterDto.Id }, rackMasterDto);
        }

        private async Task<bool> RackMasterDetailExistsAsync(Guid id)
        {
            return await _rackMasterService.GetByIdAsync(id) != null;
        }

        
    }
}