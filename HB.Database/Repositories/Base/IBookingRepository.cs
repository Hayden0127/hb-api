using HB.Database.DbModels;
using Strateq.Core.Database.Repositories.Base;

namespace HB.Database.Repositories
{
    public interface IBookingRepository : IRepositoryBase<Booking>
    {
        Task<Booking> FindByIdAsync(int id);
        IQueryable<Booking> ToQueryable();
    }
}