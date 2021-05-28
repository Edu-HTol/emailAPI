using MailingApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailingApi.Interfaces
{
    public interface IDataLayer
    {
        public MailingGroup SelectGroupById(int groupId);
        public IEnumerable<MailingGroup> SelectGroupsByOwnerId(int ownerId);
        public MailUser SelectOwnerById(int ownerId);
        public IEnumerable<MailConsumer> SelectConsumersByGroupId(int groupId);
        public MailUser SelectUser(string username);
        public int InsertGroup(MailingGroup group, IEnumerable<MailConsumer> consumers);
        public bool DeleteGroup(int groupId);
        public bool UpdateGroup(int groupId, MailingGroup newGroup, IEnumerable<MailConsumer> newConsumers);
        public int InsertUser(MailUser owner);
    }
}
