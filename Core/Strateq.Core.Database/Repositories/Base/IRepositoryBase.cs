using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Strateq.Core.Database.Repositories.Base
{
    public interface IRepositoryBase<TEntity> where TEntity : class, new()
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> AddAndSaveChangesAsync(TEntity entity);
        Task<IList<TEntity>> AddRangeAndSaveChangesAsync(IList<TEntity> entity);
        TEntity Update(TEntity entity);
        Task<TEntity> UpdateAndSaveChangesAsync(TEntity entity);
        Task<IList<TEntity>> UpdateRangeAndSaveChangesAsync(IList<TEntity> entity);
        Task SaveChangesAsync();
        Task<TEntity> DeleteAndSaveChangesAsync(TEntity entity);
        Task<IEnumerable<TEntity>> DeleteRangeAndSaveChangesAsync(IEnumerable<TEntity> entity);
    }
}