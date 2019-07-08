using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace DocumentManagement.Infrastructure.Interfaces
{
    public interface IRepositoryBase<T> : IDisposable
    {
        IQueryable<T> Get();
        T Get(Guid id);
        void Create(T record);
        void Update(T record);
        void Delete(Guid id);
        int Save();
        Task<int> SaveAsync();
    }
}