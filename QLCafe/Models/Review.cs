namespace QLCafe.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = null!;
        public int UserId { get; set; }
        public int DrinkId { get; set; }

        public ApplicationUser User { get; set; } = null!;
        public Drink Drink { get; set; } = null!;
    }
}
