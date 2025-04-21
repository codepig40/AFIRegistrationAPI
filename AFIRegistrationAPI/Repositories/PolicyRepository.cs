using AFIRegistrationAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AFIRegistrationAPI.Repositories
{
    public class PolicyRepository : IPolicyRepository
    {

        private readonly DatabaseContext _context;
        public PolicyRepository(DatabaseContext context)
        {

            _context = context;

        }

        public async Task<Policy> AddPolicyAsync(Policy policy)
        {
            try
            {
                await _context.Policies.AddAsync(policy);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //$"Failed to Create Policy";
            }

            return policy;

        }

        public async Task<List<Policy>> GetPoliciesAsync()
        {
            return await _context.Policies.ToListAsync();
        }

        public async Task<List<Policy>> GetPoliciesByCustomerAsync(int id)
        {
            return await _context.Policies.Where(w => w.CustomerId == id).ToListAsync();
        }

        public async Task<Policy> GetPolicyByIdAsync(int id)
        {
            return await _context.Policies.FirstOrDefaultAsync(f => f.PolicyId == id);
        }

        public async Task<Policy> GetPolicyByReferenceAsync(string policyReference)
        {
            return await _context.Policies.FirstOrDefaultAsync(f => f.PolicyReference == policyReference);
        }

        public async Task<Policy> UpdatePolicyAsync(Policy policy)
        {
            _context.Policies.Update(policy);
            await _context.SaveChangesAsync();

            return policy;
        }

    }
}
