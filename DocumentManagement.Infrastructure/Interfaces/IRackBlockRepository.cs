using DocumentManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Infrastructure.Interfaces
{
    public interface IRackBlockRepository : IGenericRepository<RackBlockMaster>
    {
        Task<RackBlockMaster> GetByIdAsync(Guid id);
    }
}
