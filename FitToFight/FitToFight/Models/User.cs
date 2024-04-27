using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FitToFight.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";

        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
    }
}
