namespace ProfileService.WebApi.Model
{
    public record EmployerInfo
    {
        public string EmployerName { get; init; }

        public string PhoneNumber { get; init; }
        
        public MailingAddress MailingAddress { get; init; }
    }
}