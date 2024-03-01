using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlyersSoft.Model
{
    public class Transaction : BaseEntity
    {
        [Key(), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int transactionId { get; set; }
        public int userBeneficiaryId { get; set; }

        [ForeignKey("userBeneficiaryId")]
        public virtual UserBeneficiary? userBeneficiary { get; set; }
        public int topupOptionId { get; set; }
        public decimal? topUpAmount { get; set; }
        public decimal? transactionAmount { get; set; }
        public TransactionStatus status { get; set; }
    }
    public enum TransactionStatus
    {
        Success = 1,
        Failure = 0
    }
}
