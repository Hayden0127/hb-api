using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace Strateq.Core.Database.UnitOfWork
{
    public class UnitOfWorkBase : IUnitOfWorkBase, IDisposable
    {
        private IDbContextTransaction _objTran;
        private readonly DbContext _dbContext;

        public UnitOfWorkBase(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CommitTransaction()
        {
            _objTran.Commit();
            _objTran.Dispose();
        }

        public void CreateTransaction()
        {
            _objTran = _dbContext.Database.BeginTransaction();
        }

        public void RollbackTransaction()
        {
            if (_objTran != null)
            {
                _objTran.Rollback();
                _objTran.Dispose();
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
