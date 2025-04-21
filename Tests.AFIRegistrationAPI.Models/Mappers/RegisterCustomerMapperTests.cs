using AFIRegistrationAPI.Mappers;
using AFIRegistrationAPI.Models;

namespace Tests.AFIRegistrationAPI.Models.Mappers
{
    public class RegisterCustomerMapperTests
    {
        private readonly RegisterCustomerMapper _mapper;

        public RegisterCustomerMapperTests()
        {
            _mapper = new RegisterCustomerMapper();
        }

        [Fact]
        public void Given_RegisterCustomer_When_ToCustomerIsCalled_Then_ShouldMapRegisterCustomerToCustomerCorrectly()
        {
            // Arrange
            var registerCustomer = new RegisterCustomer
            {
                CustomerId = 1,
                CustomerFirstName = "John",
                CustomerLastName = "Doe",
                CustomerTitle = 2,
                CustomerDateOfBirth = "1985-05-15",
                CustomerEmail = "john.doe@example.com",
                PolicyReference = "AB-123456" // Should not be mapped to Customer
            };

            // Act
            var customer = _mapper.ToCustomer(registerCustomer);

            // Assert
            Assert.Equal(registerCustomer.CustomerId, customer.CustomerId);
            Assert.Equal(registerCustomer.CustomerFirstName, customer.CustomerFirstName);
            Assert.Equal(registerCustomer.CustomerLastName, customer.CustomerLastName);
            Assert.Equal(registerCustomer.CustomerTitle, customer.CustomerTitle);
            Assert.Equal(registerCustomer.CustomerDateOfBirth, customer.CustomerDateOfBirth);
            Assert.Equal(registerCustomer.CustomerEmail, customer.CustomerEmail);
        }

        [Fact]
        public void Given_CustomerAndPolicyReference_When_ToRegisterCustomerIsCalled_Then_ShouldMapCustomerToRegisterCustomerCorrectly()
        {
            // Arrange
            var customer = new Customer
            {
                CustomerId = 1,
                CustomerFirstName = "Jane",
                CustomerLastName = "Smith",
                CustomerTitle = 1,
                CustomerDateOfBirth = "1990-07-20",
                CustomerEmail = "jane.smith@example.com"
            };
            var policyReference = "CD-987654";

            // Act
            var registerCustomer = _mapper.ToRegisterCustomer(customer, policyReference);

            // Assert
            Assert.Equal(customer.CustomerId, registerCustomer.CustomerId);
            Assert.Equal(customer.CustomerFirstName, registerCustomer.CustomerFirstName);
            Assert.Equal(customer.CustomerLastName, registerCustomer.CustomerLastName);
            Assert.Equal(customer.CustomerTitle, registerCustomer.CustomerTitle);
            Assert.Equal(customer.CustomerDateOfBirth, registerCustomer.CustomerDateOfBirth);
            Assert.Equal(customer.CustomerEmail, registerCustomer.CustomerEmail);
            Assert.Equal(policyReference, registerCustomer.PolicyReference); // Ensure PolicyReference is mapped
        }

        [Fact]
        public void Given_CustomerWithoutPolicyReference_When_ToRegisterCustomerIsCalled_Then_ShouldMapWithEmptyPolicyReference_WhenNotProvided()
        {
            // Arrange
            var customer = new Customer
            {
                CustomerId = 1,
                CustomerFirstName = "Tom",
                CustomerLastName = "Brown",
                CustomerTitle = 2,
                CustomerDateOfBirth = "1995-11-05",
                CustomerEmail = "tom.brown@example.com"
            };

            // Act
            var registerCustomer = _mapper.ToRegisterCustomer(customer);

            // Assert
            Assert.Equal(customer.CustomerId, registerCustomer.CustomerId);
            Assert.Equal(customer.CustomerFirstName, registerCustomer.CustomerFirstName);
            Assert.Equal(customer.CustomerLastName, registerCustomer.CustomerLastName);
            Assert.Equal(customer.CustomerTitle, registerCustomer.CustomerTitle);
            Assert.Equal(customer.CustomerDateOfBirth, registerCustomer.CustomerDateOfBirth);
            Assert.Equal(customer.CustomerEmail, registerCustomer.CustomerEmail);
            Assert.Equal(string.Empty, registerCustomer.PolicyReference); // Ensure PolicyReference is empty when not passed
        }
    }
}