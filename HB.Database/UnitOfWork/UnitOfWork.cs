using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using HB.Database;
using Strateq.Core.Database.UnitOfWork;


namespace HB.Database.UnitOfWork
{
    public class UnitOfWork : UnitOfWorkBase, IUnitOfWork
    {
        private readonly HBContext _dbContext;

        public UnitOfWork(HBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
