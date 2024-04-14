using HB.Database.DbModels;
using Strateq.Core.Database.Repositories.Base;

namespace HB.Database.Repositories
{
    public interface ICPConnectorRepository : IRepositoryBase<CPConnector>
    {
        Task<List<CPConnector>> GetAllAsync();
        Task<CPConnector> FindByIdAsync(int id);
        IQueryable<CPConnector> ToQueryable();
    }
}