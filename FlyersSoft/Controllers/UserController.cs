using FlyersSoft.Model;
using FlyersSoft.Services.Implementation;
using FlyersSoft.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlyersSoft.Controllers
{
    [Route("User")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        public UserController(IUserService _userService)
        {
            userService = _userService;
        }
        [HttpPost]
        [Route("addNewUser")]
        [AllowAnonymous]
        public async Task<JsonResult> addNewUser([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                var beneficiary = await userService.addNewUser(user);
                return Json(new { msg = "success", resp = beneficiary });
            }
            else
                return Json(new { msg = "Invalid request" });
        }
    }
}
