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
    public class UserAccountAuthorizationTokenRepository : RepositoryBase<UserAccountAuthorizationToken>, IUserAccountAuthorizationTokenRepository
    {
        #region Fields
        private readonly HBContext _context;
        private readonly DbSet<UserAccountAuthorizationToken> _dbSet;
        #endregion

        #region Ctor
        public UserAccountAuthorizationTokenRepository(HBContext _context) : base(_context)
        {
            _dbSet = _context.UserAccountAuthorizationToken;
            this._context = _context;
        }
        #endregion

        #region Methods
        public async Task<UserAccountAuthorizationToken> FindByIdAsync(int id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        }

        public IQueryable<UserAccountAuthorizationToken> ToQueryable()
        {
            return _dbSet;
        }
        #endregion
    }
}
