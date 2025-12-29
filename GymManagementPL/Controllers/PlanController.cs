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
        
        public ActionResult Details([FromRoute]int id)
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
            return View(Plan);
        }
    }
}
