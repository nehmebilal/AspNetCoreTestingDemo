using System.Threading.Tasks;
using ProfileService.WebApi.Exceptions;
using ProfileService.WebApi.Model;

namespace ProfileService.WebApi.Services
{
    public interface IEmployerService
    {
        /// <returns>The employer info or null if the employer is not found</returns>
        /// <exception cref="EmployerServiceUnavailableException">If the employer service is not reachable</exception>
        Task<EmployerInfo?> FindEmployer(string employerName);
    }
}