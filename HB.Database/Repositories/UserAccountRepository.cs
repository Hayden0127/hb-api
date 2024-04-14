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
    public class UserAccountRepository : RepositoryBase<UserAccount>, IUserAccountRepository
    {
        #region Fields
        private readonly HBContext _context;
        private readonly DbSet<UserAccount> _dbSet;
        #endregion

        #region Ctor
        public UserAccountRepository(HBContext _context) : base(_context)
        {
            _dbSet = _context.UserAccount;
            this._context = _context;
        }
        #endregion

        #region Methods
        public async Task<UserAccount> FindByIdAsync(int id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        }

        public IQueryable<UserAccount> ToQueryable()
        {
            return _dbSet;
        }
        #endregion
    }
}
