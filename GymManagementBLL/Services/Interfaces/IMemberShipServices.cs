using GymManagementBLL.ViewModels.MemberShipViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMemberShipServices
    {
        IEnumerable<MemberShipViewModel> GetAllMemberShipWithMemberAndPlans(Func<MemberShipViewModel, bool>? Condition = null);

        IEnumerable<PlanToSelectListViewModel> GetAllPlansToDropDown();
        IEnumerable<MemberToSelectListViewModel> GetAllMembersToDropDown();
        bool CreateMemberShip(CreateMemberShipViewModel model);

        bool DeleteMemberShip(int memberId);
    }
}
