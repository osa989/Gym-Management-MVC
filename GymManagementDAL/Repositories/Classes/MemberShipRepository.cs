using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class MemberShipRepository : GenericRepository<MemberShip>, IMemberShipRepository
    {
        private readonly GymDbContext _context;

        public MemberShipRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<MemberShip> GetAllMemberShipWithMemberAndPlans(Func<MemberShip, bool>? Condition = null)
        {
           var memberships = _context.Memberships.Include(ms => ms.Member).Include(ms => ms.Plan)
                .Where(Condition ?? (_ => true));
            return memberships;
        }
    }
}
