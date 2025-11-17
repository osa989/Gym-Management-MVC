using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class HomeController : Controller
    {
        //Actions
        //BaseUrl/Home/Index    
        public ActionResult Index()
        { 
            return View();
        }
    }
}
