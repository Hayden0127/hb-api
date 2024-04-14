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
    public class CPBreakdownErrorRepository : RepositoryBase<CPBreakdownError>, ICPBreakdownErrorRepository
    {
        #region Fields
        private readonly HBContext _context;
        private readonly DbSet<CPBreakdownError> _dbSet;
        #endregion

        #region Ctor
        public CPBreakdownErrorRepository(HBContext _context) : base(_context)
        {
            _dbSet = _context.CPBreakdownError ?? throw new Exception(nameof(_context.CPBreakdownError));
            this._context = _context;
        }
        #endregion

        #region Methods
        public async Task<List<CPBreakdownError>> GetAllAsync()
        {
            return await GetAll()
                .Where(x => x.IsActive).ToListAsync();
        }

        public async Task<CPBreakdownError> FindByIdAsync(int id)
        {
            return await GetAll()
                .FirstOrDefaultAsync(x => x.IsActive && x.Id == id);
        }

        public IQueryable<CPBreakdownError> ToQueryable()
        {
            return _dbSet.Where(x => x.IsActive);
        }
        #endregion
    }
}
