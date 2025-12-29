using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public IActionResult Index()
        {
            var Sessions = _sessionService.GetAllSessions();
            return View(Sessions);
        }
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id ";
                return RedirectToAction(nameof(Index));
            }
            var Session = _sessionService.GetSessionById(id);
            if (Session == null)
            {
                TempData["ErrorMessage"] = "Session not found ";
                return RedirectToAction(nameof(Index));
            }
            return View(Session);
        }

    }
}
