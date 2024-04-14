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
    public class PricingPlanTypeRepository : RepositoryBase<PricingPlanType>, IPricingPlanTypeRepository
    {
        #region Fields
        private readonly HBContext _context;
        private readonly DbSet<PricingPlanType> _dbSet;
        #endregion

        #region Ctor
        public PricingPlanTypeRepository(HBContext _context) : base(_context)
        {
            _dbSet = _context.PricingPlanType;
            this._context = _context;
        }
        #endregion

        #region Methods
        public async Task<PricingPlanType> FindByIdAsync(int id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        }

        public IQueryable<PricingPlanType> ToQueryable()
        {
            return _dbSet;
        }
        #endregion
    }
}
