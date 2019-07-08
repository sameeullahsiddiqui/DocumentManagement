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
    public class FileAllocationRepository : GenericRepository<FileAllocation>, IFileAllocationRepository
    {
        public FileAllocationRepository(DbContext context): base(context)
        {

        }

        public override IQueryable<FileAllocation> GetAll()
        {
            return _entities.Set<FileAllocation>().AsQueryable();
        }

        public async Task<FileAllocation> GetByIdAsync(Guid id)
        {
            return await _dbset.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
