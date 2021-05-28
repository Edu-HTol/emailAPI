using MailingApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MailingApi.Models
{
    public class BusinessModelGroup : IDentifier, IGroup
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public IEnumerable<BusinessModelEmails> Emails { get; set; }
        [JsonIgnore]
        public int GroupOwnerId { get; set; }

        public BusinessModelGroup()
        {

        }

        public BusinessModelGroup(IEnumerable<MailConsumer> consumers, MailingGroup group)
        {
            Id = group.Id;
            Emails = consumers.Select(x => new BusinessModelEmails() { Email = x.ConsumerAddress, Id = x.Id }).ToList();
            GroupOwnerId = group.GroupOwnerId;
            GroupName = group.Name;
        }
    }
}
