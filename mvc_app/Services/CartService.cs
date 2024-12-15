using Microsoft.EntityFrameworkCore;
using mvc_app.Models;

namespace mvc_app.Services
{
    public class CartService
    {
        private readonly ProductContext _context;

        public CartService(ProductContext context)
        {
            _context = context;
        }
        public async Task<bool> CheckIfProductExists(int productId)
        {
            return await _context.Products.AnyAsync(p => p.Id == productId);
        }

        // Додати товар до кошика
        public async Task<CartItem> AddToCart(CartItem cartItem)
        {
            Console.WriteLine($"Adding to cart: UserId={cartItem.UserId}, ProductId={cartItem.ProductId}, Quantity={cartItem.Quantity}");

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == cartItem.UserId && c.ProductId == cartItem.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += cartItem.Quantity;
                Console.WriteLine($"Updated quantity: {existingItem.Quantity}");
            }
            else
            {
                Console.WriteLine($"Adding new item to cart: {cartItem.ProductId}");
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            // Завантажуємо дані продукту, щоб повернути актуальні дані
            return await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.Id == cartItem.Id)
                .Select(c => new CartItem
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    ProductName = c.Product.Name,
                    Price = c.Product.Price,
                    TotalPrice = c.Quantity * c.Product.Price
                })
                .FirstOrDefaultAsync();
        }



        // Отримати всі товари з кошика користувача
        public IEnumerable<CartItem> GetCartItems(string userId)
        {
            Console.WriteLine($"Fetching cart items for UserId: {userId}");

            var items = _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .Select(c => new CartItem
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    ProductName = c.Product.Name,
                    Price = c.Product.Price,
                    TotalPrice = c.Quantity * c.Product.Price
                });

            Console.WriteLine($"Found {items.Count()} items");

            return items.ToList();
        }



        // Видалити товар із кошика
        public async Task<bool> RemoveFromCart(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null) return false;

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        // Очистити кошик
        public async Task ClearCart(string userId)
        {
            var cartItems = _context.CartItems.Where(c => c.UserId == userId);
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }
    }
}
