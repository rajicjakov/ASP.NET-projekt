using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_projekt.Models
{
    public class TabColumn
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("TabMeasure")]
        public int TabMeasureId { get; set; }

        public virtual TabMeasure TabMeasure { get; set; } = null!;

        [Required]
        public int ColumnNumber { get; set; }

        [Required]
        public Duration ColumnDuration { get; set; } = new();

        public virtual ICollection<TabNote> Notes { get; set; } = new HashSet<TabNote>();
    }
}