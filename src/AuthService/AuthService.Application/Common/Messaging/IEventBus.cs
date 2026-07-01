using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Common.Messaging
{
    public interface IEventBus
    {
        Task PublishAsync<T>(
            string topic,
            T @event,
            CancellationToken cancellationToken = default);
    }
}
