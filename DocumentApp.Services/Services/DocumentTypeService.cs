using DocumentApp.Core.Data;
using DocumentApp.Core.Entities;
using DocumentApp.Core.Services;

namespace DocumentApp.Services.Services
{
    public class DocumentTypeService : BaseService<DocumentType>, IDocumentTypeService
    {
        public DocumentTypeService(IUnitOfWork unitOfWork): base(unitOfWork)
        {
        }
    }
}
