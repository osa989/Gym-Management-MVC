using GymManagementBLL.Services.Interfaces;
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
        
        public ActionResult Details([FromRoute]int id, string ViewName ="Details")
        {
            if(id<= 0)
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
            return View(ViewName,Plan);
        }
        public ActionResult Edit([FromRoute] int id)
        {
            //if (id <= 0)
            //{
            //    TempData["ErrorMessage"] = "Invalid Plan Id ";
            //    return RedirectToAction(nameof(Index));
            //}
            //var PlanToUpdate = _planService.GetPlanToUpdate(id);
            //if (PlanToUpdate == null)
            //{
            //    TempData["ErrorMessage"] = "Plan not found or cannot be updated ";
            //    return RedirectToAction(nameof(Index));
            //}
            return Details(id,"Edit");
        }
        [HttpPost]
        public ActionResult Edit([FromRoute] int id, UpdatePlanViewModel UpdatedPlan)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id ";
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                return View(UpdatedPlan);
            }
            var isUpdated = _planService.UpdatePlan(id, UpdatedPlan);
            if (!isUpdated)
            {
                TempData["ErrorMessage"] = "Plan not found or cannot be updated ";
                return RedirectToAction(nameof(Index));
            }
            TempData["SuccessMessage"] = "Plan updated successfully ";
            return RedirectToAction(nameof(Index));
        }
}
