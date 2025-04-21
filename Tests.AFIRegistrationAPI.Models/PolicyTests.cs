using AFIRegistrationAPI.Models;

namespace Tests.AFIRegistrationAPI.Models
{
    public class PolicyTests
    {

        [Fact]
        public void Given_PolicyProperties_When_SetValues_Then_PropertiesShouldReturnCorrectValues()
        {
            // Arrange
            var policy = new Policy();

            // Act
            policy.PolicyId = 101;
            policy.PolicyReference = "XX-000123";
            policy.IsActive = 1;
            policy.CustomerId = 2001;

            // Assert
            Assert.Equal(101, policy.PolicyId);
            Assert.Equal("XX-000123", policy.PolicyReference);
            Assert.Equal(1, policy.IsActive);
            Assert.Equal(2001, policy.CustomerId);
        }

        [Fact]
        public void Given_PolicyReference_When_Set_Then_PolicyReferenceShouldNotBeNull()
        {
            // Arrange
            var policy = new Policy
            {
                PolicyReference = "XX-000123"
            };

            // Act & Assert
            Assert.NotNull(policy.PolicyReference);
            Assert.Equal("XX-000123", policy.PolicyReference);
        }

        [Fact]
        public void Given_Policy_When_CustomerIdIsSetToNull_Then_CustomerIdShouldAllowNull()
        {
            // Arrange
            var policy = new Policy
            {
                CustomerId = null
            };

            // Act & Assert
            Assert.Null(policy.CustomerId);
        }

        [Fact]
        public void Given_Policy_When_DefaultConstructorIsUsed_Then_ShouldSetDefaultValues()
        {
            // Arrange
            var policy = new Policy();

            // Act & Assert
            Assert.Equal(0, policy.PolicyId);
            Assert.Null(policy.PolicyReference);
            Assert.Equal(0, policy.IsActive);
            Assert.Null(policy.CustomerId);
        }
    }
}