namespace ProfileService.WebApi.Model
{
    public record MailingAddress
    {
        public string Addressee { get; init; }
        public string Street { get; init; }
        public string Street2 { get; init; }
        public string Secondary { get; init; }
        public string City { get; init; }
        public string State { get; init; }
        public string Zipcode { get; init; }
    }
}