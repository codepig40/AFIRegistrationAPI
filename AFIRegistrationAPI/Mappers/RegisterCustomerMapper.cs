using AFIRegistrationAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AFIRegistrationAPI.Mappers
{
    public class RegisterCustomerMapper : IRegisterCustomerMapper
    {

        // Map from RegisterCustomer to Customer
        public Customer ToCustomer(RegisterCustomer registerCustomer)
        {
            return new Customer
            {
                CustomerId = registerCustomer.CustomerId,
                CustomerFirstName = registerCustomer.CustomerFirstName,
                CustomerLastName = registerCustomer.CustomerLastName,
                CustomerTitle = registerCustomer.CustomerTitle,
                CustomerDateOfBirth = registerCustomer.CustomerDateOfBirth,
                CustomerEmail = registerCustomer.CustomerEmail
                // Note: PolicyReference doesn't belong in Customer, so it's ignored
            };
        }

        // Map from Customer to RegisterCustomer
        public RegisterCustomer ToRegisterCustomer(Customer customer, string policyReference = "")
        {
            return new RegisterCustomer
            {
                CustomerId = customer.CustomerId,
                CustomerFirstName = customer.CustomerFirstName,
                CustomerLastName = customer.CustomerLastName,
                CustomerTitle = customer.CustomerTitle,
                CustomerDateOfBirth = customer.CustomerDateOfBirth,
                CustomerEmail = customer.CustomerEmail,
                PolicyReference = policyReference
            };
        }
    }
}
