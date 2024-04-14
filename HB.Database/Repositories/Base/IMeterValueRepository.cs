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
    public interface IMeterValueRepository : IRepositoryBase<MeterValue>
    {
        Task<List<MeterValue>> GetAllAsync();

        Task<MeterValue> FindByIdAsync(int id);

        IQueryable<MeterValue> ToQueryable();
    }
}
