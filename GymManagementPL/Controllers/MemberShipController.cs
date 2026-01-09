using GymManagementBLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
            return View();
        }
    }
        
}
