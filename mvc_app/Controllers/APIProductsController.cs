using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mvc_app.Models;
using mvc_app.Services;
using System.Threading.Tasks;

namespace mvc_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class APIProductsController : ControllerBase
    {
        private readonly IServiceProducts _serviceProducts;
        private readonly ILogger<APIProductsController> _logger;

        public APIProductsController(IServiceProducts serviceProducts, ILogger<APIProductsController> logger)
        {
            _serviceProducts = serviceProducts;
            _logger = logger;
        }

        // Получить все продукты
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            _logger.LogInformation("Fetching all products.");
            var products = await _serviceProducts.ReadAsync();
            return Ok(products);
        }

        // Получить продукт по ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            _logger.LogInformation($"Fetching product with ID: {id}");
            var product = await _serviceProducts.GetByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning($"Product with ID {id} not found.");
                return NotFound();
            }
            return Ok(product);
        }

        // Создать новый продукт
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            if (product == null)
            {
                _logger.LogError("CreateProduct: Product object is null.");
                return BadRequest("Product object is null.");
            }

            _logger.LogInformation("Creating a new product.");
            var productCreated = await _serviceProducts.CreateAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = productCreated.Id }, productCreated);
        }

        // Обновить продукт
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (product == null)
            {
                _logger.LogError("UpdateProduct: Product object is null.");
                return BadRequest("Product object is null.");
            }

            _logger.LogInformation($"Updating product with ID: {id}");
            var productUpdated = await _serviceProducts.UpdateAsync(id, product);
            if (productUpdated == null)
            {
                _logger.LogWarning($"Product with ID {id} not found for update.");
                return NotFound();
            }
            return Ok(productUpdated);
        }

        // Удалить продукт
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            _logger.LogInformation($"Deleting product with ID: {id}");
            var deleted = await _serviceProducts.DeleteAsync(id);
            if (!deleted)
            {
                _logger.LogWarning($"Product with ID {id} not found for deletion.");
                return NotFound();
            }
            return Ok(new { message = "Product deleted successfully." });
        }
    }
}
