namespace QLCafe.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string Status { get; set; } = null!;
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public ApplicationUser? User { get; set; } 

        public Bill? Bill { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
