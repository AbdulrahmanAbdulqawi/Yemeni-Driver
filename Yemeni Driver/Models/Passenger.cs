using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Yemeni_Driver.Data.Enums;

namespace Yemeni_Driver.Models
{
    public class Passenger : User 
    {
        [Key]
        public string UserId { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual CancelRequest CancelRequest { get; set; }


    }
}
