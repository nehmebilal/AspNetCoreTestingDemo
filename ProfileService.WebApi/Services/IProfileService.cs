using System.Threading.Tasks;
using ProfileService.WebApi.Exceptions;
using ProfileService.WebApi.Model;

namespace ProfileService.WebApi.Services
{
    public interface IProfileService
    {
        /// <exception cref="ProfileNotFoundException">is thrown if the profile does not exists</exception>
        /// <exception cref="StorageUnavailableException">the database is unavailable</exception>
        /// <exception cref="EmployerServiceUnavailableException">If the employer service is not reachable</exception>
        Task<FullProfile> GetFullProfile(string username);
        
        /// <exception cref="ProfileNotFoundException">is thrown if the profile does not exists</exception>
        /// <exception cref="StorageUnavailableException">the database is unavailable</exception>
        /// <exception cref="EmployerServiceUnavailableException">If the employer service is not reachable</exception>
        Task<FullProfile> GetFullProfile(Profile profile);

        /// <exception cref="DuplicateProfileException">is thrown if the profile already exists</exception>
        /// <exception cref="StorageUnavailableException">the database is unavailable</exception>
        Task AddProfile(Profile profile);
        
        /// <exception cref="ProfileNotFoundException">is thrown if the profile does not exists</exception>
        /// <exception cref="StorageUnavailableException">the database is unavailable</exception>
        Task UpdateProfile(Profile profile);
        
        /// <exception cref="ProfileNotFoundException">is thrown if the profile does not exists</exception>
        /// <exception cref="StorageUnavailableException">the database is unavailable</exception>
        Task DeleteProfile(string username);
    }
}