using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cassandra;
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
            IEnumerable<DeviceToken> customers;
            
            try
            {
                customers = _pushDataRepository.GetAllCustomersTokens();
            }
            catch (NotImplementedException)
            {
                return StatusCode(StatusCodes.Status501NotImplemented);
            }
            catch (Exception)
            {
                // @todo: perhaps logging? 
                // log e.Message
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(customers);
        }

        /// <summary>
        /// Gets a customer's token given a customer ID 
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("get/{customerId}")]
        public async Task<ActionResult<PushCustomer?>> Get(int customerId)
        {
            PushCustomer? customer;
            try
            {
                customer = await _pushDataRepository.GetCustomer(customerId);
            }
            catch (Exception e)
            {
                // @todo: perhaps logging?
                // log e.Message
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return Ok(customer);
        }
    }
}