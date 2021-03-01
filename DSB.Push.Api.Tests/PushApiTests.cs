using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DSB.Push.Api.Controllers;
using DSB.Push.Repositories;
using DSB.Push.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;

namespace DSB.Push.Api.Tests
{
    [TestFixture]
    [Description("Controller tests for Push Notification endpoints")]
    public class PushApiTests
    {
        #region Fields
        
        private PushController _controller;
        
        private Mock<IPushDataRepository> _pushDataRepository;
        
        private static readonly PushCustomer PushCustomer = new()
        {
            Id = 1,
            DeviceTokens = new List<DeviceToken>
            {
                new() { Token = "abcd" }
            }
         };

        private static readonly DeviceToken DeviceToken = new()
        {
            Token = "TestToken"
        };
        
        #endregion
        
        
        #region Setup
        
        [SetUp]
        public void Setup()
        {
            _pushDataRepository = new Mock<IPushDataRepository>();
            _controller = new PushController(_pushDataRepository.Object);
        }
        
        #endregion

        
        #region Tests
        
        [Test]
        [Description("Tests PushController -> Get(customer_id) returns customer object")]
        [Category("Get User By ID")]
        public async Task TestPushControllerGetCustomerByIdReturnsCustomer()
        {
            _pushDataRepository.Setup(
                x => x.GetCustomer(It.IsAny<int>())
            ).ReturnsAsync(() => PushCustomer);

            var action = await _controller.Get(PushCustomer.Id);
            var result = action.Result as OkObjectResult;
            
            Assert.IsNotNull(result);
            
            
            var customerId = ((PushCustomer)result.Value).Id;
            var customerDeviceTokens = ((PushCustomer)result.Value).DeviceTokens;
            
            Assert.AreEqual(result.StatusCode, StatusCodes.Status200OK);
            Assert.AreEqual(customerId, PushCustomer.Id);
            Assert.IsInstanceOf<IEnumerable<DeviceToken>>(customerDeviceTokens);
       }
        
        [Test]
        [Description("Tests PushController -> Get(customer_id) returns null., which means no customers were found with that ID")]
        [Category("Get User By ID")]
        public async Task TestPushControllerGetCustomerByIdReturnsNull()
        {
            _pushDataRepository.Setup(
                x => x.GetCustomer(It.IsAny<int>())
            ).ReturnsAsync(() => null);

            var action = await _controller.Get(9999);
            var result = action.Result as OkObjectResult;
            
            Assert.IsNotNull(result);
            
            Assert.AreEqual(result.StatusCode, StatusCodes.Status200OK);
            Assert.IsNull(result.Value);
        }
        
        [Test]
        [Description("Tests PushController -> Post(customer_id, deviceToken) returns 201 with a customer object, which means the returned customer has been saved / updated")]
        [Category("Post customer - device token")]
        public async Task TestPushControllerPostCustomerToken()
        {
            const int customerId = 99999;

            _pushDataRepository.Setup(
                x => x.SaveCustomer(It.IsAny<int>(), It.IsAny<DeviceToken>())
            ).ReturnsAsync(() => new PushCustomer()
            {
                Id = customerId,
                DeviceTokens = new List<DeviceToken>()
                {
                    DeviceToken
                }
            });

            
            
            var action = await _controller.Post(customerId, DeviceToken);
            var result = action.Result as CreatedResult;
            
            Assert.IsNotNull(result);
            
            Assert.AreEqual(result.StatusCode, StatusCodes.Status201Created);
            
            var customerIdResult = ((PushCustomer)result.Value).Id;
            var customerDeviceTokens = ((PushCustomer)result.Value).DeviceTokens;
            
            Assert.AreEqual(customerId, customerIdResult);
            Assert.IsInstanceOf<IEnumerable<DeviceToken>>(customerDeviceTokens);
        }

        [Test]
        [Description("Tests PushController -> Get all customers' tokens functionality has not been implemented")]
        [Category("Get all tokens")]
        public void TestPushControllerGetAllCustomersTokenReturnsBadRequestNotImplemented()
        {
            _pushDataRepository.Setup(
                x => x
                    .GetAllCustomersTokens())
                    .Throws(new NotImplementedException());
            
            var result = _controller.Get().Result as StatusCodeResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status501NotImplemented,result.StatusCode);
        }
        
        #endregion
    }
}