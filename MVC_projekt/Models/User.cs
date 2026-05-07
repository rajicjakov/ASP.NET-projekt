
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_projekt.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(256)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public DateTime DateJoined { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [InverseProperty(nameof(Tab.Creator))]
        public virtual ICollection<Tab> Tabs { get; set; } = new HashSet<Tab>();
    }
}