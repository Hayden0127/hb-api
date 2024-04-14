using HB.Database.DbModels;
using Strateq.Core.Database.Repositories.Base;

namespace HB.Database.Repositories
{
    public interface IUserAccountAuthorizationTokenRepository : IRepositoryBase<UserAccountAuthorizationToken>
    {
        Task<UserAccountAuthorizationToken> FindByIdAsync(int id);
        IQueryable<UserAccountAuthorizationToken> ToQueryable();
    }
}