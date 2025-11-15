using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class HomeController : Controller
    {
        //Actions
        //BaseUrl/Home/Index    
        public IActionResult Index()
        {
            return View();
        }
    }
}
