using ProfileService.WebApi.Model;

namespace ProfileService.WebApi.DataContracts
{
    public record CreateProfileRequest
    {
        public string Username { get; set; }
        public PersonalInfo PersonalInfo { get; set; }
        public string EmployerName { get; set; }

        public Profile ToProfile()
        {
            return new()
            {
                Username = Username,
                PersonalInfo = PersonalInfo,
                EmployerName = EmployerName
            };
        }
        
        public bool IsValid(out string? error)
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                error = "Username must not be empty";
                return false;
            }

            if (PersonalInfo == null)
            {
                error = "Missing Personal Info";
                return false;
            }

            // etc

            error = null;
            return true;
        }

        private static bool ValidateField(string fieldName, string fieldValue, out string? error)
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
            {
                error = $"{fieldName} must not be empty";
                return false;
            }

            error = null;
            return true;
        }
    }
}