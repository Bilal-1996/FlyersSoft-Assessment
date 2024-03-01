using FlyersSoft.Model;
using Microsoft.AspNetCore.Mvc;

namespace FlyersSoft.Services.Interface
{
    public interface ITopupService
    {
        public List<TopupOption> GetTopupOptions();

        public JsonResult addNewTopUpOption(TopupOption topupOption);
    }
}
