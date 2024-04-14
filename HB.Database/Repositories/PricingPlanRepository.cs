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
    public class PricingPlanRepository : RepositoryBase<PricingPlan>, IPricingPlanRepository
    {
        #region Fields
        private readonly HBContext _context;
        private readonly DbSet<PricingPlan> _dbSet;
        #endregion

        #region Ctor
        public PricingPlanRepository(HBContext _context) : base(_context)
        {
            _dbSet = _context.PricingPlan;
            this._context = _context;
        }
        #endregion

        #region Methods
        public async Task<PricingPlan> FindByIdAsync(int id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        }

        public IQueryable<PricingPlan> ToQueryable()
        {
            return _dbSet;
        }
        #endregion
    }
}
