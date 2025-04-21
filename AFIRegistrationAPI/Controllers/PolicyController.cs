using AFIRegistrationAPI.Models;
using AFIRegistrationAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AFIRegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {

        IPolicyRepository _policyRepository;

        public PolicyController(IPolicyRepository policyRepository)
        {
            _policyRepository = policyRepository;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var policies = await _policyRepository.GetPoliciesAsync();

            return Ok(policies);
        }

        [HttpGet]
        [Route("GetPolicyById/{id}")]
        public async Task<IActionResult> GetPolicyById(int id)
        {
            var policy = await _policyRepository.GetPolicyByIdAsync(id);

            return Ok(policy);
        }

        [HttpGet]
        [Route("GetPolicyByReference/{policyReference}")]
        public async Task<IActionResult> GetPolicyByReference(string policyReference)
        {
            var policy = await _policyRepository.GetPolicyByReferenceAsync(policyReference);

            return Ok(policy);
        }

        [HttpGet]
        [Route("GetPolicyByCustomerId/{id}")]
        public async Task<IActionResult> GetPolicyByCustomerId(int id)
        {
            var policies = await _policyRepository.GetPoliciesByCustomerAsync(id);

            return Ok(policies);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Policy policy)
        {
            policy = await _policyRepository.AddPolicyAsync(policy);

            return Ok(policy);
        }
    }
}
