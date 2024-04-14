using HB.Database.DbModels;
using Strateq.Core.Database.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Database.Repositories
{
    public interface ICPBreakdownErrorRepository : IRepositoryBase<CPBreakdownError>
    {
        Task<List<CPBreakdownError>> GetAllAsync();
        Task<CPBreakdownError> FindByIdAsync(int id);
        IQueryable<CPBreakdownError> ToQueryable();
    }
}
