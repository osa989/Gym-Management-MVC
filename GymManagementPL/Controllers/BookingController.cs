using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.BookingViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class BookingController(IBookingService _bookingService) : Controller
    {
        public IActionResult Index()
        {
            var sessions = _bookingService.GetAllSessionsWithTrainersAndCategory();
            return View(sessions);
        }
        public IActionResult GetMembersForUpcomingSession(int id)
        {
            var members = _bookingService.GetAllMemberSession(id);
            return View(members);
        }
        public IActionResult GetMembersForOngoingSession(int id)
        {
            var members = _bookingService.GetAllMemberSession(id);
            return View(members);
        }
        public IActionResult Create(int id)
        {
            var members = _bookingService.GetMemberForDropDowon(id);
            var memberSelectList = new SelectList(members, "Id", "Name");
            ViewBag.Members = memberSelectList;
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateBookingViewModel model)
        {
            var result = _bookingService.CreateBooking(model);
            if(result)
            {
                TempData["SuccessMessage"] = "Booking created successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create booking. Please check the details and try again.";
            }
            return RedirectToAction(nameof(GetMembersForOngoingSession), new {id = model.SessionId});
 
        }
    }
}