using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        // to make specific repository methods for Session entity to load navigation properties
        IEnumerable<Session> GetAllSessionsWithTrainerAndCategories();
        int GetCountOfBookedSlots(int sessionId);
    }
}
