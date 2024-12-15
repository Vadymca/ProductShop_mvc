using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mvc_app.Services;

namespace mvc_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder()
        {
            // Отримання UserId з claims
            var userIdClaim = HttpContext.User.FindFirst("id");
            if (userIdClaim == null)
            {
                return Unauthorized("User not logged in.");
            }

            var userId = userIdClaim.Value; // Отримуємо UserId як рядок

            try
            {
                // Створюємо замовлення для користувача
                var order = await _orderService.CreateOrder(userId);
                return Ok(order);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Обробка інших можливих помилок
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"An error occurred while creating the order: {ex.Message}");
            }
        }
    }
}
