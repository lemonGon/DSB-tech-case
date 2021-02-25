namespace DSB.Push.Api.InternalModels
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

sudo docker network create dsb-cassandra-net
sudo docker run -p 9042:9042 --name dsb_cassandra --network dsb-cassandra-net -d cassandra:latest
sudo docker run -it --rm --network dsb-cassandra-net cassandra:latest cqlsh dsb_cassandra

CREATE KEYSPACE push_data WITH REPLICATION={'class': 'SimpleStrategy', 'replication_factor': 1};
CREATE TABLE push_data.customers (customer_id INT, device_token TEXT, PRIMARY KEY((customer_id), device_token));
exit
*/