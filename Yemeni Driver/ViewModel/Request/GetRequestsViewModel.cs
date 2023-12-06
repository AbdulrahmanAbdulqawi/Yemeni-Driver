﻿using Yemeni_Driver.Data.Enums;

namespace Yemeni_Driver.ViewModel.Request
{
    public class GetRequestsViewModel
    {
        public string ApplicationUserId { get; set; }
        public DateTime PickupTime { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public double EstimationPrice { get; set; }
        public int NumberOfSeats { get; set; }
        public RequestStatus Status { get; set; }
    }
}
