using Strateq.Core.Database.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HB.Database.DbModels;
using Microsoft.EntityFrameworkCore;

namespace HB.Database.Repositories
{
    public class CPBreakdownDurationDetailsRepository: RepositoryBase<CPBreakdownDurationDetails>, ICPBreakdownDurationDetailsRepository
    {
        #region Fields
        private readonly HBContext _context;
        private readonly DbSet<CPBreakdownDurationDetails> _dbSet;
        #endregion

        #region Ctor
        public CPBreakdownDurationDetailsRepository(HBContext _context) : base(_context)
        {
            _dbSet = _context.CPBreakdownDurationDetails ?? throw new Exception(nameof(_context.CPBreakdownDurationDetails));
            this._context = _context;
        }
        #endregion

        #region Methods
        public async Task<List<CPBreakdownDurationDetails>> GetAllAsync()
        {
            return await GetAll()
                .Where(x => x.IsActive).ToListAsync();
        }

        public async Task<CPBreakdownDurationDetails> FindByIdAsync(int id)
        {
            return await GetAll()
                .FirstOrDefaultAsync(x => x.IsActive && x.Id == id);
        }

        public IQueryable<CPBreakdownDurationDetails> ToQueryable()
        {
            return _dbSet.Where(x => x.IsActive);
        }
        #endregion
    }
}
