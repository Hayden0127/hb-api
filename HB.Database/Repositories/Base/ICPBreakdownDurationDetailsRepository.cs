using Strateq.Core.Database.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HB.Database.DbModels;
using Microsoft.EntityFrameworkCore;

namespace HB.Database.Repositories
{
    public interface ICPBreakdownDurationDetailsRepository : IRepositoryBase<CPBreakdownDurationDetails>
    {
        Task<List<CPBreakdownDurationDetails>> GetAllAsync();

        Task<CPBreakdownDurationDetails> FindByIdAsync(int id);

        IQueryable<CPBreakdownDurationDetails> ToQueryable();
    }
}
