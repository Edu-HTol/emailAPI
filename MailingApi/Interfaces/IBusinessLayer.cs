using MailingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailingApi.Interfaces
{
    public interface IBusinessLayer
    {
        public BusinessModelGroup GetBusinessModel(int groupId);

        public IEnumerable<BusinessModelGroup> GetAllBusinessModel(int ownerId);

        public int SaveBusinessModelGroup(BusinessModelGroup model);

        public bool DeleteBusinessModelGroup(int groupId);

        public bool PutBusinessModelGroup(BusinessModelGroup model);
    }
}
