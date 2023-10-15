using System;
using System.Net.Http;
using System.Threading.Tasks;
using ECommerce;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Nest;
using Xunit;

namespace ECommerce_ElasticsearchIntegrationTests
{
    public class ElasticsearchIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly Mock<IElasticClient> _elasticClientMock;

        public ElasticsearchIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _elasticClientMock = new Mock<IElasticClient>();
        }

        [Fact]
        public async Task SearchProducts_ReturnsExpectedResult()
        {
            // Arrange
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(_elasticClientMock.Object);
                });
            }).CreateClient();

            var searchResponse = new Nest.SearchResponse<Product>
            {
                Documents = new List<Product>
                {
                    new Product { Id = 1, Name = "Product 1", Description = "Description 1" },
                    new Product { Id = 2, Name = "Product 2", Description = "Description 2" }
                },
                IsValid = true
            };

            _elasticClientMock.Setup(client => client.SearchAsync<Product>(It.IsAny<Func<SearchDescriptor<Product>, ISearchRequest>>()))
                .ReturnsAsync(searchResponse);

            // Act
            var response = await client.GetAsync("/api/products/search?query=Product");

            // Assert
            response.EnsureSuccessStatusCode();
            // Assert your response content or other conditions here
        }
    }
}