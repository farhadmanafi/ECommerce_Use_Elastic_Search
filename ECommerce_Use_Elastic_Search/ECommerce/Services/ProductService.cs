using ECommerce.Models;
using Nest;

namespace ECommerce.Services
{
    public class ProductService : IProductService
    {
        private readonly IElasticClient _elasticClient;

        public ProductService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<IEnumerable<Product>> SearchProducts(string query)
        {
            var searchResponse = await _elasticClient.SearchAsync<Product>(s => s
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.Name) // Property to search
                        .Query(query)
                    )
                )
            );

            return searchResponse.Documents;
        }

        public async Task<bool> AddProductToElasticsearch(Product product)
        {
            var indexResponse = await _elasticClient.IndexDocumentAsync(product);
            return indexResponse.IsValid;
        }

        // Implement more methods for CRUD operations
    }
}
