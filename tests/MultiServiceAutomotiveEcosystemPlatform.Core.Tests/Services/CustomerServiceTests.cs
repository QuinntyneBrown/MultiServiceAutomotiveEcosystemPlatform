using Microsoft.EntityFrameworkCore;
using Moq;
using MultiServiceAutomotiveEcosystemPlatform.Core.Data;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.CustomerAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Services;
using MultiServiceAutomotiveEcosystemPlatform.Core.Tests.Helpers;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Tests.Services;

public class CustomerServiceTests
{
    private readonly Mock<IMultiServiceAutomotiveEcosystemPlatformContext> _mockContext;
    private readonly TestTenantContext _tenantContext;
    private readonly CustomerService _service;
    private readonly List<Customer> _customers;
    private readonly List<CustomerOwnershipHistory> _ownershipHistories;

    public CustomerServiceTests()
    {
        _mockContext = new Mock<IMultiServiceAutomotiveEcosystemPlatformContext>();
        _tenantContext = new TestTenantContext();
        _customers = new List<Customer>();
        _ownershipHistories = new List<CustomerOwnershipHistory>();

        var mockCustomersDbSet = MockDbSetHelper.CreateMockDbSet(_customers);
        var mockHistoryDbSet = MockDbSetHelper.CreateMockDbSet(_ownershipHistories);

        _mockContext.Setup(c => c.Customers).Returns(mockCustomersDbSet.Object);
        _mockContext.Setup(c => c.CustomerOwnershipHistories).Returns(mockHistoryDbSet.Object);
        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _service = new CustomerService(_mockContext.Object, _tenantContext);
    }

    [Fact]
    public async Task CreateCustomerAsync_WithValidParameters_CreatesCustomer()
    {
        // Act
        var customer = await _service.CreateCustomerAsync(
            "test@example.com",
            "1234567890",
            "John",
            "Doe");

        // Assert
        Assert.NotNull(customer);
        Assert.Equal("test@example.com", customer.Email);
        Assert.Equal("John", customer.FirstName);
        Assert.Equal("Doe", customer.LastName);
        Assert.Equal(_tenantContext.TenantId, customer.TenantId);
        Assert.Single(_customers);
    }

    [Fact]
    public async Task CreateCustomerAsync_WithOwner_SetsOwner()
    {
        // Arrange
        var ownerId = Guid.NewGuid();

        // Act
        var customer = await _service.CreateCustomerAsync(
            "test@example.com",
            "1234567890",
            "John",
            "Doe",
            ownerId);

        // Assert
        Assert.Equal(ownerId, customer.OwnerProfessionalId);
    }

