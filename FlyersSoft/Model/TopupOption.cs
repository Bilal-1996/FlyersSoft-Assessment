using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlyersSoft.Model
{
    public class TopupOption : BaseEntity
    {
        [Key(), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int topupOptionId { get; set; }
        [Required]
        public string? topupOptionName { get; set; }
        [Required]
        public decimal amount { get; set; }
    }
}
