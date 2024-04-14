using HB.Database.DbModels;
using Strateq.Core.Database.Repositories.Base;

namespace HB.Database.Repositories
{
    public interface IPaymentDetailsRepository : IRepositoryBase<PaymentDetails>
    {
        Task<PaymentDetails> FindByIdAsync(int id);
        IQueryable<PaymentDetails> ToQueryable();
    }
}