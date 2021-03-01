using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DSB.Push.Repositories;
using DSB.Push.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

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
            IEnumerable<DeviceToken> deviceTokens;
            
            try
            {
                deviceTokens = _pushDataRepository.GetAllCustomersTokens();
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

            return Ok(deviceTokens);
        }

        /// <summary>
        /// Gets a customer's token given a customer ID
        /// <param name="customerId">The Customer ID to retrieve</param>
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
        
        /// <summary>
        /// Saves a new customer token: customerId -> DeviceToken
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Route("post/{customerId}")]
        public async Task<ActionResult<PushCustomer?>> Post(int customerId, [FromBody] DeviceToken deviceToken)
        {
            PushCustomer? customer;
            
            try
            {
                customer = await _pushDataRepository.SaveCustomer(customerId, deviceToken);
            }

            catch (Exception)
            {
                // @todo: perhaps logging? 
                // log e.Message
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            // @todo inject through HttpClient 
            return Created("http://localhost", customer);
        }
    }
}