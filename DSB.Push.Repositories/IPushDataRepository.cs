using System.Collections.Generic;
using System.Threading.Tasks;
using DSB.Push.Shared.Models;

namespace DSB.Push.Repositories
{
    public interface IPushDataRepository
    {
        /// <summary>
        /// Gets all device tokens of a customer given a Customer ID
        /// </summary>
        /// <param name="customerId">The Persona ID to get the tokens from</param>
        /// <returns>A PushCustomer model CustomerId => customer tokens or null when not present</returns>
        Task<PushCustomer?> GetCustomer(int customerId);

        /// <summary>
        /// Saves a Customer Token against a customer ID
        /// </summary>
        /// <param name="customerId">The Persona ID to be associated with the token</param>
        /// <param name="deviceToken">The customer token to be saved</param>
        /// <returns>A PushCustomer model CustomerId => customer token</returns>        
        Task<PushCustomer?> SaveCustomer(int customerId, DeviceToken deviceToken);
        
        /// <summary>
        /// Gets all device tokens of all customers
        /// </summary>
        /// <returns>A list of DeviceToken models</returns>
        IEnumerable<DeviceToken> GetAllCustomersTokens();
    }
}