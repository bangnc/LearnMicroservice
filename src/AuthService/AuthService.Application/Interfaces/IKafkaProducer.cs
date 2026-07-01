using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces
{
    public interface IKafkaProducer
    {
        Task PublishAsync<T>(
            string topic,
            T message,
            CancellationToken cancellationToken = default);
    }
}
