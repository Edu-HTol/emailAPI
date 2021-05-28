using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailingApi.Helper
{
    public class AppSettings
    {
        public string SecurityKey { get; set; }
        public bool RequireHttps { get; set; }
        public int TokenDurationInMinutes { get; set; }
    }
}
