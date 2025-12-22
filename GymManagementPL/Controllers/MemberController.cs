using GymManagementBLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        #region Get All Members 
        public ActionResult Index()
        {
            var members = _memberService.GetAllMember();
            return View(members);
        }
        #endregion

        #region GetMember Data
            public ActionResult MemberDetails(int id)
                {
            // BaseUrl/Member/MemberDetails/1 may be 0 
            if (id<=0)
            return RedirectToAction(nameof(Index));
                
            var Member = _memberService.GetMemberDetails(id);
            if(Member is null)
                return RedirectToAction(nameof(Index));
            return View(Member);

        }
        #endregion

        #region show member health record
        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
                return RedirectToAction(nameof(Index));
            var HealthRecord= _memberService.GetMemberHealthRecordDetails(id);
            if (HealthRecord is null)
                return RedirectToAction(nameof(Index));
            return View(HealthRecord);
        }
        #endregion
    }
}
