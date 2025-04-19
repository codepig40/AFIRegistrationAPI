using AFIRegistrationAPI.Models;

namespace AFIRegistrationAPI.Repositories
{
    public interface ICustomerRepository
    {

        Task<string> TESTAsync(int id);

        Task<Customer> AddCustomerAsync(Customer customer);
        Task<List<Customer>> GetCustomersAsync();
    }
}
