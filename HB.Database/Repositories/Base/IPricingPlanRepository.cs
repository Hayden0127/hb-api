using HB.Database.DbModels;
using Strateq.Core.Database.Repositories.Base;

namespace HB.Database.Repositories
{
    public interface IPricingPlanRepository : IRepositoryBase<PricingPlan>
    {
        Task<PricingPlan> FindByIdAsync(int id);
        IQueryable<PricingPlan> ToQueryable();
    }
}