using FlyersSoft.Model;
using FlyersSoft.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FlyersSoft.Services.Implementation
{
    public class TopupService : ITopupService
    {
        private readonly MyDBContext myDBContext;
        public TopupService(MyDBContext _myDBContext)
        {
            myDBContext = _myDBContext;
        }

        public List<TopupOption> GetTopupOptions()
        {
            return myDBContext.TopupOption.Where(x => x.isActive == true).ToList();
        }

        public JsonResult addNewTopUpOption(TopupOption topupOption)
        {
            topupOption.isActive = true;
            topupOption.createdOn = DateTime.Now;
            topupOption.createdBy = 0; //userid need to implement 
            var topup = myDBContext.TopupOption.Add(topupOption).Entity;
            myDBContext.SaveChanges();
            return new JsonResult(new { msg = "success", resp = topup });
        }
    }
}
