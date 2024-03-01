using FlyersSoft.Model;
using FlyersSoft.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FlyersSoft.Controllers
{
    [Route("Beneficiary")]
    [Authorize]
    public class BeneficiaryController : Controller
    {
        private readonly IBeneficiaryService beneficiaryService;

        
        public BeneficiaryController(IBeneficiaryService _beneficiaryService)
        {
            beneficiaryService = _beneficiaryService;
        }
        [HttpGet]
        [Route("getUserBeneficiaries")]
        public List<UserBeneficiary> getUserBeneficiaries(int userId)
        {
            return beneficiaryService.GetUserBeneficiaries(userId);
        }

        [HttpPost]
        [Route("addNewBeneficiary")]
        public JsonResult addNewBeneficiary([FromBody]  UserBeneficiary userBeneficiary)
        {
            if (ModelState.IsValid)
            {
                var beneficiary = beneficiaryService.addNewBeneficiary(userBeneficiary);
                return Json(new { msg = "success", resp = beneficiary });
            }
            else
                return Json(new { msg = "Invalid request" });
        }
    }
}
