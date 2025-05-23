namespace QLCafe.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public int DrinkId { get; set; }
        public int Quantity { get; set; }

        public Cart Cart { get; set; } = null!;
        public Drink Drink { get; set; } = null!;
    }
}
