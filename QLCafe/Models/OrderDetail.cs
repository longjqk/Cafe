namespace QLCafe.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int DrinkId { get; set; }
        public int Quantity { get; set; }

        public Order Order { get; set; } = null!;
        public Drink Drink { get; set; } = null!;
        public ICollection<OrderDetailTopping> OrderDetailToppings { get; set; } = new List<OrderDetailTopping>();
    }
}
