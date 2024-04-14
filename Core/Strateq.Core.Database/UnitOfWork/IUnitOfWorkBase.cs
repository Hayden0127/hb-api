using System;
using System.Threading.Tasks;

namespace Strateq.Core.Database.UnitOfWork
{
    public interface IUnitOfWorkBase : IDisposable
    {
        //Task<int> SaveChanges();
        void CommitTransaction();
        void CreateTransaction();
        void RollbackTransaction();

    }
}
