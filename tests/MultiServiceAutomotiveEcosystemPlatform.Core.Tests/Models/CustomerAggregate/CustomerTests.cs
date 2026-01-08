using MultiServiceAutomotiveEcosystemPlatform.Core.Models.CustomerAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.CustomerAggregate.Enums;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Tests.Models.CustomerAggregate;

public class CustomerTests
{
    private readonly Guid _tenantId = Guid.NewGuid();

    [Fact]
    public void Constructor_WithValidParameters_CreatesCustomer()
    {
        // Arrange & Act
        var customer = new Customer(
            _tenantId,
            "test@example.com",
            "1234567890",
            "John",
            "Doe");

        // Assert
        Assert.NotEqual(Guid.Empty, customer.CustomerId);
        Assert.Equal(_tenantId, customer.TenantId);
        Assert.Equal("test@example.com", customer.Email);
        Assert.Equal("1234567890", customer.Phone);
        Assert.Equal("John", customer.FirstName);
        Assert.Equal("Doe", customer.LastName);
        Assert.Equal("John Doe", customer.FullName);
        Assert.Equal(CustomerStatus.Active, customer.Status);
        Assert.True(customer.IsActive);
        Assert.Null(customer.OwnerProfessionalId);
    }

    [Fact]
    public void Constructor_WithOwner_SetsOwnerProfessionalId()
    {
        // Arrange
        var ownerId = Guid.NewGuid();

        // Act
        var customer = new Customer(_tenantId, "test@example.com", "1234567890", "John", "Doe", ownerId);

        // Assert
        Assert.Equal(ownerId, customer.OwnerProfessionalId);
    }

