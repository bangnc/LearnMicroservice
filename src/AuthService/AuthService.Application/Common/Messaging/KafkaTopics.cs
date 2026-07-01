using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Common.Messaging
{
    public static class KafkaTopics
    {
        public const string SendOtp = "send-otp";

        public const string UserRegistered = "user-registered";

        public const string UserLoggedIn = "user-logged-in";

        public const string AuditLog = "audit-log";
    }
}
