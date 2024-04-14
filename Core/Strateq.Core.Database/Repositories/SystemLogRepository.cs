using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Strateq.Core.Database;
using Strateq.Core.Database.DbModel;

namespace Strateq.Core.Database.Repositories
{
    public class SystemLogRepository : ISystemLogRepository
    {
        private LoggingContext _dbContext;
        private DbSet<SystemLog> _dbSet;

        public SystemLogRepository(LoggingContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.SystemLog;
        }

        public async Task<SystemLog> AddAndSaveChangesAsync(SystemLog entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(entity)} entity must not be null");
            }

            try
            {
                await _dbSet.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved:{ex.Message} {ex}");
            }
        }
    }
}
