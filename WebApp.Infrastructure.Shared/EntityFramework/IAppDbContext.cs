using System;
using System.Collections.Generic;
using System.Text;
using WebApp.Infrastructure.Shared.Discovery;

namespace WebApp.Infrastructure.Shared.EntityFramework
{
    public interface IAppDbContext : IDbContext, IScopedService
    {

    }
}
