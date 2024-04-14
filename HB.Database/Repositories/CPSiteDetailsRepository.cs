using Microsoft.EntityFrameworkCore;
using HB.Database.DbModels;
using Strateq.Core.Database.Repositories.Base;
using System;

namespace HB.Database.Repositories
{
    public class CPSiteDetailsRepository : RepositoryBase<CPSiteDetails>, ICPSiteDetailsRepository
    {
        #region Fields
        private readonly HBContext _context;
        private readonly DbSet<CPSiteDetails> _dbSet;
        #endregion

        #region Ctor
        public CPSiteDetailsRepository(HBContext _context) : base(_context)
        {
            _dbSet = _context.CPSiteDetails ?? throw new Exception(nameof(_context.CPSiteDetails));
            this._context = _context;
        }
        #endregion

        #region Methods
        public async Task<List<CPSiteDetails>> GetAllAsync()
        {
            return await GetAll()
                .Where(x => x.IsActive).ToListAsync();
        }

        public async Task<CPSiteDetails> FindByIdAsync(int id)
        {
            return await GetAll()
                .FirstOrDefaultAsync(x => x.IsActive && x.Id == id);
        }

        public IQueryable<CPSiteDetails> ToQueryable()
        {
            return _dbSet.Where(x => x.IsActive);
        }
        #endregion
    }
}
