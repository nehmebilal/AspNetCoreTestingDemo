using ProfileService.WebApi.Model;

namespace ProfileService.WebApi.Tests
{
    public static class TestUtils
    {
        public static readonly EmployerInfo TestEmployerInfo = new()
        {
            EmployerName = "Earnin",
            MailingAddress = new MailingAddress
            {
                Street = "260 Sheridan Avenue",
                Addressee = "Earnin",
                Secondary = "Suite 300",
                City = "Palo Alto",
                State = "CA",
                Zipcode = "94306"
            },
            PhoneNumber = "111-111-1111" // not a real number
        };

        public static readonly PersonalInfo TestPersonalInfo = new()
        {
            Email = "foo.bar@earnin.com",
            FirstName = "Foo",
            LastName = "Bar",
            PhoneNumber = "222-222-2222",
            MailingAddress = new MailingAddress
            {
                Street = "123 Street",
                Addressee = "Foo Bar",
                Street2 = "North",
                Secondary = "Suite 500",
                City = "City",
                State = "State",
                Zipcode = "12345"
            }
        };
    }
}