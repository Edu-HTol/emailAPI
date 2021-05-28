using MailingApi.Interfaces;
using MailingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailingApi.Layers
{
    public class AuthenticationLayer : IAuthenticationLayer
    {
        private readonly IDataLayer _data;
        public AuthenticationLayer(IDataLayer data)
        {
            _data = data;
        }
        public int RegisterUser(BusinessModelUser user)
        {
            var owner = new MailUser()
            {
                Password = user.Password,
                Username = user.Username
            };
            return _data.InsertUser(owner);
        }

        public BusinessModelUser GetUser(string name)
        {
            var user = _data.SelectUser(name);
            if (user != null)
            {
                var model = new BusinessModelUser
                {
                    Id = user.Id,
                    Username = user.Username,
                    Password = user.Password,
                    Role = Helper.Roles.User,
                };
                return model;
            }
            return null;
        }

        public int RegisterUser(string name, string hashPassword)
        {
            var user = new MailUser
            {
                Password = hashPassword,
                Username = name
            };
            return _data.InsertUser(user);
        }

    }
}
