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
    public class RackRepository : GenericRepository<RackMaster>, IRackRepository
    {
        public RackRepository(DbContext context): base(context)
        {

        }

        public override IQueryable<RackMaster> GetAll()
        {
            return _entities.Set<RackMaster>().Include(x => x.RackBlocks).AsQueryable();
        }

        public async Task<RackMaster> GetByIdAsync(Guid id)
        {
            return await _dbset.Include(x => x.RackBlocks).Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
