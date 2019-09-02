using DocumentApp.Core.Data;
using DocumentApp.Core.Entities;
using DocumentApp.Core.Services;

namespace DocumentApp.Services.Services
{
    public class RackBlockMasterService : BaseService<RackBlockMaster>, IRackBlockMasterService
    {
        public RackBlockMasterService(IUnitOfWork unitOfWork): base(unitOfWork)
        {
        }
    }
}
