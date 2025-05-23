namespace QLCafe.Models
{
    public class Bill
    {
        public int BillId { get; set; }
        public int OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public double TotalAmout { get; set; }
        public string PaymentMethod { get; set; } = null!;

        public Order Order { get; set; } = null!;
    }
}
