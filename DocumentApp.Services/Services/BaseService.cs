using DocumentApp.Core.Data;
using DocumentApp.Core.Data.Repositories;
using DocumentApp.Core.Entities.Foundation;
using DocumentApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentApp.Services.Services
{
    public class BaseService<TEntity> : IService<TEntity> where TEntity : BaseEntity
    {
        public IUnitOfWork UnitOfWork { get; private set; }
        private readonly IRepository<TEntity> _repository;
        private bool _disposed;

        public BaseService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            _repository = UnitOfWork.Repository<TEntity>();
        }

        public TEntity Add(TEntity entity)
        {
            _repository.Add(entity);
            return entity;
        }

        public void Update(TEntity entity)
        {
            _repository.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _repository.Delete(entity);
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<TEntity> GetByIdAsync(Guid id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task CommitAsync()
        {
            return UnitOfWork.CommitAsync();
        }

        public int Commit()
        {
            return UnitOfWork.Commit();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                UnitOfWork.Dispose();
            }
            _disposed = true;
        }

        public IQueryable<TEntity> GetAllQuerableAsync()
        {
            return _repository.GetAllQuerableAsync();
        }

        public IQueryable<TEntity> GetByIdQuerableAsync(Guid id)
        {
            return _repository.GetByIdQuerableAsync(id);
        }
    }
}
