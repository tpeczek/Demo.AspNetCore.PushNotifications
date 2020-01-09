using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebPush = Lib.Net.Http.WebPush;

namespace Demo.AspNetCore.PushNotifications.Services.Cosmos
{
    internal class PushSubscriptionCosmosDbClient : IPushSubscriptionCosmosDbClient
    {
        private class PushSubscription : WebPush.PushSubscription
        {
            [JsonProperty(PropertyName = "id")]
            public string Id { get; set; }

            [JsonProperty(PropertyName = PARTITION_KEY_PROPERTY)]
            public string Partition { get; set; }

            public PushSubscription()
            { }

            public PushSubscription(WebPush.PushSubscription subscription)
            {
                Id = GetId(subscription.Endpoint);
                Partition = PARTITION_KEY_VALUE;
                Endpoint = subscription.Endpoint;
                Keys = subscription.Keys;
            }

            public static string GetId(string endpoint)
            {
                return endpoint.Substring(endpoint.LastIndexOf('/') + 1);
            }
        }

        private const string COSMOSDB_CONFIGURATION_SECTION = "CosmosDB";
        private const string DATABASE_NAME_CONFIGURATION_KEY = "DatabaseName";
        private const string CONTAINER_NAME_CONFIGURATION_KEY = "ContainerName";
        private const string ACCOUNT_ENDPOINT_CONFIGURATION_KEY = "AccountEndpoint";
        private const string AUTH_KEY_CONFIGURATION_KEY = "AuthKey";

        private const string PARTITION_KEY_PROPERTY = "partition";
        private const string PARTITION_KEY_PATH = "/" + PARTITION_KEY_PROPERTY;
        private const string PARTITION_KEY_VALUE = "1";

        private readonly string _databaseName;
        private readonly string _containerName;
        private readonly CosmosClient _cosmosDbClient;
        private Container _subscriptions;
        private readonly PartitionKey _partitionKey = new PartitionKey(PARTITION_KEY_VALUE);

        private Container Subscriptions
        {
            get
            {
                if (_subscriptions is null)
                {
                    _subscriptions = _cosmosDbClient.GetContainer(_databaseName, _containerName);
                }

                return _subscriptions;
            }
        }

        public PushSubscriptionCosmosDbClient(IConfiguration configuration)
        {
            IConfigurationSection cosmosDbConfigurationSection = configuration.GetSection(COSMOSDB_CONFIGURATION_SECTION);

            _databaseName = cosmosDbConfigurationSection.GetSection(DATABASE_NAME_CONFIGURATION_KEY).Value;
            _containerName = cosmosDbConfigurationSection.GetSection(CONTAINER_NAME_CONFIGURATION_KEY).Value;
            string accountEndpoint = cosmosDbConfigurationSection.GetSection(ACCOUNT_ENDPOINT_CONFIGURATION_KEY).Value;
            string authKey = cosmosDbConfigurationSection.GetSection(AUTH_KEY_CONFIGURATION_KEY).Value;

            CosmosClientBuilder clientBuilder = new CosmosClientBuilder(accountEndpoint, authKey);
            _cosmosDbClient = clientBuilder
                .WithConnectionModeDirect()
                .Build();
        }

        public async Task EnsureCreatedAsync()
        {
            DatabaseResponse database = await _cosmosDbClient.CreateDatabaseIfNotExistsAsync(_databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(_containerName, PARTITION_KEY_PATH);
        }

        public Task AddAsync(WebPush.PushSubscription subscription)
        {
            return Subscriptions.CreateItemAsync<PushSubscription>(new PushSubscription(subscription), _partitionKey);
        }

        public Task RemoveAsync(string endpoint)
        {
            return Subscriptions.DeleteItemAsync<PushSubscription>(PushSubscription.GetId(endpoint), _partitionKey);
        }

        public async IAsyncEnumerable<WebPush.PushSubscription> GetAllAsync()
        {
            FeedIterator<PushSubscription> subscriptionsFeedIterator = Subscriptions.GetItemQueryIterator<PushSubscription>(new QueryDefinition("SELECT * FROM c"));
            while (subscriptionsFeedIterator.HasMoreResults)
            {
                FeedResponse<PushSubscription> subscriptionsFeedResponse = await subscriptionsFeedIterator.ReadNextAsync();

                foreach (PushSubscription subscription in subscriptionsFeedResponse)
                {
                    yield return subscription;
                }
            }
        }
    }
}
