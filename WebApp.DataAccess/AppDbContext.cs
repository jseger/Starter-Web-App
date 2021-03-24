using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using WebApp.Domain.Shared.Entities;
using WebApp.Infrastructure.Shared.EntityFramework;

namespace WebApp.DataAccess
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        private readonly ILoggerFactory loggerFactory;
        public AppDbContext(DbContextOptions<AppDbContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            this.loggerFactory = loggerFactory;
        }

        public DbSet<CustomerEntity> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(loggerFactory);

            base.OnConfiguring(optionsBuilder);
        }
    }
}
