using AFIRegistrationAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AFIRegistrationAPI.Mappers
{
    public interface IRegisterCustomerMapper 
    {

        // Map from RegisterCustomer to Customer
        Customer ToCustomer(RegisterCustomer registerCustomer);


        // Map from Customer to RegisterCustomer
        RegisterCustomer ToRegisterCustomer(Customer customer, string policyReference = "");
    }
}
