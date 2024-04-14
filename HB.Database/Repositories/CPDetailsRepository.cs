using Microsoft.EntityFrameworkCore;
using HB.Database.DbModels;
using Strateq.Core.Database.Repositories.Base;
using System;

namespace HB.Database.Repositories
{
    public class CPDetailsRepository : RepositoryBase<CPDetails>, ICPDetailsRepository
    {
        #region Fields
        private readonly HBContext _context;
        private readonly DbSet<CPDetails> _dbSet;
        #endregion

        #region Ctor
        public CPDetailsRepository(HBContext _context) : base(_context)
        {
            _dbSet = _context.CPDetails ?? throw new Exception(nameof(_context.CPDetails));
            this._context = _context;
        }
        #endregion

        #region Methods
        public async Task<List<CPDetails>> GetAllAsync()
        {
            return await GetAll()
                .Where(x => x.IsActive).ToListAsync();
        }

        public async Task<CPDetails> FindByIdAsync(int id)
        {
            return await GetAll()
                .FirstOrDefaultAsync(x => x.IsActive && x.Id == id);
        }

        public IQueryable<CPDetails> ToQueryable()
        {
            return _dbSet.Where(x => x.IsActive);
        }
        #endregion
    }
}
