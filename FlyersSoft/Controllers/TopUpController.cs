using FlyersSoft.Model;
using FlyersSoft.Services.Implementation;
using FlyersSoft.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlyersSoft.Controllers
{
    [Route("TopUp")]
    [Authorize]
    public class TopUpController : Controller
    {
        private readonly ITopupService topupService;
        private readonly ITransactionService transactionService;

        public TopUpController(ITopupService _topupService, ITransactionService _transactionService)
        {
            topupService = _topupService;
            transactionService = _transactionService;
        }
        [HttpGet]
        [Route("getTopUpOptions")]
        public List<TopupOption> getTopUpOptions()
        {
            return topupService.GetTopupOptions();
        }

        [HttpPost]
        [Route("addNewTopUpOption")]
        //[Authorize]
        public JsonResult addNewTopUpOption([FromBody] TopupOption topupOption)
        {
            if (ModelState.IsValid)
            {
                var topup = topupService.addNewTopUpOption(topupOption);
                return Json(new { msg = "success", resp = topup });
            }
            else
                return Json(new { msg = "Invalid request" });
        }

        [HttpPost]
        [Route("initiateTopUp")]
        public JsonResult initiateTopUp([FromBody] Transaction transaction)
        {
            return transactionService.initiateTopUp(transaction);
        }
    }
}
