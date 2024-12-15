using System.ComponentModel.DataAnnotations;

namespace mvc_app.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; } // Унікальний ідентифікатор для кожного товару в замовленні

        [Required]
        public int ProductId { get; set; } // Ідентифікатор товару

        [Required]
        public int Quantity { get; set; } // Кількість товару

        [Required]
        public decimal Price { get; set; } // Ціна товару
    }
}
