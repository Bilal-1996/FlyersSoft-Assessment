using FlyersSoft.Model;
using Microsoft.AspNetCore.Mvc;

namespace FlyersSoft.Services.Interface
{
    public interface ITransactionService
    {
        public JsonResult initiateTopUp(Transaction transaction);
    }
}
