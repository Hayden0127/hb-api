using Microsoft.EntityFrameworkCore;
using HB.Database.DbModels;
using Strateq.Core.Database.Repositories.Base;
using System;

namespace HB.Database.Repositories
{
    public class CPConnectorRepository : RepositoryBase<CPConnector>, ICPConnectorRepository
    {
        #region Fields
        private readonly HBContext _context;
        private readonly DbSet<CPConnector> _dbSet;
        #endregion

        #region Ctor
        public CPConnectorRepository(HBContext _context) : base(_context)
        {
            _dbSet = _context.CPConnector ?? throw new Exception(nameof(_context.CPConnector));
            this._context = _context;
        }
        #endregion

        #region Methods
        public async Task<List<CPConnector>> GetAllAsync()
        {
            return await GetAll()
                .Where(x => x.IsActive).ToListAsync();
        }

        public async Task<CPConnector> FindByIdAsync(int id)
        {
            return await GetAll()
                .FirstOrDefaultAsync(x => x.IsActive && x.Id == id);
        }

        public IQueryable<CPConnector> ToQueryable()
        {
            return _dbSet.Where(x => x.IsActive);
        }
        #endregion
    }
}
