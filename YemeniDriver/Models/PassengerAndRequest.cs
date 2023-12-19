namespace YemeniDriver.Models
{
    public class PassengerAndRequest
    {
        public string RequestId { get; set; }
        public string PassengerId { get; set; }

        public virtual Request Request { get; set; }
        public virtual ApplicationUser Passenger { get; set; }
    }
}
