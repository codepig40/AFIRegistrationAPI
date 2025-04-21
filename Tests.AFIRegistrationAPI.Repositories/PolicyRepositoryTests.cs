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
    public class PolicyRepositoryTests
    {
        private DbContextOptions<DatabaseContext> GetInMemoryOptions()
        {
            return new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{System.Guid.NewGuid()}")
                .Options;
        }

        private async Task SeedData(DatabaseContext context)
        {
            context.Policies.AddRange(
                new Policy { PolicyId = 1, CustomerId = 1, PolicyReference = "XX-000123" },
                new Policy { PolicyId = 2, CustomerId = 2, PolicyReference = "XX-000124" },
                new Policy { PolicyId = 3, CustomerId = 1, PolicyReference = "XX-000125" }
            );
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task Given_NewPolicy_When_AddPolicyAsync_Then_PolicyIsAdded()
        {
            // Given
            var options = GetInMemoryOptions();
            using var context = new DatabaseContext(options);
            var repository = new PolicyRepository(context);
            var newPolicy = new Policy { PolicyReference = "XX-000126" };

            // When
            var result = await repository.AddPolicyAsync(newPolicy);

            // Then
            Assert.Equal("XX-000126", result.PolicyReference);
        }

        [Fact]
        public async Task Given_SeededPolicies_When_GetPoliciesAsync_Then_ReturnsAllPolicies()
        {
            // Given
            var options = GetInMemoryOptions();
            using var context = new DatabaseContext(options);
            await SeedData(context);
            var repository = new PolicyRepository(context);

            // When
            var policies = await repository.GetPoliciesAsync();

            // Then
            Assert.Equal(3, policies.Count);
        }

        [Fact]
        public async Task Given_CustomerId_When_GetPoliciesByCustomerAsync_Then_ReturnsOnlyCustomerPolicies()
        {
            // Given
            var options = GetInMemoryOptions();
            using var context = new DatabaseContext(options);
            await SeedData(context);
            var repository = new PolicyRepository(context);

            // When
            var customerPolicies = await repository.GetPoliciesByCustomerAsync(1);

            // Then
            Assert.Equal(2, customerPolicies.Count);
            Assert.All(customerPolicies, p => Assert.Equal(1, p.CustomerId));
        }

        [Fact]
        public async Task Given_PolicyId_When_GetPolicyByIdAsync_Then_ReturnsCorrectPolicy()
        {
            // Given
            var options = GetInMemoryOptions();
            using var context = new DatabaseContext(options);
            await SeedData(context);
            var repository = new PolicyRepository(context);

            // When
            var policy = await repository.GetPolicyByIdAsync(2);

            // Then
            Assert.NotNull(policy);
            Assert.Equal("XX-000124", policy.PolicyReference);
        }

        [Fact]
        public async Task Given_PolicyReference_When_GetPolicyByReferenceAsync_Then_ReturnsCorrectPolicy()
        {
            // Given
            var options = GetInMemoryOptions();
            using var context = new DatabaseContext(options);
            await SeedData(context);
            var repository = new PolicyRepository(context);

            // When
            var policy = await repository.GetPolicyByReferenceAsync("XX-000125");

            // Then
            Assert.NotNull(policy);
            Assert.Equal(3, policy.PolicyId);
        }

        [Fact]
        public async Task Given_UpdatedPolicy_When_UpdatePolicyAsync_Then_PolicyIsUpdated()
        {
            // Given
            var options = GetInMemoryOptions();
            using var context = new DatabaseContext(options);
            await SeedData(context);
            var repository = new PolicyRepository(context);

            var policyToUpdate = await context.Policies.FirstAsync(p => p.PolicyId == 1);
            policyToUpdate.CustomerId = 3;

            // When
            await repository.UpdatePolicyAsync(policyToUpdate);

            // Then
            var updatedPolicy = await context.Policies.FirstAsync(p => p.PolicyId == 1);
            Assert.Equal(3, updatedPolicy.CustomerId);
        }
    }
}
