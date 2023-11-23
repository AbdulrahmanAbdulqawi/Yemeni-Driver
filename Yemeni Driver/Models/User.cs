using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Yemeni_Driver.Data.Enums;

namespace Yemeni_Driver.Models
{
    public class User 
    {
        [NotNull]
        public string FirstName { get; set; }
        [NotNull]
        public string LastName { get; set; }
        [NotNull]
        public string Email { get; set; }
        [NotNull]
        [PasswordPropertyText]
        [Length(8,20)]
        public string Password { get; set; }
        [NotNull]
        public Gender Gender { get; set; }
        [NotNull]
        [Length(6, 10)]
        public string PhoneNumber { get; set; }
        [NotNull]
        [Range(0, 5)]
        public int Rateing { get; set; }

    }
}
