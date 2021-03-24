using WebApp.Infrastructure.Shared.Discovery;
using WebApp.Infrastructure.Shared.EntityFramework;
using System.Linq;

namespace WebApp.Infrastructure.Shared.Repositories
{
    public interface IRepository<T, TId> : IScopedService where T : class, IEntity
    {
        IQueryable<T> Get();

        T Get(TId id);

        T Add(T item);

        T Update(TId id, T item);

        void Remove(T item);
    }
}
