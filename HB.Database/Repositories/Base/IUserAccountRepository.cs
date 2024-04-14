using HB.Database.DbModels;
using Strateq.Core.Database.Repositories.Base;

namespace HB.Database.Repositories
{
    public interface IUserAccountRepository : IRepositoryBase<UserAccount>
    {
        Task<UserAccount> FindByIdAsync(int id);
        IQueryable<UserAccount> ToQueryable();
    }
}