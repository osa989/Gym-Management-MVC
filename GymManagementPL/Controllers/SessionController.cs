using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModel;
using GymManagementBLL.ViewModels.SessionViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    [Authorize]
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
            LoadCategoryDropdowns();
            LoadTrainerDropdowns();
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateSessionViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                LoadCategoryDropdowns();
                LoadTrainerDropdowns();
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
                LoadCategoryDropdowns();
                LoadTrainerDropdowns();
                return View(viewModel) ;
            }
        }

        public ActionResult Edit(int id) //why not put [FromRoute] here
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id ";
                return RedirectToAction(nameof(Index));
            }
            var Session = _sessionService.GetSessionToUpdate(id);
            if (Session == null)
            {
                TempData["ErrorMessage"] = "Plan not found or cannot be updated ";
                return RedirectToAction(nameof(Index));
            }
            LoadTrainerDropdowns();
            return View(Session);
        }
        [HttpPost]
        public ActionResult Edit(int id, UpdateSessionViewModel updateSession)
        {
            if (id <= 0)
            { 
                TempData["ErrorMessage"] = "Invalid Plan Id ";
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                LoadTrainerDropdowns();
                ModelState.AddModelError("WrongData", "Check data Validation");
                return View(updateSession);
            }
            var isUpdated = _sessionService.UpdateSession(updateSession, id);
            if (isUpdated)
            {
                TempData["SuccessMessage"] = "Session updated successfully ";
            }
            else
            {
                TempData["ErrorMessage"] = "Session not found or cannot be updated ";

            }
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id ";
                return RedirectToAction(nameof(Index));
            }
            var Session = _sessionService.GetSessionById(id);
            if (Session == null)
            {
                TempData["ErrorMessage"] = "Plan not found or cannot be updated ";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SessionId = id;
            return View();
        }
        public ActionResult DeleteConfirmed(int id)
        {
            var Result =_sessionService.RemoveSession(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Session deleted successfully ";
            }
            else
            {
                TempData["ErrorMessage"] = "Session Failed To delete ";

            }
            return RedirectToAction(nameof(Index));

        }
        #region Helpers
        private void LoadTrainerDropdowns()
        {
            var Categories = _sessionService.GetAllCategoriesForDropDown();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");

            var Trainers = _sessionService.GetAllTrainersForDropDown();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
        }
        private void LoadCategoryDropdowns()
        {
            var Categories = _sessionService.GetAllCategoriesForDropDown();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");

            var Trainers = _sessionService.GetAllTrainersForDropDown();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
        }
        #endregion
    }
}
