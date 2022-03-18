using MarketPlace.dataLayer.Context;
using MarketPlace.dataLayer.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly MarketPlaceDbContext _context;
        private readonly DbSet<TEntity> _dBSet;
        public GenericRepository(MarketPlaceDbContext context)
        {
            _context = context;
            _dBSet = _context.Set<TEntity>();
        }

        public async Task AddEntity(TEntity entity)
        {
            entity.CreateDate = DateTime.Now;
           await _context.AddAsync(entity);
        }

        public void DeleteEntity(TEntity entity)
        {
            entity.IsRemoved = true;
            EditEntity(entity);
        }

        public async Task DeleteEntity(long entityId)
        {
            DeleteEntity(await GetEntityById(entityId));
        }

        public void DeletePermanet(TEntity entity)
        {
            _context.Remove(entity);
        }

        public async Task DeletePermanet(long entityId)
        {
            DeletePermanet(await GetEntityById(entityId));
        }

        public async ValueTask DisposeAsync()
        {
            if (_context != null)await _context.DisposeAsync();
           
        }

        public void EditEntity(TEntity entity)
        {
            _context.Update(entity);
        }

        public async Task<TEntity> GetEntityById(long entityId)
        {
          return await _dBSet.SingleOrDefaultAsync(p=>p.Id==entityId);
        }

        public IQueryable<TEntity> GetQuery()
        {
            return _dBSet.AsQueryable();
        }

        public async Task SaveChangesAsync()
        {
           await _context.SaveChangesAsync();
        }
    }
}
