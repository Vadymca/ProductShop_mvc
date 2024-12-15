using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace mvc_app.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; } // Унікальний ідентифікатор
        public string? UserId { get; set; } = string.Empty;// Ідентифікатор користувача

        [Required]
        public int ProductId { get; set; } // Ідентифікатор товару

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; } // Кількість
        public string? ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; } 
    }
}
