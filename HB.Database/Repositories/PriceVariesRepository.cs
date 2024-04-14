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
    public class PriceVariesRepository : RepositoryBase<PriceVaries>, IPriceVariesRepository
    {
        #region Fields
        private readonly HBContext _context;
        private readonly DbSet<PriceVaries> _dbSet;
        #endregion

        #region Ctor
        public PriceVariesRepository(HBContext _context) : base(_context)
        {
            _dbSet = _context.PriceVaries;
            this._context = _context;
        }
        #endregion

        #region Methods
        public async Task<PriceVaries> FindByIdAsync(int id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        }

        public async Task<List<PriceVaries>> GetAllAsync()
        {
            return await GetAll()
                .Where(x => x.IsActive).ToListAsync();
        }

        public IQueryable<PriceVaries> ToQueryable()
        {
            return _dbSet;
        }
        #endregion
    }
}
