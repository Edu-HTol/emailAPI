using MailingApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MailingApiTests.Helper
{
    public static class DataLayerTestHelper
    {
        public static MailUser CreateMailUser(string username, string password)
        {
            return new MailUser
            {
                Username = username,
                Password = password,
            };
        }

        public static MailingGroup CreateMailingGroup(string name, int ownerId)
        {
            return new MailingGroup
            {
                Name = name,
                GroupOwnerId = ownerId,
            };
        }

        public static MailConsumer CreateMailConsumer(string address, int groupId)
        {
            return new MailConsumer
            {
                ConsumerAddress = address,
                GroupId = groupId
            };
        }
    }
}
