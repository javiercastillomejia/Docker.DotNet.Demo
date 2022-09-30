using Azure;
using Azure.Data.Tables;

namespace Docker.DotNet.Demo
{
    public class Storage
    {
        private readonly TableClient _tableClient;
        private const string ConnectionStrings =
            "DefaultEndpointsProtocol=http;" +
            "AccountName=devstoreaccount1;" +
            "AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;" +
            "BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;" +
            "QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;" +
            "TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
        private const string OurTable = "OurData";

        public Storage()
        {
            _tableClient = new TableClient(ConnectionStrings, OurTable);
        }

        public async Task Create(OurRecord record)
        {
            await _tableClient.CreateIfNotExistsAsync();
            await _tableClient.AddEntityAsync(record);
        }
        public async Task<OurRecord?> Read(string id)
        {
            var records = await _tableClient.QueryAsync<OurRecord>
                    (TableClient.CreateQueryFilter($"RowKey eq {id}"))
                    .ToListAsync();

            return records.FirstOrDefault();
        }
    }

    public class OurRecord : ITableEntity
    {
        public OurRecord(string partitionKey, string rowKey, string team)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            Team = team;
        }

        public OurRecord() { }

        public string Team { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