    [Fact]
    public async Task CreateCustomerAsync_WithDuplicateEmail_ThrowsInvalidOperationException()
    {
        // Arrange
        await _service.CreateCustomerAsync("test@example.com", "1234567890", "John", "Doe");

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateCustomerAsync("test@example.com", "0987654321", "Jane", "Smith"));
    }

    [Fact]
    public async Task GetCustomerByIdAsync_WithExistingCustomer_ReturnsCustomer()
    {
        // Arrange
        var customer = await _service.CreateCustomerAsync("test@example.com", "1234567890", "John", "Doe");

        // Act
        var result = await _service.GetCustomerByIdAsync(customer.CustomerId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customer.CustomerId, result.CustomerId);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_WithNonExistingCustomer_ReturnsNull()
    {
        // Act
        var result = await _service.GetCustomerByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetCustomerByEmailAsync_WithExistingEmail_ReturnsCustomer()
    {
        // Arrange
        await _service.CreateCustomerAsync("test@example.com", "1234567890", "John", "Doe");

        // Act
        var result = await _service.GetCustomerByEmailAsync("test@example.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public async Task GetCustomerByEmailAsync_NormalizesEmail()
    {
        // Arrange
        await _service.CreateCustomerAsync("test@example.com", "1234567890", "John", "Doe");

        // Act
        var result = await _service.GetCustomerByEmailAsync("  TEST@EXAMPLE.COM  ");

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetCustomersByOwnerAsync_ReturnsOwnedCustomers()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        await _service.CreateCustomerAsync("test1@example.com", "1234567890", "John", "Doe", ownerId);
        await _service.CreateCustomerAsync("test2@example.com", "0987654321", "Jane", "Smith", ownerId);
        await _service.CreateCustomerAsync("test3@example.com", "5555555555", "Bob", "Wilson");

        // Act
        var result = await _service.GetCustomersByOwnerAsync(ownerId);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetActiveCustomersAsync_ReturnsOnlyActiveCustomers()
    {
        // Arrange
        var customer1 = await _service.CreateCustomerAsync("test1@example.com", "1234567890", "John", "Doe");
        await _service.CreateCustomerAsync("test2@example.com", "0987654321", "Jane", "Smith");
        await _service.DeactivateCustomerAsync(customer1.CustomerId);

        // Act
        var result = await _service.GetActiveCustomersAsync();

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task UpdateCustomerAsync_WithValidParameters_UpdatesCustomer()
    {
        // Arrange
        var customer = await _service.CreateCustomerAsync("test@example.com", "1234567890", "John", "Doe");

        // Act
        var updated = await _service.UpdateCustomerAsync(
            customer.CustomerId,
            "new@example.com",
            "0987654321",
            "Jane",
            "Smith");

        // Assert
        Assert.Equal("new@example.com", updated.Email);
        Assert.Equal("Jane", updated.FirstName);
        Assert.Equal("Smith", updated.LastName);
    }

    [Fact]
    public async Task UpdateCustomerAsync_WithNonExistingCustomer_ThrowsInvalidOperationException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.UpdateCustomerAsync(Guid.NewGuid(), "test@example.com", "1234567890", "John", "Doe"));
    }

    [Fact]
    public async Task TransferOwnershipAsync_CreatesHistoryAndUpdatesOwner()
    {
        // Arrange
        var originalOwnerId = Guid.NewGuid();
        var newOwnerId = Guid.NewGuid();
        var transferredBy = Guid.NewGuid();
        var customer = await _service.CreateCustomerAsync("test@example.com", "1234567890", "John", "Doe", originalOwnerId);

        // Act
        await _service.TransferOwnershipAsync(customer.CustomerId, newOwnerId, "Customer requested", transferredBy);

        // Assert
        Assert.Equal(newOwnerId, customer.OwnerProfessionalId);
        Assert.Single(_ownershipHistories);
        Assert.Equal(originalOwnerId, _ownershipHistories[0].PreviousOwnerId);
        Assert.Equal(newOwnerId, _ownershipHistories[0].NewOwnerId);
    }

    [Fact]
    public async Task AssignOwnerIfNotSetAsync_WhenNoOwner_AssignsOwner()
    {
        // Arrange
        var customer = await _service.CreateCustomerAsync("test@example.com", "1234567890", "John", "Doe");
        var ownerId = Guid.NewGuid();

        // Act
        var result = await _service.AssignOwnerIfNotSetAsync(customer.CustomerId, ownerId);

        // Assert
        Assert.True(result);
        Assert.Equal(ownerId, customer.OwnerProfessionalId);
    }

    [Fact]
    public async Task AssignOwnerIfNotSetAsync_WhenOwnerExists_ReturnsFalse()
    {
        // Arrange
        var existingOwnerId = Guid.NewGuid();
        var customer = await _service.CreateCustomerAsync("test@example.com", "1234567890", "John", "Doe", existingOwnerId);
        var newOwnerId = Guid.NewGuid();

        // Act
        var result = await _service.AssignOwnerIfNotSetAsync(customer.CustomerId, newOwnerId);

        // Assert
        Assert.False(result);
        Assert.Equal(existingOwnerId, customer.OwnerProfessionalId);
    }

    [Fact]
    public async Task DeactivateCustomerAsync_DeactivatesCustomer()
    {
        // Arrange
        var customer = await _service.CreateCustomerAsync("test@example.com", "1234567890", "John", "Doe");

        // Act
        await _service.DeactivateCustomerAsync(customer.CustomerId);

        // Assert
        Assert.False(customer.IsActive);
    }

    [Fact]
    public async Task EmailExistsAsync_WithExistingEmail_ReturnsTrue()
    {
        // Arrange
        await _service.CreateCustomerAsync("test@example.com", "1234567890", "John", "Doe");

        // Act
        var result = await _service.EmailExistsAsync("test@example.com");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task EmailExistsAsync_WithExcludedCustomer_ReturnsFalse()
    {
        // Arrange
        var customer = await _service.CreateCustomerAsync("test@example.com", "1234567890", "John", "Doe");

        // Act
        var result = await _service.EmailExistsAsync("test@example.com", customer.CustomerId);

        // Assert
        Assert.False(result);
    }
}
