
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Strateq.Core.API.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Strateq.Core.Database.Repositories.Base
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class, new()
    {
        //private readonly IDbFactory _dbFactory;
        private readonly CoreContext _dbContext;
        private DbSet<TEntity> _dbSet;

        public RepositoryBase(CoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected DbSet<TEntity> DbSet
        {
            get => _dbSet ?? (_dbSet = _dbContext.Set<TEntity>());
        }

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return DbSet;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                await DbSet.AddAsync(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved:{ex.Message} {ex}");
            }
        }

        public async Task<TEntity> AddAndSaveChangesAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                await DbSet.AddAsync(entity);
                await _dbContext.SaveAuditedChanges();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved:{ex.Message} {ex}");
            }
        }

        public async Task<IList<TEntity>> AddRangeAndSaveChangesAsync(IList<TEntity> entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                await DbSet.AddRangeAsync(entity);
                await _dbContext.SaveAuditedChanges();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved:{ex.Message} {ex}");
            }
        }

        public TEntity Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                DbSet.Update(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message} {ex}");
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
               await _dbContext.SaveAuditedChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"could not be saved: {ex.Message} {ex}");
            }
        }

        public async Task<TEntity> UpdateAndSaveChangesAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                DbSet.Update(entity);
                //await _dbFactory.DbContext.SaveAuditedChanges(_eventBus, source);
                await _dbContext.SaveAuditedChanges();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message} {ex}");
            }
        }

        public async Task<IList<TEntity>> UpdateRangeAndSaveChangesAsync(IList<TEntity> entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                DbSet.UpdateRange(entity);
                //await _dbFactory.DbContext.SaveAuditedChanges(_eventBus, source);
                await _dbContext.SaveAuditedChanges();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message} {ex}");
            }
        }

        public async Task<TEntity> DeleteAndSaveChangesAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                DbSet.Remove(entity);
                //await _dbFactory.DbContext.SaveAuditedChanges(_eventBus, source);
                await _dbContext.SaveAuditedChanges();
                return entity;
            }
            catch (Exception ex)
            {

                if (ex is DbUpdateException dbUpdateEx)
                {
                    Int32 ErrorCode = ((SqlException)ex.InnerException).Number;

                    if (dbUpdateEx.InnerException != null)
                    {
                        if (ErrorCode == 547)
                        {
                            throw new ForeignKeyConflictException(dbUpdateEx.InnerException.Message, dbUpdateEx.InnerException);
                        }
                    }
                }

                throw new Exception($"{nameof(entity)} could not be updated: {ex.InnerException} {ex}");
            }
        }

        public async Task<IEnumerable<TEntity>> DeleteRangeAndSaveChangesAsync(IEnumerable<TEntity> entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                DbSet.RemoveRange(entity);
                await _dbContext.SaveAuditedChanges();
                return entity;
            }
            catch (Exception ex)
            {

                if (ex is DbUpdateException dbUpdateEx)
                {
                    Int32 ErrorCode = ((SqlException)ex.InnerException).Number;

                    if (dbUpdateEx.InnerException != null)
                    {
                        if (ErrorCode == 547)
                        {
                            throw new ForeignKeyConflictException(dbUpdateEx.InnerException.Message, dbUpdateEx.InnerException);
                        }
                    }
                }

                throw new Exception($"{nameof(entity)} could not be updated: {ex.InnerException} {ex}");
            }

        }
    }
}