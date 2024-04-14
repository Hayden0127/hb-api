using HB.Database.DbModels;
using Strateq.Core.Database.Repositories.Base;

namespace HB.Database.Repositories
{
    public interface ICPDetailsRepository : IRepositoryBase<CPDetails>
    {
        Task<List<CPDetails>> GetAllAsync();
        Task<CPDetails> FindByIdAsync(int id);
        IQueryable<CPDetails> ToQueryable();
    }
}