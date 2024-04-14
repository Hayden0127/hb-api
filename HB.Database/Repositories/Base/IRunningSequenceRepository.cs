using HB.Database.DbModels;
using Strateq.Core.Database.Repositories.Base;

namespace HB.Database.Repositories
{
    public interface IRunningSequenceRepository : IRepositoryBase<RunningSequenceNumber>
    {
        Task<RunningSequenceNumber> FindByIdAsync(int id);
        IQueryable<RunningSequenceNumber> ToQueryable();
    }
}