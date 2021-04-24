using System.Collections.Generic;
using System.Threading.Tasks;
using Nito.AsyncEx;
using ProfileService.WebApi.Exceptions;
using ProfileService.WebApi.Model;
using ProfileService.WebApi.Storage;

namespace ProfileService.WebApi.Tests
{
    public class ProfileStoreStub : IProfileStore
    {
        private readonly Dictionary<string, Profile> _profiles = new();
        private readonly AsyncLock _asyncLock = new();
        
        public async Task<Profile> GetProfile(string username)
        {
            using (await _asyncLock.LockAsync())
            {
                if (!_profiles.TryGetValue(username, out Profile? profile))
                {
                    throw new ProfileNotFoundException(username);
                }
                return profile;
            }
        }

        public async Task AddProfile(Profile profile)
        {
            using (await _asyncLock.LockAsync())
            {
                if (!_profiles.TryAdd(profile.Username, profile))
                {
                    throw new DuplicateProfileException(profile.Username);
                }
            }
        }

        public async Task UpdateProfile(Profile profile)
        {
            using (await _asyncLock.LockAsync())
            {
                if (!_profiles.ContainsKey(profile.Username))
                {
                    throw new ProfileNotFoundException(profile.Username);
                }
                _profiles[profile.Username] = profile;
            }
        }

        public async Task DeleteProfile(string username)
        {
            using (await _asyncLock.LockAsync())
            {
                if (!_profiles.ContainsKey(username))
                {
                    throw new ProfileNotFoundException(username);
                }
                _profiles.Remove(username);
            }
        }
    }
}