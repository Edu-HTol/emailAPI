using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailingApi.Interfaces
{
    interface IGroup
    {
        public string GroupName { get; set; }
        public int GroupOwnerId { get; set; }
    }
}
