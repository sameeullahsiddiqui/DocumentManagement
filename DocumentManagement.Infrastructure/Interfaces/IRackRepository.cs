using DocumentManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Infrastructure.Interfaces
{
    public interface IRackRepository : IGenericRepository<RackMaster>
    {
        Task<RackMaster> GetByIdAsync(Guid id);
    }
}
