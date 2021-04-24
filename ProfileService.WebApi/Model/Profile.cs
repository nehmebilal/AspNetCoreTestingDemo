namespace ProfileService.WebApi.Model
{
    public record Profile
    {
        public string Username { get; init; }

        public PersonalInfo PersonalInfo { get; init; }
        public string? EmployerName { get; init; }
    }
}