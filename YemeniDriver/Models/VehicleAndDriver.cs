namespace YemeniDriver.Models
{
    public class VehicleAndDriver
    {
        public string VehicleId { get; set; }
        public string DriverId { get; set; }


        public Vehicle Vehicle { get; set; }
        public Driver Driver { get; set; }
    }
}
