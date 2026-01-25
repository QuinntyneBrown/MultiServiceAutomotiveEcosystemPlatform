using Customer.Core.Entities;
using FluentAssertions;
using Xunit;

namespace Customer.Api.Tests;

public class CustomerTests
{
    private readonly Guid _tenantId = Guid.NewGuid();

    [Fact]
    public void Create_ValidInput_CreatesCustomer()
    {
        // Arrange & Act
        var customer = new Core.Entities.Customer(
            _tenantId,
            "test@example.com",
            "555-1234",
            "John",
            "Doe");

        // Assert
        customer.Id.Should().NotBeEmpty();
        customer.TenantId.Should().Be(_tenantId);
        customer.Email.Should().Be("test@example.com");
        customer.FirstName.Should().Be("John");
        customer.LastName.Should().Be("Doe");
        customer.Phone.Should().Be("555-1234");
        customer.Status.Should().Be(Shared.Contracts.Enums.CustomerStatus.Active);
    }

    [Fact]
    public void Create_EmptyTenantId_ThrowsException()
    {
        // Arrange & Act
        var act = () => new Core.Entities.Customer(
            Guid.Empty,
            "test@example.com",
            null,
            "John",
            "Doe");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Tenant*");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_EmptyEmail_ThrowsException(string email)
    {
        // Arrange & Act
        var act = () => new Core.Entities.Customer(
            _tenantId,
            email,
            null,
            "John",
            "Doe");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*email*");
    }

    [Fact]
    public void TransferOwnership_ValidProfessional_TransfersOwnership()
    {
        // Arrange
        var customer = new Core.Entities.Customer(
            _tenantId,
            "test@example.com",
            null,
            "John",
            "Doe");
        var newOwnerProfessionalId = Guid.NewGuid();

        // Act
        customer.TransferOwnership(newOwnerProfessionalId, "Business transfer");

        // Assert
        customer.OwnerProfessionalId.Should().Be(newOwnerProfessionalId);
    }

    [Fact]
    public void UpdateContactInfo_ValidInput_UpdatesContact()
    {
        // Arrange
        var customer = new Core.Entities.Customer(
            _tenantId,
            "test@example.com",
            null,
            "John",
            "Doe");

        // Act
        customer.UpdateContactInfo("new@example.com", "555-5678", Shared.Contracts.Enums.PreferredContactMethod.Phone);

        // Assert
        customer.Email.Should().Be("new@example.com");
        customer.Phone.Should().Be("555-5678");
        customer.PreferredContactMethod.Should().Be(Shared.Contracts.Enums.PreferredContactMethod.Phone);
    }

    [Fact]
    public void VerifyEmail_UnverifiedCustomer_SetsEmailVerified()
    {
        // Arrange
        var customer = new Core.Entities.Customer(
            _tenantId,
            "test@example.com",
            null,
            "John",
            "Doe");

        // Act
        customer.VerifyEmail();

        // Assert
        customer.EmailVerified.Should().BeTrue();
    }
}
