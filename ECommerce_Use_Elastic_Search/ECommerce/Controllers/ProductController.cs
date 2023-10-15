using ECommerce.Models;
using ECommerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string query)
        {
            var products = await _productService.SearchProducts(query);

            return Ok(products);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var status = await _productService.AddProductToElasticsearch(product);

            return Ok(product.Id);
        }
    }
}
