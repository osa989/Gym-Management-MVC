using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public ActionResult Create()
        {
            LoadDropdowns();
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateSessionViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(viewModel);
            }
            var Result = _sessionService.CreateSession(viewModel);
            if (Result)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Session failed create";
                LoadDropdowns();
                return View(viewModel) ;
            }
        }
        #region Helpers
        private void LoadDropdowns()
        {
            var Categories = _sessionService.GetAllCategoriesForDropDown();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");

            var Trainers = _sessionService.GetAllTrainersForDropDown();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
        }
        #endregion
    }
}
