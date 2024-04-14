using HB.Database.DbModels;
using Strateq.Core.Database.Repositories.Base;

namespace HB.Database.Repositories
{
    public interface IProductTypeRepository : IRepositoryBase<ProductType>
    {
        Task<ProductType> FindByIdAsync(int id);
        IQueryable<ProductType> ToQueryable();
    }
}