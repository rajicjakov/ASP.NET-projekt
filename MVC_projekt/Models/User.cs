using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MVC_projekt.Models
{
    public class User : IdentityUser<int>
    {
        [NotMapped]
        public string Username
        {
            get => UserName ?? string.Empty;
            set => UserName = value;
        }

        [Required]
        public DateTime DateJoined { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [InverseProperty(nameof(Tab.Creator))]
        public virtual ICollection<Tab> Tabs { get; set; } = new HashSet<Tab>();
    }
}
