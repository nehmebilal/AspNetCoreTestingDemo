using System.Threading.Tasks;
using ProfileService.WebApi.Model;

namespace ProfileService.WebApi.Services
{
    public class EmployerService : IEmployerService
    {
        public Task<EmployerInfo?> FindEmployer(string employerName)
        {
            throw new System.NotImplementedException();
        }
    }
}