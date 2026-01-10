using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberShipViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class MemberShipController : Controller
    {
        private readonly IMemberShipServices _memberShipServices;

        public MemberShipController(IMemberShipServices memberShipServices)
        {
            _memberShipServices = memberShipServices;
        }
        public IActionResult Index()
        {
           var memberShips= _memberShipServices.GetAllMemberShipWithMemberAndPlans();
            return View(memberShips);
        }
        public ActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateMemberShipViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Result = _memberShipServices.CreateMemberShip(model);
                if (Result)
                {
                    TempData["SuccessMessage"] = "Membership created successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    
                    TempData["ErrorMessage"] = "Failed to create membership. Please ensure the member and plan exist, and the member does not have an active membership.";
                    return RedirectToAction("Index");
                }
            }
            TempData["ErrorMessage"] = "Invalid data. Please correct the errors and try again.";
            LoadDropdowns();
            return View(model);
        }
        public IActionResult Cancel(int id) {
            {
                var result =_memberShipServices.DeleteMemberShip(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Membership canceled successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to cancel membership. Please try again.";
                    return RedirectToAction("Index");


                }
                
            }
        }
        #region Helper methode
        public void LoadDropdowns()
        {
            var members = _memberShipServices.GetAllMembersToDropDown();
            var plans = _memberShipServices.GetAllPlansToDropDown();
            ViewBag.Members = new SelectList(members, "Id", "Name");
            ViewBag.Plans = new SelectList(plans, "Id", "Name");
        }
        #endregion
    }
        
}
