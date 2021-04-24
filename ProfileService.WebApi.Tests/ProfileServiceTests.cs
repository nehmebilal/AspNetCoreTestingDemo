using System;
using System.Threading.Tasks;
using ProfileService.WebApi.Exceptions;
using ProfileService.WebApi.Model;
using Xunit;
using ProfileService = ProfileService.WebApi.Services.ProfileService;

namespace ProfileService.WebApi.Tests
{
    public class ProfileServiceTests
    {
        private readonly ProfileStoreStub _profileStoreStub = new();
        private readonly EmployerServiceStub _employerServiceStub = new();

        private readonly EmployerInfo _employerInfo = TestUtils.TestEmployerInfo;
        private readonly PersonalInfo _personalInfo = TestUtils.TestPersonalInfo;
        private readonly Services.ProfileService _profileService;

        public ProfileServiceTests()
        {
            _profileService = new Services.ProfileService(_profileStoreStub, _employerServiceStub);
        }
        
        [Fact]
        public async Task GetFullProfile()
        {
            var profile = new Profile
            {
                Username = "Foo",
                EmployerName = _employerInfo.EmployerName,
                PersonalInfo = _personalInfo
            };
            
            await _profileStoreStub.AddProfile(profile);
            _employerServiceStub.AddEmployer(_employerInfo);

            FullProfile fullProfile = await _profileService.GetFullProfile(profile.Username);
            Assert.Equal(profile.Username, fullProfile.Username);
            Assert.Equal(_personalInfo, fullProfile.PersonalInfo);
            Assert.Equal(_employerInfo, fullProfile.EmployerInfo);
        }

        [Fact]
        public async Task GetFullProfile_EmployerNotFound()
        {
            var profile = new Profile
            {
                Username = "Foo",
                EmployerName = "Bar",
                PersonalInfo = _personalInfo
            };
            
            await _profileStoreStub.AddProfile(profile);
            
            FullProfile fullProfile = await _profileService.GetFullProfile(profile.Username);
            Assert.Equal(profile.Username, fullProfile.Username);
            Assert.Equal(_personalInfo, fullProfile.PersonalInfo);
            Assert.Null(fullProfile.EmployerInfo);
        }

        [Fact]
        public async Task AddProfile()
        {
            var profile = new Profile
            {
                Username = "Foo",
                PersonalInfo = _personalInfo
            };
            
            await _profileService.AddProfile(profile);
            var fullProfile = await _profileService.GetFullProfile(profile.Username);
            Assert.Equal(profile.PersonalInfo, fullProfile.PersonalInfo);
            Assert.Equal(profile.Username, fullProfile.Username);
            Assert.Null(fullProfile.EmployerInfo);
        }
        
        [Fact]
        public async Task UpdateProfile()
        {
            var profile = new Profile
            {
                Username = "Foo",
                PersonalInfo = _personalInfo
            };
            
            await _profileService.AddProfile(profile);

            var updatedPersonalInfo = _personalInfo with {LastName = Guid.NewGuid().ToString()};
            var updatedProfile = profile with {PersonalInfo = updatedPersonalInfo};
            await _profileService.UpdateProfile(updatedProfile);
            
            var fullProfile = await _profileService.GetFullProfile(profile.Username);
            Assert.Equal(updatedPersonalInfo, fullProfile.PersonalInfo);
        }
        
        [Fact]
        public async Task DeleteProfile()
        {
            var profile = new Profile
            {
                Username = "Foo",
                PersonalInfo = _personalInfo
            };
            
            await _profileService.AddProfile(profile);
            Assert.NotNull(await _profileService.GetFullProfile(profile.Username));
            
            await _profileService.DeleteProfile(profile.Username);
            await Assert.ThrowsAsync<ProfileNotFoundException>(() => _profileService.GetFullProfile(profile.Username));
        }
    }
}