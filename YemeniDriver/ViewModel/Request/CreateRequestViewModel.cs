using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YemeniDriver.Data.Enums;

namespace YemeniDriver.ViewModel.Request
{
    public class CreateRequestViewModel
    {
        //public string RequestId { get; set; }
        //public string ApplicationUserId { get; set; }
        public string DropoffLocation { get; set; }
    }
}
