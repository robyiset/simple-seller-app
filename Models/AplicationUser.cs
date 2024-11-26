
using Microsoft.AspNetCore.Identity;

namespace simple_seller_app.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Role { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime? LoginDate { get; set; }
    }
}