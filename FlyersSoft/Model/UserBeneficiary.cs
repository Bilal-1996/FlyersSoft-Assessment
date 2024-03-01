using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FlyersSoft.Model
{
    public class UserBeneficiary : BaseEntity
    {
        [Key(), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userBeneficiaryId { get; set; }
        public int userId { get; set; }

        
        [ForeignKey("userId")]
        public virtual User? user { get; set; }

        [Required]
        [MaxLength(20)]
        public string beneficiaryName { get; set; }
    }
}
