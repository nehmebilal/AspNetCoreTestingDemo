using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ProfileService.WebApi.Exceptions;
using ProfileService.WebApi.Model;
using ProfileService.WebApi.Storage;
using Xunit;

namespace ProfileService.WebApi.Tests
{
    public class ProfileStoreIntegrationTests : IAsyncLifetime
    {
        private readonly IProfileStore _profileStore;
        private readonly Profile _profile;

        public ProfileStoreIntegrationTests()
        {
            var services = Program.CreateHostBuilder().Build().Services;
            _profileStore = services.GetRequiredService<IProfileStore>();
            
            var username = Guid.NewGuid().ToString(); // should be random to avoid race conditions between tests
            _profile = new Profile
            {
                Username = username,
                PersonalInfo = TestUtils.TestPersonalInfo
            };
        }
        
        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            try
            {
                await _profileStore.DeleteProfile(_profile.Username); // cleanup the store
            }
            catch (ProfileNotFoundException)
            {
                // profile was already deleted in the test
            }
        }

        [Fact]
        public async Task AddGetProfile()
        {
            await _profileStore.AddProfile(_profile);
            var storedProfile = await _profileStore.GetProfile(_profile.Username);
            Assert.Equal(_profile, storedProfile);
        }

        [Fact]
        public async Task AddProfileThatAlreadyExistsThrows()
        {
            await _profileStore.AddProfile(_profile);
            await Assert.ThrowsAsync<DuplicateProfileException>(() => _profileStore.AddProfile(_profile));
        }

        [Fact]
        public async Task GetNonExistingProfileThrows()
        {
            await Assert.ThrowsAsync<ProfileNotFoundException>(() => _profileStore.GetProfile(_profile.Username));
        }

        [Fact]
        public async Task DeleteProfile()
        {
            await _profileStore.AddProfile(_profile);
            await _profileStore.DeleteProfile(_profile.Username);
            await GetNonExistingProfileThrows();
        }

        [Fact]
        public async Task DeleteNonExistingProfile()
        {
            await Assert.ThrowsAsync<ProfileNotFoundException>(() => _profileStore.DeleteProfile(_profile.Username));
        }
    }
}