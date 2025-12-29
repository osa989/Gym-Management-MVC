using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModel;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }
        public IActionResult Index()
        {
            var Plans = _planService.GetAllPlans();
            return View(Plans);
        }

        public ActionResult Details([FromRoute] int id, string ViewName = "Details") // we don't put [FromRoute] here because it is by default
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id ";
                return RedirectToAction(nameof(Index));
            }
            var Plan = _planService.GetPlanDetails(id);
            if (Plan == null)
            {
                TempData["ErrorMessage"] = "Plan not found ";
                return RedirectToAction(nameof(Index));
            }
            return View(ViewName, Plan);
        }
        public ActionResult Edit(int id) //why not put [FromRoute] here
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id ";
                return RedirectToAction(nameof(Index));
            }
            var PlanToUpdate = _planService.GetPlanToUpdate(id);
            if (PlanToUpdate == null)
            {
                TempData["ErrorMessage"] = "Plan not found or cannot be updated ";
                return RedirectToAction(nameof(Index));
            }
            return View(PlanToUpdate);
        }
        [HttpPost]
        public ActionResult Edit( int id, UpdatePlanViewModel UpdatedPlan)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id ";
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check data Validation");
                return View(UpdatedPlan);
            }
            var isUpdated = _planService.UpdatePlan(id, UpdatedPlan);
            if (isUpdated)
            {
            TempData["SuccessMessage"] = "Plan updated successfully ";
            }
            else
            {
                TempData["ErrorMessage"] = "Plan not found or cannot be updated ";
            }
                return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public ActionResult Activate(int id)
        {
            var Result = _planService.ToggleStatus(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Plan status changed ";
            }
            else
            {
                TempData["ErrorMessage"] = "failed to change plan status ";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}