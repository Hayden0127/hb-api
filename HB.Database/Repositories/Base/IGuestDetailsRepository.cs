using HB.Database.DbModels;
using Strateq.Core.Database.Repositories.Base;

namespace HB.Database.Repositories
{
    public interface IGuestDetailsRepository : IRepositoryBase<GuestDetails>
    {
        Task<GuestDetails> FindByIdAsync(int id);
        IQueryable<GuestDetails> ToQueryable();
    }
}