using DocumentApp.Core.Data;
using DocumentApp.Core.Entities;
using DocumentApp.Core.Services;

namespace DocumentApp.Services.Services
{
    public class FileAllocationService : BaseService<FileAllocation>, IFileAllocationService
    {
        public FileAllocationService(IUnitOfWork unitOfWork): base(unitOfWork)
        {
        }
    }
}
