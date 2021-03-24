using System.Threading.Tasks;
using WebApp.Infrastructure.Shared.EntityFramework;
using WebApp.Infrastructure.Shared.Repositories;

namespace WebApp.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IAppDbContext dbContext;

        public UnitOfWork(IAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task SaveChangesAsync()
        {
            await this.dbContext.SaveChangesAsync();
        }
    }
}
