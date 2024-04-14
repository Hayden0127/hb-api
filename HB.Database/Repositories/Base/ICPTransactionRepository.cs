using HB.Database.DbModels;
using Strateq.Core.Database.Repositories.Base;

namespace HB.Database.Repositories
{
    public interface ICPTransactionRepository : IRepositoryBase<CPTransaction>
    {
        Task<List<CPTransaction>> GetAllAsync();
        Task<CPTransaction> FindByIdAsync(int id);
        IQueryable<CPTransaction> ToQueryable();
    }
}