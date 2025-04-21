using AFIRegistrationAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace Tests.AFIRegistrationAPI.Models
{
    public class RegisterCustomerTests
    {
        private List<ValidationResult> ValidateModel(RegisterCustomer model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            Validator.TryValidateObject(model, context, results, validateAllProperties: true);
            return results;
        }

        [Fact]
        public void Given_RegisterCustomerWithMinimumRequiredFieldsEmailOnly_When_ValidateModelIsCalled_Then_ShouldBeValid()
        {
            var model = new RegisterCustomer
            {
                CustomerId = 1,
                CustomerFirstName = "Alice",
                CustomerLastName = "Smith",
                CustomerTitle = 1,
                PolicyReference = "AB-123456",
                CustomerEmail = "alice1234@email.com" // satisfies EmailOrDOB
            };

            var results = ValidateModel(model);
            Assert.Empty(results);
        }

        [Fact]
        public void Given_RegisterCustomerWithMinimumRequiredFieldsDobOnly_When_ValidateModelIsCalled_Then_ShouldBeValid()
        {
            var model = new RegisterCustomer
            {
                CustomerId = 1,
                CustomerFirstName = "Alice",
                CustomerLastName = "Smith",
                CustomerTitle = 1,
                PolicyReference = "AB-123456",
                CustomerDateOfBirth = "01/01/2001" // satisfies EmailOrDOB
            };

            var results = ValidateModel(model);
            Assert.Empty(results);
        }

        [Fact]
        public void Given_RegisterCustomerWithShortFirstName_When_ValidateModelIsCalled_Then_ShouldReturnFirstNameLengthError()
        {
            var model = new RegisterCustomer
            {
                CustomerFirstName = "Al", // too short
                CustomerLastName = "Smith",
                CustomerTitle = 1,
                PolicyReference = "AB-123456",
                CustomerEmail = "test1234@email.com"
            };

            var results = ValidateModel(model);
            Assert.Contains(results, r => r.ErrorMessage!.Contains("First name must be between"));
        }

        [Fact]
        public void Given_RegisterCustomerWithShortLastName_When_ValidateModelIsCalled_Then_ShouldReturnLastNameLengthError()
        {
            var model = new RegisterCustomer
            {
                CustomerFirstName = "Alice",
                CustomerLastName = "Jo", // too short
                CustomerTitle = 1,
                PolicyReference = "AB-123456",
                CustomerEmail = "test1234@email.com"
            };

            var results = ValidateModel(model);
            Assert.Contains(results, r => r.ErrorMessage!.Contains("Surname must be between"));
        }

        [Theory]
        [InlineData("abc@email")]         // invalid format
        [InlineData("ab@em.com")]         // <4 characters before @
        [InlineData("abcd@e.com.net")]    // invalid domain
        public void Given_InvalidEmail_When_ValidateModelIsCalled_Then_ShouldReturnInvalidEmailFormatError(string email)
        {
            var model = new RegisterCustomer
            {
                CustomerFirstName = "Alice",
                CustomerLastName = "Smith",
                CustomerTitle = 1,
                PolicyReference = "AB-123456",
                CustomerEmail = email
            };

            var results = ValidateModel(model);
            Assert.Contains(results, r => r.ErrorMessage!.Contains("Invalid email format"));
        }

        [Theory]
        [InlineData("2024-01-01")] // underage
        [InlineData("01/01/2024")] // underage in dd/MM/yyyy
        public void Given_RegisterCustomerWithUnderageDateOfBirth_When_ValidateModelIsCalled_Then_ShouldReturnUnder18Error(string dob)
        {
            var model = new RegisterCustomer
            {
                CustomerFirstName = "Alice",
                CustomerLastName = "Smith",
                CustomerTitle = 1,
                CustomerDateOfBirth = dob,
                PolicyReference = "AB-123456"
            };

            var results = ValidateModel(model);
            Assert.Contains(results, r => r.ErrorMessage!.Contains("at least 18 years old"));
        }

        [Theory]
        [InlineData("1990-05-20")]
        [InlineData("20/05/1990")]
        public void Given_RegisterCustomerWithValidAdultDateOfBirth_When_ValidateModelIsCalled_Then_ShouldNotReturnUnder18Error(string dob)
        {
            var model = new RegisterCustomer
            {
                CustomerFirstName = "Bob",
                CustomerLastName = "Brown",
                CustomerTitle = 1,
                CustomerDateOfBirth = dob,
                PolicyReference = "CD-987654"
            };

            var results = ValidateModel(model);
            Assert.DoesNotContain(results, r => r.ErrorMessage!.Contains("at least 18 years old"));
        }

        [Fact]
        public void Given_RegisterCustomerWithoutEmailOrDOB_When_ValidateModelIsCalled_Then_ShouldReturnEmailOrDOBRequiredError()
        {
            var model = new RegisterCustomer
            {
                CustomerFirstName = "Jake",
                CustomerLastName = "Long",
                CustomerTitle = 1,
                PolicyReference = "EF-111111"
                // Missing both Email and DOB
            };

            var results = ValidateModel(model);
            Assert.Contains(results, r => r.ErrorMessage!.Contains("Either Email or Date of Birth must be provided"));
        }

        [Theory]
        [InlineData("AA-123456")]
        [InlineData("ZZ-999999")]
        public void Given_ValidPolicyReference_When_ValidateModelIsCalled_Then_ShouldBeValid(string policyRef)
        {
            var model = new RegisterCustomer
            {
                CustomerFirstName = "John",
                CustomerLastName = "Doe",
                CustomerTitle = 1,
                PolicyReference = policyRef,
                CustomerEmail = "john1234@domain.com"
            };

            var results = ValidateModel(model);
            Assert.Empty(results);
        }

        [Theory]
        [InlineData("A-123456")]      // too short
        [InlineData("ABC-123456")]    // too long
        [InlineData("AA-12345")]      // too few digits
        [InlineData("123-123456")]    // no letters
        [InlineData("12-123456")]    // no letters
        [InlineData("12_123456")]    // no dash
        [InlineData("12123456")]    // no dash
        [InlineData("aB-123456")]    // not all caps letters
        public void Given_InvalidPolicyReference_When_ValidateModelIsCalled_Then_ShouldReturnPolicyReferenceFormatError(string policyRef)
        {
            var model = new RegisterCustomer
            {
                CustomerFirstName = "Jane",
                CustomerLastName = "Smith",
                CustomerTitle = 1,
                PolicyReference = policyRef,
                CustomerEmail = "jane1234@domain.com"
            };

            var results = ValidateModel(model);
            Assert.Contains(results, r => r.ErrorMessage!.Contains("Policy Reference must be in format"));
        }
    }
}