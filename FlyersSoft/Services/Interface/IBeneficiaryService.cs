using FlyersSoft.Model;
using Microsoft.AspNetCore.Mvc;

namespace FlyersSoft.Services.Interface
{
    public interface IBeneficiaryService
    {
        public List<UserBeneficiary> GetUserBeneficiaries(int userId);
        public JsonResult addNewBeneficiary(UserBeneficiary userBeneficiary);

    }
}
