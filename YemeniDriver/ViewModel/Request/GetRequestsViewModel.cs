using YemeniDriver.Data.Enums;

namespace YemeniDriver.ViewModel.Request
{
    public class GetRequestsViewModel
    {
        public string ApplicationUserId { get; set; }
        public string PassengerName {  get; set; }
        public string DriverName { get; set; }

        public DateTime PickupTime { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public double EstimationPrice { get; set; }
        public int NumberOfSeats { get; set; }
        public RequestStatus Status { get; set; }
    }
}
