namespace ProfileService.WebApi.Model
{
    public record FullProfile
    {
        public string Username { get; init; }

        public PersonalInfo PersonalInfo { get; init; }

        public EmployerInfo? EmployerInfo { get; init; }
    }
}