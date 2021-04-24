using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ProfileService.WebApi.Exceptions;
using ProfileService.WebApi.Model;

namespace ProfileService.WebApi.Storage
{
    public class DynamoProfileStore : IProfileStore
    {
        private const string UsernameAttribute = "username";
        private const string PayloadAttribute = "payload";

        private readonly IAmazonDynamoDB _dynamoDb;
        private readonly IOptions<DynamoProfileStoreSettings> _options;

        public DynamoProfileStore(IAmazonDynamoDB dynamoDb, IOptions<DynamoProfileStoreSettings> options)
        {
            _dynamoDb = dynamoDb;
            _options = options;
        }

        private string TableName => _options.Value.TableName;

        public async Task<Profile> GetProfile(string username)
        {
            try
            {
                GetItemRequest request = NewGetItemRequest(username);
                GetItemResponse response = await _dynamoDb.GetItemAsync(request);
                if (response == null || !response.IsItemSet)
                    throw new ProfileNotFoundException(username);

                return DeserializeProfile(username, response);
            }
            catch (AmazonDynamoDBException e)
            {
                throw new StorageUnavailableException(
                    $"Could not get profile for user {username} from storage", e);
            }
        }

        public async Task AddProfile(Profile profile)
        {
            PutItemRequest request = NewPutItemRequest(profile);
            request.ConditionExpression = "attribute_not_exists(username)"; // add or fail (no update)

            try
            {
                await _dynamoDb.PutItemAsync(request);
            }
            catch (ConditionalCheckFailedException)
            {
                throw new DuplicateProfileException(profile.Username);
            }
            catch (AmazonDynamoDBException e)
            {
                throw new StorageUnavailableException(
                    $"Could not add profile for user {profile.Username} to storage", e);
            }
        }

        public async Task UpdateProfile(Profile profile)
        {
            PutItemRequest request = NewPutItemRequest(profile);

            try
            {
                await _dynamoDb.PutItemAsync(request);
            }
            catch (AmazonDynamoDBException e)
            {
                throw new StorageUnavailableException(
                    $"Could not put profile for user {profile.Username} to storage", e);
            }
        }

        public async Task DeleteProfile(string username)
        {
            DeleteItemRequest request = NewDeleteItemRequest(username);
            request.ConditionExpression = "attribute_exists(username)"; // add or fail (no update)

            try
            {
                await _dynamoDb.DeleteItemAsync(request);
            }
            catch (ConditionalCheckFailedException)
            {
                throw new ProfileNotFoundException(username);
            }
            catch (AmazonDynamoDBException e)
            {
                throw new StorageUnavailableException(
                    $"Could not delete profile for user {username} from storage", e);
            }
        }

        private DeleteItemRequest NewDeleteItemRequest(string username)
        {
            return new()
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>()
                {
                    {UsernameAttribute, new AttributeValue {S = username}},
                }
            };
        }

        private GetItemRequest NewGetItemRequest(string username)
        {
            return new()
            {
                TableName = TableName,
                Key = new Dictionary<string, AttributeValue>()
                {
                    {UsernameAttribute, new AttributeValue {S = username}},
                },
                ConsistentRead = true
            };
        }

        private static Profile DeserializeProfile(string username, GetItemResponse response)
        {
            var payload = GetPayload(response);
            if (payload == null)
            {
                return new Profile {Username = username};
            }

            try
            {
                return JsonConvert.DeserializeObject<Profile>(payload);
            }
            catch (JsonReaderException)
            {
                return new Profile {Username = username};
            }
        }

        private PutItemRequest NewPutItemRequest(Profile profile)
        {
            return new()
            {
                TableName = TableName,
                Item = new Dictionary<string, AttributeValue>()
                {
                    {UsernameAttribute, new AttributeValue {S = profile.Username}},
                    {PayloadAttribute, new AttributeValue {S = JsonConvert.SerializeObject(profile)}},
                }
            };
        }

        private static string? GetPayload(GetItemResponse response)
        {
            if (!response.Item.TryGetValue(PayloadAttribute, out AttributeValue? payload))
            {
                return null;
            }

            return payload?.S;
        }
    }
}