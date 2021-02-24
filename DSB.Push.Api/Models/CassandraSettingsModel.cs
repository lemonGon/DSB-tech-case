namespace DSB.Push.Api.Models
{
    internal class CassandraSettingsModel
    {
        public string KeySpace { get; set; }
        public string ContactPoint { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
    }
}

/*

********************************************
CREATE PUSH SERVICE CASSANDRA
********************************************

sudo docker network create cassandra-net
sudo docker run -p 9042:9042 --name cdb --network cassandra-net -d cassandra:latest
sudo docker run -it --rm --network cassandra-net cassandra:latest cqlsh cdb

CREATE KEYSPACE push_data WITH REPLICATION={'class': 'SimpleStrategy', 'replication_factor': 1};
CREATE TABLE push_data.users (customer_id TEXT, device_token TEXT, PRIMARY KEY((customer_id), device_token));
exit
*/