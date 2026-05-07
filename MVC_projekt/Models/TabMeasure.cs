using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_projekt.Models
{
    public class TabMeasure
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Tab")]
        public int TabId { get; set; }

        public virtual Tab Tab { get; set; } = null!;

        [Required]
        public int MeasureNumber { get; set; }

        [Required]
        public int TimeSignatureTop { get; set; }

        [Required]
        public int TimeSignatureBottom { get; set; }

        public virtual ICollection<TabColumn> Columns { get; set; } = new HashSet<TabColumn>();
    }
}