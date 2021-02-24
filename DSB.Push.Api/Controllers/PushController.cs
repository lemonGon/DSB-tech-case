using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DSB.Push.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PushController : ControllerBase
    {
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("get")]
        public IActionResult GetAll()
        {
            return Ok("lorem ipsum");
        }
    }
}