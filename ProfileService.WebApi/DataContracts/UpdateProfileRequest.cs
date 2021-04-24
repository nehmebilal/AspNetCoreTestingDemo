using ProfileService.WebApi.Model;

namespace ProfileService.WebApi.DataContracts
{
    public class UpdateProfileRequest
    {
        public PersonalInfo PersonalInfo { get; set; }
        public string EmployerName { get; set; }
        
        public Profile ToProfile(string username)
        {
            return new()
            {
                Username = username,
                PersonalInfo = PersonalInfo,
                EmployerName = EmployerName
            };
        }
    }
}