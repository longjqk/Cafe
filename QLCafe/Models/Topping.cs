namespace QLCafe.Models
{
    public class Topping
    {
        public int ToppingId { get; set; }
        public string ToppingName { get; set; } = null!;
        public double Price { get; set; }

        public ICollection<OrderDetailTopping> OrderDetailToppings { get; set; } = new List<OrderDetailTopping>();
    }
}
