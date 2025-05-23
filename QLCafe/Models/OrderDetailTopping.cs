namespace QLCafe.Models
{
    public class OrderDetailTopping
    {
        public int OrderDetailToppingId { get; set; }
        public int OrderDetailId { get; set; }
        public int ToppingId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        public OrderDetail OrderDetail { get; set; } = null!;
        public Topping Topping { get; set; } = null!;
    }
}
