using DSB.Push.Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using NUnit.Framework;

namespace DSB.Push.Api.Tests
{
    public class PushApiTests
    {
        private PushController _controller;
        
        [SetUp]
        public void Setup()
        {
            _controller = new PushController();
        }

        [Test]
        public void TestPushControllerGetReturns200()
        {
            var result = _controller.Get() as OkObjectResult;
            
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.AreEqual("lorem ipsum", result.Value);
        }
    }
}