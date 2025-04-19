using AFIRegistrationAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace AFIRegistrationAPI.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DatabaseContext _context;
        public CustomerRepository(DatabaseContext context) 
        { 
        
            _context = context;

        }

        public async Task<List<Customer>> GetCustomersAsync() 
        {
            return await _context.Customers.ToListAsync();
        
        }


        public Task<string> TESTAsync(int id)
        {
            return Task.FromResult("Test Customer");

        }

        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            try
            {
                await  _context.Customers.AddAsync(customer);
                await  _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //$"Failed to Create Customer";
            }

            return customer;

        }


    }
}
