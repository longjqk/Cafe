namespace QLCafe.Models
{
    public class Topping
    {
        public int ToppingId { get; set; }
        public string ToppingName { get; set; } = null!;
        public decimal Price { get; set; }

        public ICollection<OrderDetailTopping> OrderDetailToppings { get; set; } = new List<OrderDetailTopping>();
        public ICollection<CartItemTopping> CartItemToppings { get; set; } = new List<CartItemTopping>();
    }
}
