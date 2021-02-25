using System;
using DSB.Push.Api.Controllers;
using DSB.Push.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;

namespace DSB.Push.Api.Tests
{
    public class PushApiTests
    {
        #region Fields
        
        private PushController _controller;
        
        private Mock<IPushDataRepository> _pushDataRepository;
        
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
        [Description("Tests PushController GetAll hs not been implemented")]
        public void TestPushControllerGetAllReturnsBadRequestNotImplemented()
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