    [Fact]
    public void Constructor_NormalizesEmailAndPhone()
    {
        // Act
        var customer = new Customer(_tenantId, "  TEST@EXAMPLE.COM  ", "(123) 456-7890", "John", "Doe");

        // Assert
        Assert.Equal("test@example.com", customer.Email);
        Assert.Equal("1234567890", customer.Phone);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithEmptyEmail_ThrowsArgumentException(string email)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Customer(_tenantId, email, "1234567890", "John", "Doe"));
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("invalid@")]
    [InlineData("@invalid")]
    public void Constructor_WithInvalidEmail_ThrowsArgumentException(string email)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Customer(_tenantId, email, "1234567890", "John", "Doe"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("123")]
    public void Constructor_WithInvalidPhone_ThrowsArgumentException(string phone)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Customer(_tenantId, "test@example.com", phone, "John", "Doe"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithEmptyFirstName_ThrowsArgumentException(string firstName)
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Customer(_tenantId, "test@example.com", "1234567890", firstName, "Doe"));
    }

    [Fact]
    public void UpdateContactInfo_WithValidParameters_UpdatesContact()
    {
        // Arrange
        var customer = new Customer(_tenantId, "test@example.com", "1234567890", "John", "Doe");

        // Act
        customer.UpdateContactInfo("new@example.com", "0987654321", "1111111111");

        // Assert
        Assert.Equal("new@example.com", customer.Email);
        Assert.Equal("0987654321", customer.Phone);
        Assert.Equal("1111111111", customer.PhoneSecondary);
    }

    [Fact]
    public void UpdatePersonalInfo_WithValidParameters_UpdatesPersonalInfo()
    {
        // Arrange
        var customer = new Customer(_tenantId, "test@example.com", "1234567890", "John", "Doe");
        var dob = new DateTime(1990, 1, 1);

        // Act
        customer.UpdatePersonalInfo("Jane", "Smith", dob);

        // Assert
        Assert.Equal("Jane", customer.FirstName);
        Assert.Equal("Smith", customer.LastName);
        Assert.Equal(dob, customer.DateOfBirth);
        Assert.Equal("Jane Smith", customer.FullName);
    }

    [Fact]
    public void UpdateAddress_SetsAddressFields()
    {
        // Arrange
        var customer = new Customer(_tenantId, "test@example.com", "1234567890", "John", "Doe");

        // Act
        customer.UpdateAddress("123 Main St", "Apt 4", "Toronto", "ON", "M5H 2N2", "CA");

        // Assert
        Assert.Equal("123 Main St", customer.AddressLine1);
        Assert.Equal("Apt 4", customer.AddressLine2);
        Assert.Equal("Toronto", customer.City);
        Assert.Equal("ON", customer.Province);
        Assert.Equal("M5H 2N2", customer.PostalCode);
        Assert.Equal("CA", customer.Country);
    }

    [Fact]
    public void UpdatePreferences_SetsPreferenceFields()
    {
        // Arrange
        var customer = new Customer(_tenantId, "test@example.com", "1234567890", "John", "Doe");

        // Act
        customer.UpdatePreferences(PreferredContactMethod.Sms, true, true);

        // Assert
        Assert.Equal(PreferredContactMethod.Sms, customer.PreferredContactMethod);
        Assert.True(customer.MarketingConsent);
        Assert.True(customer.NewsletterSubscribed);
    }

    [Fact]
    public void AssignOwner_SetsOwnerProfessionalId()
    {
        // Arrange
        var customer = new Customer(_tenantId, "test@example.com", "1234567890", "John", "Doe");
        var ownerId = Guid.NewGuid();

        // Act
        customer.AssignOwner(ownerId);

        // Assert
        Assert.Equal(ownerId, customer.OwnerProfessionalId);
    }

    [Fact]
    public void TransferOwnership_ChangesOwner()
    {
        // Arrange
        var customer = new Customer(_tenantId, "test@example.com", "1234567890", "John", "Doe", Guid.NewGuid());
        var newOwnerId = Guid.NewGuid();

        // Act
        customer.TransferOwnership(newOwnerId);

        // Assert
        Assert.Equal(newOwnerId, customer.OwnerProfessionalId);
    }

    [Fact]
    public void TransferOwnership_ToSameOwner_ThrowsInvalidOperationException()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var customer = new Customer(_tenantId, "test@example.com", "1234567890", "John", "Doe", ownerId);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => customer.TransferOwnership(ownerId));
    }

    [Fact]
    public void VerifyEmail_SetsEmailVerifiedToTrue()
    {
        // Arrange
        var customer = new Customer(_tenantId, "test@example.com", "1234567890", "John", "Doe");

        // Act
        customer.VerifyEmail();

        // Assert
        Assert.True(customer.EmailVerified);
    }

    [Fact]
    public void VerifyPhone_SetsPhoneVerifiedToTrue()
    {
        // Arrange
        var customer = new Customer(_tenantId, "test@example.com", "1234567890", "John", "Doe");

        // Act
        customer.VerifyPhone();

        // Assert
        Assert.True(customer.PhoneVerified);
    }

    [Fact]
    public void Activate_SetsStatusToActive()
    {
        // Arrange
        var customer = new Customer(_tenantId, "test@example.com", "1234567890", "John", "Doe");
        customer.Deactivate();

        // Act
        customer.Activate();

        // Assert
        Assert.Equal(CustomerStatus.Active, customer.Status);
        Assert.True(customer.IsActive);
    }

    [Fact]
    public void Deactivate_SetsStatusToInactive()
    {
        // Arrange
        var customer = new Customer(_tenantId, "test@example.com", "1234567890", "John", "Doe");

        // Act
        customer.Deactivate();

        // Assert
        Assert.Equal(CustomerStatus.Inactive, customer.Status);
        Assert.False(customer.IsActive);
    }

    [Fact]
    public void Block_SetsStatusToBlocked()
    {
        // Arrange
        var customer = new Customer(_tenantId, "test@example.com", "1234567890", "John", "Doe");

        // Act
        customer.Block();

        // Assert
        Assert.Equal(CustomerStatus.Blocked, customer.Status);
        Assert.False(customer.IsActive);
    }

    [Fact]
    public void AddTag_AddsNormalizedTag()
    {
        // Arrange
        var customer = new Customer(_tenantId, "test@example.com", "1234567890", "John", "Doe");

        // Act
        customer.AddTag("  VIP  ");

        // Assert
        Assert.Contains("vip", customer.Tags);
    }

    [Fact]
    public void AddTag_WithEmptyTag_ThrowsArgumentException()
    {
        // Arrange
        var customer = new Customer(_tenantId, "test@example.com", "1234567890", "John", "Doe");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => customer.AddTag(""));
    }

    [Fact]
    public void AddTag_DuplicateTag_DoesNotAddAgain()
    {
        // Arrange
        var customer = new Customer(_tenantId, "test@example.com", "1234567890", "John", "Doe");
        customer.AddTag("vip");

        // Act
        customer.AddTag("VIP");

        // Assert
        Assert.Single(customer.Tags);
    }

    [Fact]
    public void RemoveTag_RemovesExistingTag()
    {
        // Arrange
        var customer = new Customer(_tenantId, "test@example.com", "1234567890", "John", "Doe");
        customer.AddTag("vip");

        // Act
        customer.RemoveTag("VIP");

        // Assert
        Assert.Empty(customer.Tags);
    }

    [Fact]
    public void UpdateNotes_SetsNotes()
    {
        // Arrange
        var customer = new Customer(_tenantId, "test@example.com", "1234567890", "John", "Doe");

        // Act
        customer.UpdateNotes("Important customer notes");

        // Assert
        Assert.Equal("Important customer notes", customer.Notes);
    }

    [Fact]
    public void RecordActivity_SetsLastActivityAt()
    {
        // Arrange
        var customer = new Customer(_tenantId, "test@example.com", "1234567890", "John", "Doe");

        // Act
        customer.RecordActivity();

        // Assert
        Assert.NotNull(customer.LastActivityAt);
    }
}
