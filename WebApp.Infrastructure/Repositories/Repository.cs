using WebApp.Infrastructure.Shared.EntityFramework;
using WebApp.Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApp.Infrastructure.Repositories
{
    public class Repository<T, TId> : IRepository<T, TId> where T : class, IEntity
    {
        private readonly DbSet<T> dbSet;
        private readonly IAppDbContext dbContext;

        public Repository(IAppDbContext dbContext)
        {
            this.dbSet = dbContext.Set<T>();
            this.dbContext = dbContext;
        }

        public T Add(T item)
        {
            return this.dbSet.Add(item).Entity;
        }

        public IQueryable<T> Get()
        {
            return this.dbSet;
        }

        public T Get(TId id)
        {
            return this.dbSet.Find(id);
        }

        public void Remove(T item)
        {
            this.dbSet.Remove(item);
        }

        public T Update(TId id, T item)
        {
            var existing = this.dbSet.Find(id);

            this.dbContext.Entry(existing).CurrentValues.SetValues(item);

            return existing;
        }
    }
}
