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
                $"SELECT customer_id, device_token " +
                $"FROM customers " +
                $"WHERE customer_id = ?");
            var statement = ps.Bind(customerId);
            var rs = await _session.ExecuteAsync(statement).ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully == false)
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
        
        public async Task<PushCustomer?> SaveCustomer(int customerId, DeviceToken deviceToken)
        {
            //@todo maybe check whether a customer already exists and return a different result in that case
            var ps = _session.Prepare(
                $"INSERT INTO customers (" +
                $"customer_id, device_token" +
                $") VALUES (" +
                $" ?, ? " +
                $")" );
            var statement = ps.Bind(customerId, deviceToken.Token);
            var rs = await _session.ExecuteAsync(statement).ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully == false)
                {
                    throw new OperationCanceledException();
                }

                return t;
            });

            
            var customer = new PushCustomer()
            {
                Id = customerId,
                DeviceTokens = new List<DeviceToken>() { deviceToken }
            };
            
            return customer;
        }
        
        public IEnumerable<DeviceToken> GetAllCustomersTokens()
        {
            throw new NotImplementedException();
        }
    }
}