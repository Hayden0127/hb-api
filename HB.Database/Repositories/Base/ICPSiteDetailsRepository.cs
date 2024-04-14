using HB.Database.DbModels;
using Strateq.Core.Database.Repositories.Base;

namespace HB.Database.Repositories
{
    public interface ICPSiteDetailsRepository : IRepositoryBase<CPSiteDetails>
    {
        Task<List<CPSiteDetails>> GetAllAsync();
        Task<CPSiteDetails> FindByIdAsync(int id);
        IQueryable<CPSiteDetails> ToQueryable();
    }
}