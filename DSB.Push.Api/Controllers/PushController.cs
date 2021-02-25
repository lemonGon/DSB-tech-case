using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DSB.Push.Repositories;
using DSB.Push.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DSB.Push.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PushController : ControllerBase
    {

        private readonly IPushDataRepository _pushDataRepository;
        public PushController(IPushDataRepository pushDataRepository)
        {
            _pushDataRepository = pushDataRepository;
        }
        
        /// <summary>
        /// Gets all the customers' tokens 
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("get")]
        public ActionResult<IEnumerable<DeviceToken>> Get()
        {
            IEnumerable<DeviceToken> users;
            
            try
            {
                users = _pushDataRepository.GetAllCustomersTokens();
            }
            catch (NotImplementedException e)
            {
                return StatusCode(StatusCodes.Status501NotImplemented);
            }
            catch (Exception e)
            {
                // @todo: perhaps logging? 
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(users);
        }

    }
}