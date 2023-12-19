using System.ComponentModel.DataAnnotations.Schema;

namespace YemeniDriver.Models
{
    public class DriverAndRequest
    {
        public string RequestId { get; set; }
        public string DriverId { get; set; }

        public virtual Request Request { get; set; }
        public virtual ApplicationUser Driver { get; set; }

    }
}
