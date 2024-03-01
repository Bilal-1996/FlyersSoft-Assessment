using FlyersSoft.Model;
using Microsoft.AspNetCore.Mvc;

namespace FlyersSoft.Services.Interface
{
    public interface IUserService
    {
        public Task<JsonResult> addNewUser(User user);
        public Task<bool> isValidUser(string userName, string password);
    }
}
