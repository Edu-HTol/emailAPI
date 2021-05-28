using MailingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailingApi.Interfaces
{
    public interface IAuthenticationLayer
    {
        public int RegisterUser(BusinessModelUser user);

        public BusinessModelUser GetUser(string name);
        public int RegisterUser(string name, string hashPassword);
    }
}
