﻿using DocumentApp.Core.Data.Repositories;
using DocumentApp.Core.Entities.Foundation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentApp.Data.Repositories
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly IDbContext _context;
        private readonly IDbSet<TEntity> _dbEntitySet;
        private bool _disposed;

        public BaseRepository(IDbContext context)
        {
            _context = context;
            _dbEntitySet = _context.Set<TEntity>();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbEntitySet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbEntitySet.FirstOrDefaultAsync(t => t.Id == id);
        }

        public IQueryable<TEntity> GetAllQuerableAsync()
        {
            return _dbEntitySet;
        }

        public IQueryable<TEntity> GetByIdQuerableAsync(Guid id)
        {
            return _dbEntitySet.Where(t => t.Id == id);
        }


        public TEntity Add(TEntity entity)
        {
            _context.SetAsAdded(entity);

            return entity;
        }

        public void Update(TEntity entity)
        {
            _context.SetAsModified(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.SetAsDeleted(entity);
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
                _context.Dispose();
            }
            _disposed = true;
        }

    }
}
