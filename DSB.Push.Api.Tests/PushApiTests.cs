using System;
using System.Collections.Generic;
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
        
        private readonly PushCustomer _pushCustomer = new()
        {
            Id = 1,
            DeviceTokens = new List<DeviceToken>
            {
                new() { Token = "abcd" }
            }
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
            ).ReturnsAsync(() => _pushCustomer);

            var action = await _controller.Get(_pushCustomer.Id);
            var result = action.Result as OkObjectResult;
            
            Assert.IsNotNull(result);
            
            
            var customerId = ((PushCustomer)result.Value).Id;
            var customerDeviceTokens = ((PushCustomer)result.Value).DeviceTokens;
            
            Assert.AreEqual(result.StatusCode, StatusCodes.Status200OK);
            Assert.AreEqual(customerId, _pushCustomer.Id);
            Assert.IsInstanceOf<IEnumerable<DeviceToken>>(customerDeviceTokens);
       }
        
        [Test]
        [Description("Tests PushController -> Get(customer_id) returns null., which means no customers found with that ID")]
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