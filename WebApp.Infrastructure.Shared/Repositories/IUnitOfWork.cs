using System.Threading.Tasks;
using WebApp.Infrastructure.Shared.Discovery;

namespace WebApp.Infrastructure.Shared.Repositories
{
    public interface IUnitOfWork : IScopedService
    {
        Task SaveChangesAsync();

    }
}
