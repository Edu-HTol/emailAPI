using MailingApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailingApiTests.Helper
{
    public static class BusinessTestHelper
    {

        public static BusinessModelGroup CreateBusinessModelGroup(int userId, string groupName, IEnumerable<string> emails)
        {
            return new BusinessModelGroup()
            {
                GroupName = groupName,
                GroupOwnerId = userId,
                Emails = emails.Select(x => new BusinessModelEmails { Email = x }).ToList(),
            };
        }

        public static BusinessModelUser CreateBusinessModelUser(string username, string password)
        {
            return new BusinessModelUser()
            {
                Username = username,
                Password = password,
            };
        }

        public static void CompareBusinessModelGroup(BusinessModelGroup expected, BusinessModelGroup actual)
        {
            var actualEmails = expected.Emails as List<BusinessModelEmails>;
            var expectedEmails = actual.Emails as List<BusinessModelEmails>;
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.GroupName, actual.GroupName);
            Assert.AreEqual(expected.GroupOwnerId, actual.GroupOwnerId);
            Assert.AreEqual(expectedEmails.Count, actualEmails.Count);
            Assert.AreEqual(expectedEmails[0].Email, actualEmails[0].Email);
        }
    }
}
