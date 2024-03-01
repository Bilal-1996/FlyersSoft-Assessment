using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FlyersSoft.Model
{
    public class User : BaseEntity
    {
        [Key(), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int userId { get; set; }
        [Required]
        public string userName { get; set; }
        public string? passwordString { get; set; }
        public byte[]? password { get; set; }
        public bool isVerified { get; set; }
    }
}
