namespace QLCafe.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public int DrinkId { get; set; }
        public int Quantity { get; set; }
        public string? ToppingDescription { get; set; }
        public decimal ToppingPrice { get; set; }

        public ICollection<CartItemTopping> CartItemToppings { get; set; } = new List<CartItemTopping>();

        public Cart Cart { get; set; } = null!;
        public Drink Drink { get; set; } = null!;
    }
}
