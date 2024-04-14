using HB.Database.DbModels;
using Microsoft.EntityFrameworkCore;
using Strateq.Core.Database.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Database.Repositories
{
    public class PaymentDetailsRepository : RepositoryBase<PaymentDetails>, IPaymentDetailsRepository
    {
        #region Fields
        private readonly HBContext _context;
        private readonly DbSet<PaymentDetails> _dbSet;
        #endregion

        #region Ctor
        public PaymentDetailsRepository(HBContext _context) : base(_context)
        {
            _dbSet = _context.PaymentDetails;
            this._context = _context;
        }
        #endregion

        #region Methods
        public async Task<PaymentDetails> FindByIdAsync(int id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        }

        public IQueryable<PaymentDetails> ToQueryable()
        {
            return _dbSet;
        }
        #endregion
    }
}
