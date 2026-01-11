using GymManagementBLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    //[AllowAnonymous] // Allow access to this controller without authentication
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAnalyticsService _analyticsService;

        //Actions
        //BaseUrl/Home/Index    
        public HomeController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }
        public ActionResult Index()
         { 
            var Data = _analyticsService.GetAnalyticsData();
            return View(Data);
        }
    }
}
//
