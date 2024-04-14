using HB.Database.DbModels;
using Strateq.Core.Database.Repositories.Base;

namespace HB.Database.Repositories
{
    public interface IPriceVariesRepository : IRepositoryBase<PriceVaries>
    {
        Task<PriceVaries> FindByIdAsync(int id);
        Task<List<PriceVaries>> GetAllAsync();
        IQueryable<PriceVaries> ToQueryable();
    }
}