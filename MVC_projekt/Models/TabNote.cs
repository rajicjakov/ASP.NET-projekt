using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_projekt.Models
{
    public class TabNote
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("TabColumn")]
        public int TabColumnId { get; set; }

        public virtual TabColumn TabColumn { get; set; } = null!;

        [Required]
        [Range(1, 6)]
        public int StringNumber { get; set; }

        [Required]
        [Range(0, 24)]
        public int Fret { get; set; }

        public bool PalmMuted { get; set; }
        public bool HammerOn { get; set; }
        public bool PullOff { get; set; }
        public bool Bend { get; set; }
    }
}