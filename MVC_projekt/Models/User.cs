
namespace MVC_projekt.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime DateJoined { get; set; }
        public UserRole Role { get; set; }

        public List<Tab> Tabs { get; set; }
    }
}