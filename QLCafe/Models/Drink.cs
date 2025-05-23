namespace QLCafe.Models
{
    public class Drink
    {
        public int DrinkId { get; set; }
        public int Rating { get; set; }
        public string DrinkName { get; set; } = null!;
        public double Price { get; set; }
        public string Des { get; set; } = null!;
        public string ImgUrl { get; set; } = null!;
        public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
