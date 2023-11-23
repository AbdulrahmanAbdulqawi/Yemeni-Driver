namespace Yemeni_Driver.Models
{
    public class DriverAndRequest
    {
        public string RequestId { get; set; }
        public string DriverId { get; set; }

        public virtual Request Request { get; set; }
        public virtual Driver Driver { get; set; }

    }
}
