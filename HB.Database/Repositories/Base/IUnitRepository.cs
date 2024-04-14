using HB.Database.DbModels;
using Strateq.Core.Database.Repositories.Base;

namespace HB.Database.Repositories
{
    public interface IUnitRepository : IRepositoryBase<Unit>
    {
        Task<Unit> FindByIdAsync(int id);
        Task<List<Unit>> GetAllAsync();
        IQueryable<Unit> ToQueryable();
    }
}