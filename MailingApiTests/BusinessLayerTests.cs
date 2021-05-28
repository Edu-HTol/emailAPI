using MailingApi.Controllers;
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
using System.Text;

namespace MailingApiTests
{
    [TestClass]
    public class BusinessLayerTests
    {
        private static DataLayer _data;
        private static BusinessLayer _buisness;

        [ClassInitialize]
        public static void Initialize(TestContext _)
        {
            var options = new DbContextOptionsBuilder<MailingApiContext>().UseInMemoryDatabase(databaseName: "BusinessLayerTests")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
            var context = new MailingApiContext(options);
            var owner = DataLayerTestHelper.CreateMailUser("name", "password");
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            _data = new DataLayer(context, factory);
            _buisness = new BusinessLayer(_data);
            _data.InsertUser(owner);
            var groups = new List<MailingGroup>()
            {
                DataLayerTestHelper.CreateMailingGroup("name1", owner.Id),
                DataLayerTestHelper.CreateMailingGroup("name2", owner.Id),
                DataLayerTestHelper.CreateMailingGroup("nameToDelete3", owner.Id),
                DataLayerTestHelper.CreateMailingGroup("nameToDelete4", owner.Id),
             };
            foreach (var g in groups)
            {
                var consumers = new List<MailConsumer>()
                {
                    DataLayerTestHelper.CreateMailConsumer("a@a.com", g.Id),
                };
                _data.InsertGroup(g, consumers);
            }
        }
        [DataRow(1)]
        [DataRow(2)]
        [TestMethod]
        public void GetBusinessModelShouldReturnBusinessModel(int groupId)
        {
            var expectedModel = _data.SelectGroupById(groupId);
            var expectedConsumers = _data.SelectConsumersByGroupId(groupId) as List<MailConsumer>;
            var expectedConsumer = expectedConsumers[0];
            var actualModel =  _buisness.GetBusinessModel(groupId);
            var actualConsumers = actualModel.Emails  as List<BusinessModelEmails>;
            var actualConsumer = actualConsumers[0];
            Assert.IsNotNull(actualModel);
            Assert.AreEqual(groupId, actualModel.Id);
            Assert.AreEqual(expectedModel.Name, actualModel.GroupName);
            Assert.AreEqual(expectedModel.GroupOwnerId, actualModel.GroupOwnerId);
            Assert.AreEqual(expectedConsumers.Count, actualConsumers.Count);
            Assert.AreEqual(expectedConsumer.ConsumerAddress, actualConsumer.Email);
        }

        [DataRow(1)]
        [TestMethod]
        public void GetAllBusinessModelShouldReturnGroup(int ownerId)
        {
            var expectedModels = _data.SelectGroupsByOwnerId(ownerId) as List<MailingGroup>;
            var expectedModel = expectedModels[0];
            var expectedConsumers = _data.SelectConsumersByGroupId(expectedModel.Id) as List<MailConsumer>;
            var expectedConsumer = expectedConsumers[0];
            var actualModels = _buisness.GetAllBusinessModel(ownerId) as List<BusinessModelGroup>;
            var actualModel = actualModels[0];
            var actualConsumers = actualModel.Emails as List<BusinessModelEmails>;
            var actualConsumer = actualConsumers[0];
            Assert.IsNotNull(actualModel);
            Assert.AreEqual(ownerId, actualModel.GroupOwnerId);
            Assert.AreEqual(expectedModel.Name, actualModel.GroupName);
            Assert.AreEqual(expectedModel.GroupOwnerId, actualModel.GroupOwnerId);
            Assert.AreEqual(expectedConsumers.Count, actualConsumers.Count);
            Assert.AreEqual(expectedConsumer.ConsumerAddress, actualConsumer.Email);
        }

        [DataRow(10)]
        [DataRow(-1)]
        [TestMethod]
        public void GetAllBusinessModelShouldReturnEmptyList(int ownerId)
        {
            var actualModels = _buisness.GetAllBusinessModel(ownerId) as List<BusinessModelGroup>;
            Assert.AreEqual(0, actualModels.Count);
        }

        [DataRow(10)]
        [DataRow(-1)]
        [TestMethod]
        public void GetBusinessModelShouldReturnNull(int groupId)
        {
            var model = _buisness.GetBusinessModel(groupId);
            Assert.IsNull(model);
        }
        [TestMethod]
        public void SaveBusinessModelGroupShouldSaveModel()
        {
            var emails = new List<string>() { "inserted@inserted.com" };
            var expectedModel = BusinessTestHelper.CreateBusinessModelGroup(1, "insertedName", emails);
            var modelId = _buisness.SaveBusinessModelGroup(expectedModel);
            expectedModel.Id = modelId;
            var actualGroup = _data.SelectGroupById(modelId);
            var actualConsumers = _data.SelectConsumersByGroupId(actualGroup.Id);
            var actualModel = new BusinessModelGroup(actualConsumers, actualGroup);
            Assert.AreNotEqual(-1, modelId);
            BusinessTestHelper.CompareBusinessModelGroup(expectedModel, actualModel);
        }

        [DataRow(3)]
        [DataRow(4)]
        [TestMethod]
        public void DeleteBusinessModelGroupShouldDeleteGroup(int groupId)
        {
            var result = _buisness.DeleteBusinessModelGroup(groupId);
            var actualGroup = _data.SelectGroupById(groupId);
            var actualConsumers = _data.SelectConsumersByGroupId(groupId) as List<MailConsumer>;
            Assert.IsTrue(result);
            Assert.IsNull(actualGroup);
            Assert.AreEqual(0, actualConsumers.Count);
        }

        [DataRow(1)]
        [DataRow(2)]
        [TestMethod]
        public void PutBusinessModelGroupShouldUpdateModel(int groupId)
        {
            var emails = new List<string>() { "updated@updated.com" };
            var expectedModel = BusinessTestHelper.CreateBusinessModelGroup(1, "updatedName", emails);
            expectedModel.Id = groupId;
            var result = _buisness.PutBusinessModelGroup(expectedModel);
            var actualGroup = _data.SelectGroupById(groupId);
            var actualConsumers = _data.SelectConsumersByGroupId(groupId) as List<MailConsumer>;
            var actualModel = new BusinessModelGroup(actualConsumers, actualGroup);
            Assert.IsTrue(result);
            BusinessTestHelper.CompareBusinessModelGroup(expectedModel, actualModel);
        }
    }
}
