using Microsoft.Extensions.DependencyInjection;
using ProfileService.WebApi.Services;
using ProfileService.WebApi.Storage;
using Xunit;

namespace ProfileService.WebApi.Tests
{
    public class DiTests
    {
        [Fact]
        public void AllDependenciesAreRegistered()
        {
            var services = Program.CreateHostBuilder().Build().Services;
            
            services.GetRequiredService<IProfileService>();
            services.GetRequiredService<IEmployerService>();
            services.GetRequiredService<IProfileStore>();
        }
    }
}