using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModel;
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
            { 
                TempData["ErrorMessage"] = "Id of Member can not be 0 or negative number ";

                return RedirectToAction(nameof(Index));
            }
                
            var Member = _memberService.GetMemberDetails(id);
            if(Member is null)
            {
                TempData["ErrorMessage"] = "Member not found";

                return RedirectToAction(nameof(Index));
            }
            return View(Member);

        }
        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Member can not be 0 or negative number ";
                return RedirectToAction(nameof(Index));

            }
            var HealthRecord= _memberService.GetMemberHealthRecordDetails(id);
            if (HealthRecord is null)
            {
                TempData["ErrorMessage"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }
            return View(HealthRecord);
        }
        #endregion

        #region Add Member
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel createMember)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check data and Missing fields");
                return View(nameof(Create),createMember);
            }
            bool Result = _memberService.CreateMember(createMember);
            if(Result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member failed to Create, check phone and email";
            }
                return RedirectToAction(nameof(Index));
            
        }
        #endregion

        #region Update Member
        public ActionResult MemberEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Member can not be 0 or negative number ";
                return RedirectToAction(nameof(Index));
            }
            var Member = _memberService.GetMemberToUpdate(id);
            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
        }
        [HttpPost]
        public ActionResult MemberEdit(MemberToUpdateViewModel viewModel, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            bool Result = _memberService.UpdateMemberDetails(id,viewModel);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member failed to Update, check phone and email";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete member
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Member can not be 0 or negative number ";
                return RedirectToAction(nameof(Index));
            }
            var Member = _memberService.GetMemberDetails(id);
            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = id;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {
            var Result = _memberService.RemoveMember(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member failed to Delete";
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
