using Newtonsoft.Json;

namespace ProfileService.WebApi.Model
{
    public record PersonalInfo
    {
        public string FirstName { get; init; }
        
        public string LastName { get; init; }
        
        public string Email { get; init; }
        
        public string PhoneNumber { get; init; }
        
        public MailingAddress MailingAddress { get; init; }
    }
}