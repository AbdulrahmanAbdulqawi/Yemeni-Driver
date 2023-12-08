namespace YemeniDriver.Models
{
    public class DriverAndRequest
    {
        public string RequestId { get; set; }
        public string ApplicationUserId { get; set; }

        public virtual Request Request { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
