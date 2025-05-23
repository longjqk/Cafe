namespace QLCafe.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public ICollection<Drink> Drinks { get; set; } = new List<Drink>();
    }
}
