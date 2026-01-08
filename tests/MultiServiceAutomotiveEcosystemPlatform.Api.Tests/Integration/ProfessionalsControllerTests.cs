// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using MultiServiceAutomotiveEcosystemPlatform.Api.Features.Professionals;
using Xunit;

namespace MultiServiceAutomotiveEcosystemPlatform.Api.Tests.Integration;

public class ProfessionalsControllerTests : IClassFixture<TestWebApplicationFactory>
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory _factory;

    public ProfessionalsControllerTests(TestWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateProfessional_ReturnsCreatedProfessional()
    {
        // Arrange
        var command = new CreateProfessionalCommand
        {
            UserId = Guid.NewGuid(),
            BusinessName = "Test Auto Shop",
            BusinessType = "MechanicDomestic",
            FirstName = "John",
            LastName = "Mechanic",
            Email = "john@testautoshop.com",
            Phone = "1234567890",
            AddressLine1 = "123 Main St",
            City = "TestCity",
            Province = "ON",
            PostalCode = "K1A 0B1"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/professionals", command);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var professional = await response.Content.ReadFromJsonAsync<ProfessionalDto>(JsonOptions);
        Assert.NotNull(professional);
        Assert.Equal("Test Auto Shop", professional.BusinessName);
        Assert.Equal("John", professional.FirstName);
        Assert.Equal("Mechanic", professional.LastName);
    }

    [Fact]
    public async Task GetProfessional_ReturnsProfessional()
    {
        // Arrange - create a professional first
        var command = new CreateProfessionalCommand
        {
            UserId = Guid.NewGuid(),
            BusinessName = "German Auto Specialists",
            BusinessType = "MechanicGerman",
            FirstName = "Hans",
            LastName = "Schmidt",
            Email = "hans@germanspecs.com",
            Phone = "9876543210",
            AddressLine1 = "456 Oak Ave",
            City = "Berlin",
            Province = "QC",
            PostalCode = "H2Y 1C6"
        };
        var createResponse = await _client.PostAsJsonAsync("/api/professionals", command);
        var createdProfessional = await createResponse.Content.ReadFromJsonAsync<ProfessionalDto>(JsonOptions);

        // Act
        var response = await _client.GetAsync($"/api/professionals/{createdProfessional!.ProfessionalId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var professional = await response.Content.ReadFromJsonAsync<ProfessionalDto>(JsonOptions);
        Assert.NotNull(professional);
        Assert.Equal("Hans", professional.FirstName);
        Assert.Equal("Schmidt", professional.LastName);
    }

    [Fact]
    public async Task GetProfessionals_ReturnsPaginatedList()
    {
        // Arrange - create some professionals
        for (int i = 0; i < 3; i++)
        {
            var command = new CreateProfessionalCommand
            {
                UserId = Guid.NewGuid(),
                BusinessName = $"Auto Shop {i}",
                BusinessType = "MechanicDomestic",
                FirstName = $"Professional{i}",
                LastName = "Test",
                Email = $"prof{i}@shop.com",
                Phone = $"55500{i}0000",
                AddressLine1 = $"{i} Test St",
                City = "TestCity",
                Province = "BC",
                PostalCode = "V6B 1A1"
            };
            await _client.PostAsJsonAsync("/api/professionals", command);
        }

        // Act
        var response = await _client.GetAsync("/api/professionals?page=1&pageSize=10");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<GetProfessionalsQueryResponse>(JsonOptions);
        Assert.NotNull(result);
        Assert.True(result.Professionals.Count >= 3);
    }
}
