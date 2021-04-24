using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using ProfileService.WebApi.Model;
using ProfileService.WebApi.Services;

namespace ProfileService.WebApi.Tests
{
    public class EmployerServiceStub : IEmployerService
    {
        private readonly ConcurrentBag<EmployerInfo> _employers;
        
        public EmployerServiceStub(params EmployerInfo[] employers)
        {
            _employers = new ConcurrentBag<EmployerInfo>(employers);
        }

        public void AddEmployer(EmployerInfo employerInfo)
        {
            _employers.Add(employerInfo);
        }
        
        public Task<EmployerInfo?> FindEmployer(string employerName)
        {
            var employerInfo = _employers.SingleOrDefault(employer => employer.EmployerName == employerName);
            return Task.FromResult<EmployerInfo?>(employerInfo);
        }
    }
}