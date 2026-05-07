using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_projekt.Models
{
    public class Duration
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Base { get; set; } // 1=cijela nota, 2=polovinka, 4=četvrtinka itd.

        [Required]
        public bool IsDotted { get; set; }

        [Required]
        [ForeignKey("TabColumn")]
        public int TabColumnId { get; set; }

        public virtual TabColumn TabColumn { get; set; } = null!;

        public float GetTotal()
        {
            if (IsDotted)
                return 1f / Base + 1f / (Base / 2);
            return 1f / Base;
        }
    }
}