// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using MultiServiceAutomotiveEcosystemPlatform.Api.Features.Customers;
using MultiServiceAutomotiveEcosystemPlatform.Api.Features.Referrals;
using Xunit;

namespace MultiServiceAutomotiveEcosystemPlatform.Api.Tests.Integration;

public class ReferralsControllerTests : IClassFixture<TestWebApplicationFactory>
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory _factory;

    public ReferralsControllerTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateCustomerReferral_ReturnsCreatedReferral()
    {
        // Arrange - create a customer first
        var customerCommand = new CreateCustomerCommand
        {
            Email = "referrer@example.com",
            Phone = "1234567890",
            FirstName = "Referrer",
            LastName = "Customer"
        };
        var customerResponse = await _client.PostAsJsonAsync("/api/customers", customerCommand);
        var customer = await customerResponse.Content.ReadFromJsonAsync<CustomerDto>(JsonOptions);

        var referralCommand = new CreateCustomerReferralCommand
        {
            ReferrerCustomerId = customer!.CustomerId,
            RefereeEmail = "referee@example.com",
            RefereeName = "Referred Customer",
            ReferralSource = "test"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/referrals/customer", referralCommand);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var referral = await response.Content.ReadFromJsonAsync<CustomerReferralDto>(JsonOptions);
        Assert.NotNull(referral);
        Assert.Equal(customer.CustomerId, referral.ReferrerCustomerId);
        Assert.Equal("referee@example.com", referral.RefereeEmail);
    }

    [Fact]
    public async Task GetCustomerReferrals_ReturnsReferrals()
    {
        // Arrange - create a customer and referral
        var customerCommand = new CreateCustomerCommand
        {
            Email = "referrer2@example.com",
            Phone = "2345678901",
            FirstName = "Another",
            LastName = "Referrer"
        };
        var customerResponse = await _client.PostAsJsonAsync("/api/customers", customerCommand);
        var customer = await customerResponse.Content.ReadFromJsonAsync<CustomerDto>(JsonOptions);

        var referralCommand = new CreateCustomerReferralCommand
        {
            ReferrerCustomerId = customer!.CustomerId,
            RefereeEmail = "referee2@example.com",
            RefereeName = "Another Referred"
        };
        await _client.PostAsJsonAsync("/api/referrals/customer", referralCommand);

        // Act
        var response = await _client.GetAsync($"/api/referrals/customer/{customer.CustomerId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<GetCustomerReferralsQueryResponse>(JsonOptions);
        Assert.NotNull(result);
        Assert.True(result.Referrals.Count > 0);
    }
}
