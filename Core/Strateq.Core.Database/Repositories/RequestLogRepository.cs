using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Strateq.Core.Database.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Strateq.Core.Database;
using Strateq.Core.Database.DbModel;

namespace Strateq.Core.Database.Repositories
{
    public class RequestLogRepository : IRequestLogRepository
    {
        private LoggingContext _dbContext;
        private readonly DbSet<RequestLog> _dbSet;

        public RequestLogRepository(LoggingContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.RequestLog;
        }

        public async Task<RequestLog> AddAndSaveChangesAsync(RequestLog entity)
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

        public async Task<RequestLog> UpdateAndSaveChangesAsync(RequestLog entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(RequestLog)} entity must not be null");
            }

            try
            {
                _dbSet.Update(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message} {ex}");
            }
        }
    }
}
