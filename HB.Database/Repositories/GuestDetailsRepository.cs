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
    public class GuestDetailsRepository : RepositoryBase<GuestDetails>, IGuestDetailsRepository
    {
        #region Fields
        private readonly HBContext _context;
        private readonly DbSet<GuestDetails> _dbSet;
        #endregion

        #region Ctor
        public GuestDetailsRepository(HBContext _context) : base(_context)
        {
            _dbSet = _context.GuestDetails;
            this._context = _context;
        }
        #endregion

        #region Methods
        public async Task<GuestDetails> FindByIdAsync(int id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        }

        public IQueryable<GuestDetails> ToQueryable()
        {
            return _dbSet;
        }
        #endregion
    }
}
