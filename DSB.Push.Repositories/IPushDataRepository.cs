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
        /// <returns>A PushCustomer model CustomerId => customer tokens</returns>
        Task<PushCustomer?> GetCustomer(int customerId);
        
        /// <summary>
        /// Gets all device tokens of all customers
        /// </summary>
        /// <returns>A list of DeviceToken models</returns>
        IEnumerable<DeviceToken> GetAllCustomersTokens();
    }
}