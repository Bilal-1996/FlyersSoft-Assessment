using FlyersSoft.Model;
using FlyersSoft.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace FlyersSoft.Services.Implementation
{
    public class TransactionService : ITransactionService
    {
        private readonly MyDBContext myDBContext;
        private readonly IConfiguration config;
        public TransactionService(MyDBContext _myDBContext, IConfiguration _config)
        {
            myDBContext = _myDBContext;
            config = _config;
        }

        public JsonResult initiateTopUp(Transaction transaction)
        {
            var beneficiary = myDBContext.Beneficiary.Include(x=> x.user).Where(x => x.userBeneficiaryId == transaction.userBeneficiaryId).FirstOrDefault();

            if (beneficiary == null)
                return new JsonResult(new { msg = "Invalid request" });

            var topUpOption = myDBContext.TopupOption.Where(x => x.topupOptionId == transaction.topupOptionId).FirstOrDefault();

            if (topUpOption == null)
                return new JsonResult(new { msg = "Invalid request" });


            int perBeneficiarlyLimit = beneficiary.user.isVerified ? Convert.ToInt32(config.GetValue<string>("VerifiedLimit")) : Convert.ToInt32(config.GetValue<string>("NonVerifiedLimit"));

            decimal? perBeneficiaryAmt = myDBContext.Transaction.Where(x => x.userBeneficiaryId == transaction.userBeneficiaryId
                && x.createdOn.Year == DateTime.Now.Year
                && x.createdOn.Month == DateTime.Now.Month).ToList()?.GroupBy(x => x.userBeneficiaryId).FirstOrDefault()?.Sum(x => x.topUpAmount);

            if (perBeneficiaryAmt == null)
                perBeneficiaryAmt = 0;

            if (perBeneficiaryAmt+ topUpOption.amount > perBeneficiarlyLimit)
                return new JsonResult(new { msg = "Transaction is failed since it exceeds the top up limit of this beneficiary for a calendar month" });

            decimal? overallAmt = myDBContext.Transaction.Include(x => x.userBeneficiary).Include(x => x.userBeneficiary.user).Where(x=>
                x.userBeneficiary.user.userId == beneficiary.userId
                && x.createdOn.Year == DateTime.Now.Year
                && x.createdOn.Month == DateTime.Now.Month
                ).ToList()?.GroupBy(x => x.userBeneficiary.user.userId).FirstOrDefault()?.Sum(x => x.topUpAmount);

            if (overallAmt == null)
                overallAmt = 0;

            if (overallAmt + topUpOption.amount > Convert.ToInt32(config.GetValue<string>("OverallLimit")))
                return new JsonResult(new { msg = "Transaction is failed. Since it exceeds your overall top up limit for a calendar month" });

            //call external HTTP service to get user wallet balance 
            decimal userBalance = 5000; 

            transaction.transactionAmount = topUpOption.amount + Convert.ToDecimal(config.GetValue<string>("TransactionCharge"));
            transaction.topUpAmount = topUpOption.amount;

            if (userBalance < transaction.transactionAmount)
                return new JsonResult(new { msg = "Wallet balance is not enough to do this top up" });

            
            //call external HTTP service to debit the transaction amount from the user wallet.
            //param => transaction.transactionAmount
            var resp = "sucess";

            if (resp != "sucess")
                return new JsonResult(new { msg = "Error occurred while debiting user account balance" });

            //call external HTTP service to do the top up and then below code will execute to save the transaction in DB
            var topUpResp = "sucess";

            if (topUpResp != "sucess")
                return new JsonResult(new { msg = "Error occurred while calling top up service" });

            transaction.createdOn = DateTime.Now;
            transaction.createdBy = 0; //userid need to implement 
            var trans = myDBContext.Transaction.Add(transaction).Entity;
            myDBContext.SaveChanges();
            trans.userBeneficiary = null;
            return new JsonResult(new { msg = "success", resp = trans });
        }
    }
}
