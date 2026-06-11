using System;

namespace MVC_projekt.Models
{
    public class ProfileViewModel
    {
        public User User { get; set; } = null!;
        public string PhotoUrl { get; set; } = string.Empty;
    }
}
