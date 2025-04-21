using AFIRegistrationAPI.Mappers;
using AFIRegistrationAPI.Models;
using AFIRegistrationAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AFIRegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        ICustomerRepository _customerRepository;
        IPolicyRepository _policyRepository;

        IRegisterCustomerMapper _registerCustomerMapper;

        public CustomerController(ICustomerRepository customerRepository, IPolicyRepository policyRepository, IRegisterCustomerMapper registerCustomerMapper)
        {
            _customerRepository = customerRepository;
            _policyRepository = policyRepository;
            _registerCustomerMapper = registerCustomerMapper;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var customers = await _customerRepository.GetCustomersAsync();

            return Ok(customers);
        }



        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterCustomer registerCustomer)
        {
            var policy = await _policyRepository.GetPolicyByReferenceAsync(registerCustomer.PolicyReference);

            if (policy == null) 
            {

                var errors = new ModelStateDictionary(); 
                errors.AddModelError(nameof(registerCustomer.PolicyReference), $"Policy with reference '{registerCustomer.PolicyReference}' does not exist.");

                return ValidationProblem(errors);
            }


              // populate newCustomer
              Customer newCustomer = _registerCustomerMapper.ToCustomer(registerCustomer);

            var customer = await _customerRepository.AddCustomerAsync(newCustomer);
            policy.CustomerId = customer.CustomerId;
            policy = await _policyRepository.UpdatePolicyAsync(policy);


            return Ok(customer.CustomerId); 
        }

    }
}
