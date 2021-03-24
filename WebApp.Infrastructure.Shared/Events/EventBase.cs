using System;

namespace WebApp.Infrastructure.Shared.Events
{
    public abstract class EventBase
    {
        public Guid EventId { get; } = Guid.NewGuid();

        public DateTime Created { get; } = DateTime.UtcNow;
    }
}
