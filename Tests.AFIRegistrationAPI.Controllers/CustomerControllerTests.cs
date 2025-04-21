using AFIRegistrationAPI.Controllers;
using AFIRegistrationAPI.Mappers;
using AFIRegistrationAPI.Models;
using AFIRegistrationAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.AFIRegistrationAPI.Controllers
{
    public class CustomerControllerTests
    {
        private readonly Mock<ICustomerRepository> _customerRepoMock;
        private readonly Mock<IPolicyRepository> _policyRepoMock;
        private readonly Mock<IRegisterCustomerMapper> _mapperMock;
        private readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _customerRepoMock = new Mock<ICustomerRepository>();
            _policyRepoMock = new Mock<IPolicyRepository>();
            _mapperMock = new Mock<IRegisterCustomerMapper>();

            _controller = new CustomerController(
                _customerRepoMock.Object,
                _policyRepoMock.Object,
                _mapperMock.Object);
        }


        [Fact]
        public async Task Given_CustomersExist_When_GetIsCalled_Then_ReturnsOkWithCustomers()
        {
            // Arrange
            var customers = new List<Customer> { new Customer { CustomerId = 1 } };
            _customerRepoMock.Setup(repo => repo.GetCustomersAsync()).ReturnsAsync(customers);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(customers, okResult.Value);
        }

        [Fact]
        public async Task Given_NonexistentPolicyReference_When_RegisterIsCalled_Then_ReturnsValidationProblem()
        {
            // Arrange
            var request = new RegisterCustomer { PolicyReference = "XX-000123" };
            _policyRepoMock.Setup(repo => repo.GetPolicyByReferenceAsync(request.PolicyReference)).ReturnsAsync((Policy)null);

            // Act
            var result = await _controller.Register(request);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(objectResult.Value);

            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode ?? StatusCodes.Status400BadRequest);
            Assert.Contains(nameof(request.PolicyReference), problemDetails.Errors.Keys);
            Assert.Contains(
                $"Policy with reference '{request.PolicyReference}' does not exist.",
                problemDetails.Errors[nameof(request.PolicyReference)][0]);
        }

        [Fact]
        public async Task Given_ValidRegisterRequest_When_RegisterIsCalled_Then_ReturnsCustomerId()
        {
            // Arrange
            var request = new RegisterCustomer { PolicyReference = "XX-000123" };
            var policy = new Policy { PolicyReference = "XX-000123", PolicyId = 99 };

            var customerWithoutId = new Customer(); 
            var customerWithId = new Customer { CustomerId = 42 }; 

            _policyRepoMock
                .Setup(repo => repo.GetPolicyByReferenceAsync(request.PolicyReference))
                .ReturnsAsync(policy);

            _mapperMock
                .Setup(mapper => mapper.ToCustomer(request))
                .Returns(customerWithoutId);

            _customerRepoMock
                .Setup(repo => repo.AddCustomerAsync(customerWithoutId))
                .ReturnsAsync(customerWithId);

            _policyRepoMock
                .Setup(repo => repo.UpdatePolicyAsync(It.Is<Policy>(p => p.CustomerId == customerWithId.CustomerId)))
                .ReturnsAsync(policy);

            // Act
            var result = await _controller.Register(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(customerWithId.CustomerId, okResult.Value);

            _policyRepoMock.Verify(repo => repo.GetPolicyByReferenceAsync(request.PolicyReference), Times.Once);
            _mapperMock.Verify(mapper => mapper.ToCustomer(request), Times.Once);
            _customerRepoMock.Verify(repo => repo.AddCustomerAsync(customerWithoutId), Times.Once);
            _policyRepoMock.Verify(repo => repo.UpdatePolicyAsync(It.Is<Policy>(p => p.CustomerId == customerWithId.CustomerId)), Times.Once);
        }
    }
}
