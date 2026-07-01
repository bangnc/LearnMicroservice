using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Persistence.Configurations
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; }
        public string UserRegisteredTopic { get; set; }
    }
}
