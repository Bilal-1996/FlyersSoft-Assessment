using FlyersSoft.Model;
using FlyersSoft.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FlyersSoft.Services.Implementation
{
    public class BeneficiaryService : IBeneficiaryService
    {
        private readonly MyDBContext myDBContext;
        public BeneficiaryService(MyDBContext _myDBContext)
        {
            myDBContext = _myDBContext;
        }
        public JsonResult addNewBeneficiary(UserBeneficiary userBeneficiary)
        {
            if (GetUserBeneficiaries(userBeneficiary.userId).Count >= 5)
                return new JsonResult(new { msg = "Max beneficiary limit exceeded"});

            if(myDBContext.Beneficiary.Where(x => x.userId == userBeneficiary.userId 
                && x.beneficiaryName == userBeneficiary.beneficiaryName 
                && x.isActive == true).Any())
                return new JsonResult(new { msg = "Duplicate beneficiary name" });

            userBeneficiary.isActive = true;
            userBeneficiary.createdOn = DateTime.Now;
            userBeneficiary.createdBy = userBeneficiary.userId;
            var beneficiary = myDBContext.Beneficiary.Add(userBeneficiary).Entity;
            myDBContext.SaveChanges();
            return new JsonResult(new { msg = "success", resp = beneficiary });
        }

        public List<UserBeneficiary> GetUserBeneficiaries(int userId)
        {
            return myDBContext.Beneficiary.Where(x => x.userId == userId && x.isActive == true).ToList();
        }
    }
}
