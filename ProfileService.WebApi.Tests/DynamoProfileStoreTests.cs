using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Options;
using Moq;
using ProfileService.WebApi.Exceptions;
using ProfileService.WebApi.Model;
using ProfileService.WebApi.Storage;
using Xunit;

namespace ProfileService.WebApi.Tests
{
    public class DynamoProfileStoreTests
    {
        private readonly Mock<IAmazonDynamoDB> _dynamoDbClientMock;
        private readonly DynamoProfileStore _profileStore;

        public DynamoProfileStoreTests()
        {
            _dynamoDbClientMock = new Mock<IAmazonDynamoDB>();
            var options = new OptionsWrapper<DynamoProfileStoreSettings>(
                new DynamoProfileStoreSettings
                {
                    TableName = "Unused"
                });

            _profileStore = new DynamoProfileStore(_dynamoDbClientMock.Object, options);
        }
        
        [Fact]
        public async Task AmazonDynamoDbExceptionIsHandled()
        {
            _dynamoDbClientMock.Setup(m => m.GetItemAsync(It.IsAny<GetItemRequest>(), default))
                .ThrowsAsync(new AmazonDynamoDBException("Fake Exception"));
            _dynamoDbClientMock.Setup(m => m.PutItemAsync(It.IsAny<PutItemRequest>(), default))
                .ThrowsAsync(new AmazonDynamoDBException("Fake Exception"));
            _dynamoDbClientMock.Setup(m => m.DeleteItemAsync(It.IsAny<DeleteItemRequest>(), default))
                .ThrowsAsync(new AmazonDynamoDBException("Fake Exception"));

            await Assert.ThrowsAsync<StorageUnavailableException>(() => _profileStore.AddProfile(new Profile()));
            await Assert.ThrowsAsync<StorageUnavailableException>(() => _profileStore.UpdateProfile(new Profile()));
            await Assert.ThrowsAsync<StorageUnavailableException>(() => _profileStore.GetProfile("username"));
            await Assert.ThrowsAsync<StorageUnavailableException>(() => _profileStore.DeleteProfile("username"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Invalid JSON")]
        public async Task GetProfileHandlesUnexpectedPayload(string? payload)
        {
            _dynamoDbClientMock.Setup(m => m.GetItemAsync(It.IsAny<GetItemRequest>(), default))
                .ReturnsAsync(new GetItemResponse
                {
                    Item = new Dictionary<string, AttributeValue>()
                    {
                        ["username"] = new("foo"),
                        ["payload"] = new(payload)
                    }
                });

            Profile profile = await _profileStore.GetProfile("foo");
            Assert.Equal("foo", profile.Username);
            Assert.Null(profile.PersonalInfo);
            Assert.Null(profile.EmployerName);
        }
        
        [Fact]
        public async Task GetProfileHandlesMissingPayload()
        {
            _dynamoDbClientMock.Setup(m => m.GetItemAsync(It.IsAny<GetItemRequest>(), default))
                .ReturnsAsync(new GetItemResponse
                {
                    Item = new Dictionary<string, AttributeValue>()
                    {
                        ["username"] = new("foo"),
                    }
                });

            Profile profile = await _profileStore.GetProfile("foo");
            Assert.Equal("foo", profile.Username);
            Assert.Null(profile.PersonalInfo);
            Assert.Null(profile.EmployerName);
        }
    }
}