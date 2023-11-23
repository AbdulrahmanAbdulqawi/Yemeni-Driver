using Microsoft.AspNetCore.Identity;

namespace Yemeni_Driver.Models
{
    public class ApplicationUser : IdentityUser
    {
     

        public Passenger Passenger { get; set; }
        public Driver Driver { get; set; }

    }
}
