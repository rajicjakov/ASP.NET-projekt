using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_projekt.Models
{
    public class Tab
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Artist { get; set; } = string.Empty;

        [Required]
        [ForeignKey("Creator")]
        public int CreatorId { get; set; }

        public virtual User Creator { get; set; } = null!;

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        [StringLength(50)]
        public string StringTuning { get; set; } = string.Empty;

        [Required]
        [Range(1, 300, ErrorMessage = "BPM must be at least 1.")]
        public int BPM { get; set; }

        [Required]
        public Difficulty Difficulty { get; set; }

        public virtual ICollection<TabMeasure> Measures { get; set; } = new HashSet<TabMeasure>();
    }
}