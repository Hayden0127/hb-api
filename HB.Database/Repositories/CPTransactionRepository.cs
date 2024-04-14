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
    public class CPTransactionRepository : RepositoryBase<CPTransaction>, ICPTransactionRepository
    {
        #region Fields
        private readonly HBContext _context;
        private readonly DbSet<CPTransaction> _dbSet;
        #endregion

        #region Ctor
        public CPTransactionRepository(HBContext _context) : base(_context)
        {
            _dbSet = _context.CPTransaction ?? throw new Exception(nameof(_context.CPTransaction));
            this._context = _context;
        }
        #endregion

        #region Methods
        public async Task<List<CPTransaction>> GetAllAsync()
        {
            return await GetAll()
                .Where(x => x.IsActive).ToListAsync();
        }

        public async Task<CPTransaction> FindByIdAsync(int id)
        {
            return await GetAll()
                .FirstOrDefaultAsync(x => x.IsActive && x.Id == id);
        }

        public IQueryable<CPTransaction> ToQueryable()
        {
            return _dbSet.Where(x => x.IsActive);
        }
        #endregion
    }
}
