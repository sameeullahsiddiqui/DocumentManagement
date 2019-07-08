using DocumentManagement.Core.Models;
using DocumentManagement.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Infrastructure.Repository
{
    public class RackBlockRepository : GenericRepository<RackBlockMaster>, IRackBlockRepository
    {
        public RackBlockRepository(DbContext context): base(context)
        {

        }

        public override IQueryable<RackBlockMaster> GetAll()
        {
            return _entities.Set<RackBlockMaster>().AsQueryable();
        }

        public async Task<RackBlockMaster> GetByIdAsync(Guid id)
        {
            return await _dbset.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
