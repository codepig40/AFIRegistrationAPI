using AFIRegistrationAPI.Models;

namespace Tests.AFIRegistrationAPI.Models
{
    public class CustomerTests
    {
        [Fact]
        public void Given_Customer_When_PropertiesAreSet_Then_ShouldGetAndSetCorrectly()
        {
            // Arrange
            var customer = new Customer();

            // Act
            customer.CustomerId = 1;
            customer.CustomerFirstName = "John";
            customer.CustomerLastName = "Doe";
            customer.CustomerTitle = 1;
            customer.CustomerDateOfBirth = "1980-01-01";
            customer.CustomerEmail = "john.doe@example.com";

            // Assert
            Assert.Equal(1, customer.CustomerId);
            Assert.Equal("John", customer.CustomerFirstName);
            Assert.Equal("Doe", customer.CustomerLastName);
            Assert.Equal(1, customer.CustomerTitle);
            Assert.Equal("1980-01-01", customer.CustomerDateOfBirth);
            Assert.Equal("john.doe@example.com", customer.CustomerEmail);
        }

        [Fact]
        public void Given_Customer_When_FirstNameIsSet_Then_ShouldNotBeNull()
        {
            // Arrange & Act
            var customer = new Customer();
            customer.CustomerFirstName = "Alice";

            // Assert
            Assert.NotNull(customer.CustomerFirstName);
        }

        [Fact]
        public void Given_Customer_When_LastNameIsSet_Then_ShouldNotBeNull()
        {
            // Arrange & Act
            var customer = new Customer();
            customer.CustomerLastName = "Smith";

            // Assert
            Assert.NotNull(customer.CustomerLastName);
        }

        [Fact]
        public void Given_Customer_When_EmailIsNotSet_Then_ShouldBeNullable()
        {
            // Arrange
            var customer = new Customer();

            // Act & Assert
            Assert.Null(customer.CustomerEmail);
        }

        [Fact]
        public void Given_Customer_When_DateOfBirthIsNotSet_Then_ShouldBeNullable()
        {
            // Arrange
            var customer = new Customer();

            // Act & Assert
            Assert.Null(customer.CustomerDateOfBirth);
        }
    }
}