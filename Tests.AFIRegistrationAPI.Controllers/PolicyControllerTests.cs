using AFIRegistrationAPI.Controllers;
using AFIRegistrationAPI.Models;
using AFIRegistrationAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.AFIRegistrationAPI.Controllers
{
    public class PolicyControllerTests
    {
        private readonly Mock<IPolicyRepository> _mockRepo;
        private readonly PolicyController _controller;

        public PolicyControllerTests()
        {
            _mockRepo = new Mock<IPolicyRepository>();
            _controller = new PolicyController(_mockRepo.Object);
        }

        [Fact]
        public async Task Given_PoliciesExist_When_GetIsCalled_Then_ReturnsOkResult_WithListOfPolicies()
        {
            // Arrange
            var policies = new List<Policy> { new Policy(), new Policy() };
            _mockRepo.Setup(repo => repo.GetPoliciesAsync()).ReturnsAsync(policies);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnPolicies = Assert.IsAssignableFrom<IEnumerable<Policy>>(okResult.Value);
            Assert.Equal(2, ((List<Policy>)returnPolicies).Count);
        }

        [Fact]
        public async Task Given_PolicyExists_When_GetPolicyByIdIsCalled_Then_ReturnsOkResult_WithPolicy()
        {
            // Arrange
            var policy = new Policy { PolicyId = 1 };
            _mockRepo.Setup(repo => repo.GetPolicyByIdAsync(1)).ReturnsAsync(policy);

            // Act
            var result = await _controller.GetPolicyById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnPolicy = Assert.IsType<Policy>(okResult.Value);
            Assert.Equal(1, returnPolicy.PolicyId);
        }

        [Fact]
        public async Task Given_PolicyReferenceExists_When_GetPolicyByReferenceIsCalled_Then_ReturnsOkResult_WithPolicy()
        {
            // Arrange
            var policy = new Policy { PolicyReference = "XX-000123" };
            _mockRepo.Setup(repo => repo.GetPolicyByReferenceAsync("XX-000123")).ReturnsAsync(policy);

            // Act
            var result = await _controller.GetPolicyByReference("XX-000123");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnPolicy = Assert.IsType<Policy>(okResult.Value);
            Assert.Equal("XX-000123", returnPolicy.PolicyReference);
        }

        [Fact]
        public async Task Given_CustomerHasPolicies_When_GetPolicyByCustomerIdIsCalled_Then_ReturnsOkResult_WithPolicies()
        {
            // Arrange
            var policies = new List<Policy> { new Policy { CustomerId = 5 } };
            _mockRepo.Setup(repo => repo.GetPoliciesByCustomerAsync(5)).ReturnsAsync(policies);

            // Act
            var result = await _controller.GetPolicyByCustomerId(5);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnPolicies = Assert.IsAssignableFrom<IEnumerable<Policy>>(okResult.Value);
            Assert.Single(returnPolicies);
        }

        [Fact]
        public async Task Given_NewPolicy_When_PostIsCalled_Then_ReturnsOkResult_WithCreatedPolicy()
        {
            // Arrange
            var newPolicy = new Policy { PolicyId = 10 };
            _mockRepo.Setup(repo => repo.AddPolicyAsync(It.IsAny<Policy>())).ReturnsAsync(newPolicy);

            // Act
            var result = await _controller.Post(new Policy());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnPolicy = Assert.IsType<Policy>(okResult.Value);
            Assert.Equal(10, returnPolicy.PolicyId);
        }
    }
}