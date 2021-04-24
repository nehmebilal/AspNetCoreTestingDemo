using System.Threading.Tasks;
using ProfileService.WebApi.Model;
using ProfileService.WebApi.Storage;

namespace ProfileService.WebApi.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileStore _profileStore;
        private readonly IEmployerService _employerService;

        public ProfileService(IProfileStore profileStore, IEmployerService employerService)
        {
            _profileStore = profileStore;
            _employerService = employerService;
        }

        public async Task<FullProfile> GetFullProfile(string username)
        {
            var profile = await _profileStore.GetProfile(username);
            return await GetFullProfile(profile);
        }

        public async Task<FullProfile> GetFullProfile(Profile profile)
        {
            EmployerInfo? employerInfo = null;
            
            if (!string.IsNullOrWhiteSpace(profile.EmployerName))
            {
                employerInfo = await _employerService.FindEmployer(profile.EmployerName);
            }
            
            return new FullProfile
            {
                Username = profile.Username,
                PersonalInfo = profile.PersonalInfo,
                EmployerInfo = employerInfo
            };
        }

        public Task AddProfile(Profile profile)
        {
            return _profileStore.AddProfile(profile);
        }

        public Task UpdateProfile(Profile profile)
        {
            return _profileStore.UpdateProfile(profile);
        }

        public Task DeleteProfile(string username)
        {
            return _profileStore.DeleteProfile(username);
        }
    }
}