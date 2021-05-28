using MailingApi.Layers;
using MailingApi.Models;
using MailingApiTests.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailingApiTests
{
    [TestClass]
    public class DataLayerTests
    {
        static private DataLayer _data;
        static private MailingApiContext _context;
        [ClassInitialize]
        public static void Initialize(TestContext _)
        {
            var options = new DbContextOptionsBuilder<MailingApiContext>().UseInMemoryDatabase(databaseName: "DataLayerTests")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
            _context = new MailingApiContext(options);
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            _data = new DataLayer(_context, factory);
            var owner = DataLayerTestHelper.CreateMailUser("name", "password");
            _context.GroupOwners.Add(owner);
            _context.SaveChanges();
            var groups = new List<MailingGroup>()
            {
                DataLayerTestHelper.CreateMailingGroup("name1", owner.Id),
                DataLayerTestHelper.CreateMailingGroup("name2", owner.Id),
                DataLayerTestHelper.CreateMailingGroup("nameToDelete3", owner.Id),
                DataLayerTestHelper.CreateMailingGroup("nameToDelete4", owner.Id),
             };
            _context.Groups.AddRange(groups);
            _context.SaveChanges();
            var consumerList = new List<MailConsumer>();
            foreach (var g in groups)
            {
                consumerList.Add(DataLayerTestHelper.CreateMailConsumer($"name{g.Id}", g.Id));
            }
            _context.Consumers.AddRange(consumerList);
            _context.SaveChanges();
        }

        #region Select
        [DataRow(1)]
        [TestMethod]
        public void SelectGroupsByOwnerIdReturnGroups(int ownerId)
        {
            var actualGroups = _data.SelectGroupsByOwnerId(ownerId) as List<MailingGroup>;
            Assert.IsNotNull(actualGroups);
            Assert.AreEqual(ownerId, actualGroups[0].GroupOwnerId);
            Assert.AreNotEqual(0, actualGroups.Count);
        }

        [DataRow(-1)]
        [DataRow(10)]
        [TestMethod]
        public void SelectGroupsByOwnerIdReturnEmptyList(int ownerId)
        {
            var actualGroups = _data.SelectGroupsByOwnerId(ownerId) as List<MailingGroup>;
            Assert.IsNotNull(actualGroups);
            Assert.AreEqual(0, actualGroups.Count);
        }

        [DataRow(1)]
        [DataRow(2)]
        [TestMethod]
        public void SelectGroupByIdShouldReturnGroup(int groupId)
        {
            var actualGroup = _data.SelectGroupById(groupId);
            Assert.IsNotNull(actualGroup);
            Assert.AreEqual(groupId, actualGroup.Id);
        }
        [DataRow(-1)]
        [DataRow(10)]
        [TestMethod]
        public void SelectGroupByIdShouldReturnNull(int groupId)
        {
            var actualGroup = _data.SelectGroupById(groupId);
            Assert.IsNull(actualGroup);
        }

        [DataRow(1)]
        [TestMethod]
        public void SelectOwnerByIdShouldReturnOwner(int ownerId)
        {
            var actualOwner = _data.SelectOwnerById(ownerId);
            Assert.IsNotNull(actualOwner);
            Assert.AreEqual(ownerId, actualOwner.Id);
        }

        [DataRow(-1)]
        [DataRow(10)]
        [TestMethod]
        public void SelectOwnerByIdShouldReturnNull(int ownerId)
        {
            var owner = _data.SelectOwnerById(ownerId);
            Assert.IsNull(owner);
        }

        [DataRow(1)]
        [DataRow(2)]
        [TestMethod]
        public void SelectConsumerByGroupIdShouldReturnConsumers(int groupId)
        {
            var actualConsumers = _data.SelectConsumersByGroupId(groupId) as List<MailConsumer>;
            Assert.IsNotNull(actualConsumers);
            Assert.AreEqual(actualConsumers[0].GroupId, groupId);
        }

        [DataRow(-1)]
        [DataRow(10)]
        [TestMethod]
        public void SelectConsumerByGroupIdShouldReturnEmptyList(int groupId)
        {
            var actualConsumers = _data.SelectConsumersByGroupId(groupId) as List<MailConsumer>;
            Assert.IsNotNull(actualConsumers);
            Assert.AreEqual(0, actualConsumers.Count);
        }
        #endregion
        #region Insert
        [TestMethod]
        public void InsertGroupShouldInsertGroup()
        {
            var expectedGroup = DataLayerTestHelper.CreateMailingGroup("insertedName", 1);
            var expectedConsumer = DataLayerTestHelper.CreateMailConsumer("a@insterted.com", 0);
            var consumers = new List<MailConsumer>()
            {
                expectedConsumer
            };

            var inserted = _data.InsertGroup(expectedGroup, consumers);
            var insertedGroup = _context.Groups.Where(x => x.Id == inserted).FirstOrDefault();
            var insertedConsumer = _context.Consumers.Where(x => x.GroupId == inserted).FirstOrDefault();
            Assert.AreNotEqual(-1, inserted);
            Assert.IsNotNull(insertedGroup);
            Assert.IsNotNull(insertedConsumer);
            Assert.AreEqual(expectedGroup.GroupOwnerId, insertedGroup.GroupOwnerId);
            Assert.AreEqual(expectedGroup.Name, insertedGroup.Name);
            Assert.AreEqual(expectedConsumer.ConsumerAddress, insertedConsumer.ConsumerAddress);
            Assert.AreEqual(true, insertedGroup.Id == insertedConsumer.GroupId);
        }

        #endregion

        #region Delete
        [DataRow(3)]
        [DataRow(4)]
        [TestMethod]
        public void DeleteGroupShouldDeleteGroup(int groupId)
        {
            var deleted = _data.DeleteGroup(groupId);
            var actualGroup = _context.Groups.Where(x => x.Id == groupId).FirstOrDefault();
            var actualConsumers = _context.Consumers.Where(x => x.GroupId == groupId).FirstOrDefault();
            Assert.IsTrue(deleted);
            Assert.IsNull(actualGroup);
            Assert.IsNull(actualConsumers);
        }

        [DataRow(10)]
        [DataRow(-1)]
        [TestMethod]
        public void DeleteGroupShouldReturnFalse(int groupId)
        {
            var deleted = _data.DeleteGroup(groupId);
            Assert.IsFalse(deleted);
        }
        #endregion

        #region Update
        [DataRow(1)]
        [DataRow(2)]
        [TestMethod]
        public void UpdateGroupShouldUpdateGroup(int groupId)
        {
            var expectedGroup = DataLayerTestHelper.CreateMailingGroup("updatedName", 1);
            var expectedConsumer = DataLayerTestHelper.CreateMailConsumer("a@updated.com", groupId);
            var consumers = new List<MailConsumer>()
            {
                expectedConsumer
            };
            var update = _data.UpdateGroup(groupId, expectedGroup, consumers);
            var actualGroup = _context.Groups.Where(x => x.Id == groupId).FirstOrDefault();
            var actualConsumer = _context.Consumers.Where(x => x.GroupId == groupId).FirstOrDefault();
            Assert.IsTrue(update);
            Assert.AreEqual(expectedGroup.GroupOwnerId, actualGroup.GroupOwnerId);
            Assert.AreEqual(expectedGroup.Name, actualGroup.Name);
            Assert.AreEqual(expectedConsumer.ConsumerAddress, actualConsumer.ConsumerAddress);
            Assert.AreEqual(expectedConsumer.Id, actualConsumer.Id);
            Assert.AreEqual(expectedConsumer.GroupId, actualConsumer.GroupId);

        }

        [DataRow(10)]
        [DataRow(-1)]
        [TestMethod]
        public void UpdateGroupShouldNotUpdateGroup(int groupId)
        {
            var expectedGroup = DataLayerTestHelper.CreateMailingGroup("updatedName", 1);
            var expectedConsumers = new List<MailConsumer>()
            {
                DataLayerTestHelper.CreateMailConsumer("updatedConsumer", 1)
            };
            var update = _data.UpdateGroup(groupId, expectedGroup, expectedConsumers);
            var actualGroup = _context.Groups.Where(x => x.Id == groupId).FirstOrDefault();
            var actualConsumers = _context.Consumers.Where(x => x.GroupId == groupId).FirstOrDefault();
            Assert.IsFalse(update);
            Assert.IsNull(actualGroup);
            Assert.IsNull(actualConsumers);
        }
        #endregion
    }
}
