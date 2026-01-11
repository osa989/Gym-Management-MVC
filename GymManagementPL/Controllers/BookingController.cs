using GymManagementBLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class BookingController(IBookingService _bookingService) : Controller
    {
        public IActionResult Index()
        {
            var sessions = _bookingService.GetAllSessionsWithTrainersAndCategory();
            return View(sessions);
        }
    }
}
