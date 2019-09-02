using DocumentApp.Core.Data;
using DocumentApp.Core.Entities;
using DocumentApp.Core.Services;

namespace DocumentApp.Services.Services
{
    public class RackMasterService : BaseService<RackMaster>, IRackMasterService
    {
        public RackMasterService(IUnitOfWork unitOfWork): base(unitOfWork)
        {
        }
    }
}
