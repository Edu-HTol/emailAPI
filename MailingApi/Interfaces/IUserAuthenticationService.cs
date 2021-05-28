using MailingApi.Layers;
using MailingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailingApi.Interfaces
{
    public interface IUserAuthenticationService
    {
        BusinessModelUser Authenticate(string username, string password);

        bool Register(string username, string password);
    }
}
