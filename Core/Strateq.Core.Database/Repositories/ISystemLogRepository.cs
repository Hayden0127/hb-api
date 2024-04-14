
using Strateq.Core.Database.DbModel;
using System.Threading.Tasks;

namespace Strateq.Core.Database.Repositories
{
    public interface ISystemLogRepository
    {
        Task<SystemLog> AddAndSaveChangesAsync(SystemLog entity);

    }
}
