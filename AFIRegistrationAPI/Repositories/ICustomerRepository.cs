using AFIRegistrationAPI.Models;

namespace AFIRegistrationAPI.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> AddCustomerAsync(Customer customer);
        Task<List<Customer>> GetCustomersAsync();
    }
}
