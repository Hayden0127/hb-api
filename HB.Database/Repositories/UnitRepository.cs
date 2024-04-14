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
    public class UnitRepository : RepositoryBase<Unit>, IUnitRepository
    {
        #region Fields
        private readonly HBContext _context;
        private readonly DbSet<Unit> _dbSet;
        #endregion

        #region Ctor
        public UnitRepository(HBContext _context) : base(_context)
        {
            _dbSet = _context.Unit;
            this._context = _context;
        }
        #endregion

        #region Methods
        public async Task<Unit> FindByIdAsync(int id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        }
        
        public async Task<List<Unit>> GetAllAsync()
        {
            return await GetAll()
                .Where(x => x.IsActive).ToListAsync();
        }

        public IQueryable<Unit> ToQueryable()
        {
            return _dbSet;
        }
        #endregion
    }
}
