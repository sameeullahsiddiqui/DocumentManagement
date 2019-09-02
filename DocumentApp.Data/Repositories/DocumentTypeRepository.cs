using DocumentApp.Core.Data.Repositories;
using DocumentApp.Core.Entities;

namespace DocumentApp.Data.Repositories
{
    public class DocumentTypeRepository : BaseRepository<DocumentType>, IDocumentTypeRepository
    {
        public DocumentTypeRepository(IDbContext context) : base(context)
        {
        }
    }
}
