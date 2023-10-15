using ECommerce.Models;

namespace ECommerce.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> SearchProducts(string query);
        Task<bool> AddProductToElasticsearch(Product product);
    }
}
