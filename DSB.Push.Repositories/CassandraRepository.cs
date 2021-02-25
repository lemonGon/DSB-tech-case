using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;
using DSB.Push.Shared.Models;

namespace DSB.Push.Repositories
{
    public class CassandraRepository : IPushDataRepository
    {
        private readonly ISession _session;

        public CassandraRepository(ISession session)
        {
            _session = session;
        }
        
        public async Task<PushCustomer?> GetCustomer(int customerId)
        {
            var ps = _session.Prepare(
                $"SELECT customer_id, device_token" +
                $"FROM customers " +
                $"WHERE customer_id = ?");
            var statement = ps.Bind(customerId);
            var rs = await _session.ExecuteAsync(statement).ContinueWith(t =>
            {
                if (!t.IsCompletedSuccessfully)
                {
                    throw new OperationCanceledException();
                }

                return t;
            });
            
            var tokens = rs.Result.Select(
                row => new DeviceToken
                {
                    Token = row.GetValue<string>("device_token")
                }
            ).ToArray();
            
            if (tokens.Length <= 0)
            {
                // No data
                return null;
            }
            
            return new PushCustomer
            {
                Id = customerId, 
                DeviceTokens = new List<DeviceToken>(tokens)
            };
        }

        public IEnumerable<DeviceToken> GetAllCustomersTokens()
        {
            throw new NotImplementedException();
        }
    }
}