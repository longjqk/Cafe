using System.ComponentModel.DataAnnotations;

namespace QLCafe.Models
{
    public class Drink
    {
        public int DrinkId { get; set; }
        public int Rating { get; set; }
        [Required(ErrorMessage = "Tên đồ uống không được để trống")]
        public string DrinkName { get; set; } = null!;
        [Range(1000, 1000000, ErrorMessage = "Giá phải từ 1,000 đến 1,000,000")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Mô tả không được để trống")]
        public string Des { get; set; } = null!;
        
        public string? ImgUrl { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
