using MarketPlace.dataLayer.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.dataLayer.Repository
{
   public interface IGenericRepository<TEntity> :IAsyncDisposable where TEntity :BaseEntity
    {
        Task AddEntity(TEntity entity);
        void EditEntity(TEntity entity);
        Task<TEntity> GetEntityById(long entityId);
        void DeleteEntity(TEntity entity);
        Task DeleteEntity(long entityId); 
        void DeletePermanet(TEntity entity);
        Task DeletePermanet(long entityId);
        Task SaveChangesAsync();
        IQueryable<TEntity> GetQuery();

    }
}
