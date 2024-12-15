using System.ComponentModel.DataAnnotations;

namespace mvc_app.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; } // Унікальний ідентифікатор замовлення

        [Required]
        public string UserId { get; set; } // Ідентифікатор користувача

        [Required]
        public DateTime OrderDate { get; set; } // Дата замовлення

        [Required]
        public decimal TotalPrice { get; set; } // Загальна сума

        [Required]
        public string? Status { get; set; } // Статус замовлення (наприклад, "Pending", "Completed", "Cancelled")

        public ICollection<OrderItem>? Items { get; set; } // Список товарів у замовленні
    }
}
