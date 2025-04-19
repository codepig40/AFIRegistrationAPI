using AFIRegistrationAPI.Models;

namespace AFIRegistrationAPI.Repositories
{
    public interface IPolicyRepository
    {

        Task<string> TESTAsync(int id);
        Task<Policy> AddPolicyAsync(Policy policy);

        Task<List<Policy>> GetPoliciesAsync();

        Task<Policy> GetPolicyByIdAsync(int id);

        Task<List<Policy>> GetPoliciesByCustomerAsync(int id);

        Task<Policy> GetPolicyByReferenceAsync(string policyReference);

        Task<Policy> UpdatePolicyAsync(Policy policy);
    }
}
