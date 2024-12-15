using mvc_app.Models;

namespace mvc_app.Services
{
    public class OrderService
    {
        private readonly ProductContext _context;

        public OrderService(ProductContext context)
        {
            _context = context;
        }

        // Створити замовлення
        public async Task<Order> CreateOrder(string userId)
        {
            // Отримати всі товари в кошику користувача
            var cartItems = _context.CartItems.Where(c => c.UserId == userId).ToList();
            if (!cartItems.Any()) throw new InvalidOperationException("Cart is empty.");

            // Отримати всі продукти, які є в кошику
            var productIds = cartItems.Select(c => c.ProductId).ToList();
            var products = _context.Products.Where(p => productIds.Contains(p.Id)).ToDictionary(p => p.Id, p => p.Price);

            // Перевірка: чи всі продукти доступні
            if (products.Count != productIds.Count)
            {
                throw new InvalidOperationException("Some products in the cart are not available.");
            }

            // Формування замовлення
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalPrice = cartItems.Sum(c => c.Quantity * products[c.ProductId]), // Розрахунок загальної ціни
                Status = "Pending",
                Items = cartItems.Select(c => new OrderItem
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    Price = products[c.ProductId] // Ціна з моделі Product
                }).ToList()
            };

            // Зберегти замовлення і очистити кошик
            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();

            return order;
        }

    }
}
