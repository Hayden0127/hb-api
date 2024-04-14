using HB.Database.DbModels;
using Strateq.Core.Database.Repositories.Base;

namespace HB.Database.Repositories
{
    public interface IPricingPlanTypeRepository : IRepositoryBase<PricingPlanType>
    {
        Task<PricingPlanType> FindByIdAsync(int id);
        IQueryable<PricingPlanType> ToQueryable();
    }
}