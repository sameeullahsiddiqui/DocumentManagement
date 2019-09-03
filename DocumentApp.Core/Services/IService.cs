using DocumentApp.Core.Entities.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentApp.Core.Services
{
    public interface IService<TEntity> : IDisposable where TEntity : BaseEntity
    {
        Task<List<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(Guid id);
        IQueryable<TEntity> GetAllQuerableAsync();
        IQueryable<TEntity> GetByIdQuerableAsync(Guid id);
        TEntity Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task CommitAsync();

        int Commit();
    }
}
