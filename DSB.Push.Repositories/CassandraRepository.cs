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
        
        public async Task<PushUser?> GetUser(string userId)
        {
            var ps = _session.Prepare(
                $"SELECT customer_id, device_token" +
                $"FROM users " +
                $"WHERE customer_id = ?");
            var statement = ps.Bind(userId);
            var rs = await _session.ExecuteAsync(statement).ContinueWith(t =>
            {
                if (!t.IsCompletedSuccessfully)
                {
                    throw new OperationCanceledException();
                }

                return t;
            });

            var tokens = rs.Result.Select(x => new DeviceToken { Token = x.GetValue<string>("device_token") }).ToArray();
            if (tokens.Length > 0)
            {
                return new PushUser { Id = userId, DeviceTokens = new List<DeviceToken>(tokens) };
            }

            // No data
            return null;
        }
    }
}