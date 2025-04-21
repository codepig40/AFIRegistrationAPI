using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using AFIRegistrationAPI.Models;
using Moq;
using Microsoft.EntityFrameworkCore;
using AFIRegistrationAPI.Repositories;

namespace Tests.AFIRegistrationAPI.Repositories
{
    public class CustomerRepositoryTests
    {
        // Helper method to set up a mock DbContext
        private static Mock<DatabaseContext> GetMockDbContext()
        {
            var mockContext = new Mock<DatabaseContext>();
            var mockSet = new Mock<DbSet<Customer>>();

            // Set up any needed DbSet methods using Moq
            mockContext.Setup(c => c.Customers).Returns(mockSet.Object);

            return mockContext;
        }

        [Fact]
        public async Task Given_CustomersExist_When_GetCustomersAsync_Then_ReturnsAllCustomers()
        {
            // Given
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{System.Guid.NewGuid()}")
                .Options;

            using var context = new DatabaseContext(options);
            context.Customers.AddRange(
                new Customer { CustomerId = 1, CustomerFirstName = "Customer1", CustomerLastName = "Customer1" },
                new Customer { CustomerId = 2, CustomerFirstName = "Customer2", CustomerLastName = "Customer2" }
            );
            await context.SaveChangesAsync();

            var repository = new CustomerRepository(context);

            // When
            var result = await repository.GetCustomersAsync();

            // Then
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Customer1", result[0].CustomerFirstName);
            Assert.Equal("Customer2", result[1].CustomerFirstName);
            Assert.Equal("Customer1", result[0].CustomerLastName);
            Assert.Equal("Customer2", result[1].CustomerLastName);
        }

        [Fact]
        public async Task Given_ValidCustomer_When_AddCustomerAsync_Then_AddsCustomerSuccessfully()
        {
            // Given
            var mockContext = GetMockDbContext();
            var customerToAdd = new Customer { CustomerFirstName = "NewCustomer", CustomerLastName = "NewCustomer" };
            var mockSet = new Mock<DbSet<Customer>>();

            mockContext.Setup(c => c.Customers).Returns(mockSet.Object);
            var repository = new CustomerRepository(mockContext.Object);

            // When
            var result = await repository.AddCustomerAsync(customerToAdd);

            // Then
            Assert.NotNull(result);
            Assert.Equal("NewCustomer", result.CustomerFirstName);
            Assert.Equal("NewCustomer", result.CustomerLastName);
            mockSet.Verify(m => m.AddAsync(It.IsAny<Customer>(), default), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Given_DatabaseThrowsException_When_AddCustomerAsync_Then_ThrowsException()
        {
            // Given
            var mockContext = GetMockDbContext();
            var customerToAdd = new Customer { CustomerFirstName = "NewCustomer", CustomerLastName = "NewCustomer" };
            var mockSet = new Mock<DbSet<Customer>>();

            mockContext.Setup(c => c.Customers).Returns(mockSet.Object);
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database exception"));

            var repository = new CustomerRepository(mockContext.Object);

            // When & Then
            var exception = await Assert.ThrowsAsync<Exception>(() => repository.AddCustomerAsync(customerToAdd));
            Assert.Equal("Database exception", exception.Message);

            mockSet.Verify(m => m.AddAsync(It.IsAny<Customer>(), default), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }
    }
}