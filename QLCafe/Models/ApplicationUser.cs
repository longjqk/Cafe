using Microsoft.AspNetCore.Identity;

namespace QLCafe.Models
{
    public class ApplicationUser: IdentityUser<int>
    {
        //public int UserId { get; set; }
        //public string UserName { get; set; } = null!;
        //public string Password { get; set; } = null!;
        
        public string Address { get; set; } = null!;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public Cart? Cart { get; set; }
    }

}
