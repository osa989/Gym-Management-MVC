using GymManagementBLL.ViewModels.BookingViewModel;
using GymManagementBLL.ViewModels.MemberShipViewModel;
using GymManagementBLL.ViewModels.SessionViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IBookingService
    {
        IEnumerable<SessionViewModel> GetAllSessionsWithTrainersAndCategory();

        IEnumerable<MemberForSessionViewModel> GetAllMemberSession(int sessionId);

        public bool CreateBooking(CreateBookingViewModel model); 


        IEnumerable<MemberToSelectListViewModel> GetMemberForDropDowon(int id);
    }
}
