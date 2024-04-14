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
    public class MeterValueRepository: RepositoryBase<MeterValue>, IMeterValueRepository
    {
        #region Fields
        private readonly HBContext _context;
        private readonly DbSet<MeterValue> _dbSet;
        #endregion

        #region Ctor
        public MeterValueRepository(HBContext _context) : base(_context)
        {
            _dbSet = _context.MeterValue ?? throw new Exception(nameof(_context.MeterValue));
            this._context = _context;
        }
        #endregion

        #region Methods
        public async Task<List<MeterValue>> GetAllAsync()
        {
            return await GetAll()
                .Where(x => x.IsActive).ToListAsync();
        }

        public async Task<MeterValue> FindByIdAsync(int id)
        {
            return await GetAll()
                .FirstOrDefaultAsync(x => x.IsActive && x.Id == id);
        }

        public IQueryable<MeterValue> ToQueryable()
        {
            return _dbSet.Where(x => x.IsActive);
        }
        #endregion
    }
}
