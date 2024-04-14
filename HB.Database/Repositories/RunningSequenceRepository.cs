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
    public class RunningSequenceRepository : RepositoryBase<RunningSequenceNumber>, IRunningSequenceRepository
    {
        #region Fields
        private readonly HBContext _context;
        private readonly DbSet<RunningSequenceNumber> _dbSet;
        #endregion

        #region Ctor
        public RunningSequenceRepository(HBContext _context) : base(_context)
        {
            _dbSet = _context.RunningSequenceNumber;
            this._context = _context;
        }
        #endregion

        #region Methods
        public async Task<RunningSequenceNumber> FindByIdAsync(int id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
        }

        public IQueryable<RunningSequenceNumber> ToQueryable()
        {
            return _dbSet;
        }
        #endregion
    }
}
