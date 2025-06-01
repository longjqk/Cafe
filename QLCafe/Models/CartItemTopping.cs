namespace QLCafe.Models
{
    public class CartItemTopping
    {
        public int CartItemToppingId { get; set; }
        public int CartItemId { get; set; }
        public CartItem CartItem { get; set; }
        public int ToppingId { get; set; }
        public Topping Topping { get; set; }

        public decimal ToppingPrice { get; set; }

    }
}
