using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IMemberShipRepository : IGenericRepository<MemberShip>
    {
        IEnumerable<MemberShip> GetAllMemberShipWithMemberAndPlans(Func<MemberShip, bool>? Condition = null);
        MemberShip? GetFirstOrDefault(Func<MemberShip, bool> Condition);



    }
}
