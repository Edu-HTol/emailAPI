using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailingApi.Interfaces
{
    interface IUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
