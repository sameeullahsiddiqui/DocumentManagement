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
    public class DocumentTypeRepository : GenericRepository<DocumentsType>, IDocumentTypeRepository
    {
        public DocumentTypeRepository(DbContext context): base(context)
        {

        }

        public override IQueryable<DocumentsType> GetAll()
        {
            return _entities.Set<DocumentsType>().AsQueryable();
        }

        public async Task<DocumentsType> GetByIdAsync(Guid id)
        {
            return await _dbset.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
