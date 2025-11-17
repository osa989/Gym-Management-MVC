using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class HomeController : Controller
    {
        //Actions
        //BaseUrl/Home/Index    
        public ViewResult Index()
        {
            return View();
        }

        public JsonResult Trainers()
        {
            var Trainers = new List<Trainer>()
            {
                new Trainer() { Name = "Ahmed", Phone = "011254696" },
                new Trainer() { Name = "Aya", Phone = "01127777" }
            };
            return Json(Trainers);
        }
        public ContentResult Content()
        {
            return Content("<h1>Hello From Gym Management System</h1>", "text/html");
        }

        public FileResult Downloadfile()
        {
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "css", "site.css");
            var FileBytes = System.IO.File.ReadAllBytes(FilePath);

            return File(FileBytes, "text/css", "DownloadSite.css");
        }

        public EmptyResult EmptyAction()
        {
            return new EmptyResult();
        }


    }
}
