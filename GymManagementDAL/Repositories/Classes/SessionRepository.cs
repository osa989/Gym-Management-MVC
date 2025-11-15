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
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;

        public SessionRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Session> GetAllSessionsWithTrainerAndCategories()
        {
            return _dbContext.Sessions
                .Include(X => X.SessionTrainer)
                .Include(X => X.SessionCategory)
                .ToList();  // toList() to execute the query and return the results immediately


        }

        public int GetCountOfBookedSlots(int sessionId)
        {
            // by using MemberSession table that made by many to many relationship between Member and Session
            return _dbContext.MemberSessions.Count(X => X.SessionId == sessionId);
        }

        public Session? GetSessionByIdWithTrainerAndCategories(int Id)
        {
            return _dbContext.Sessions
                .Include(X => X.SessionTrainer)
                .Include(X => X.SessionCategory)
                .FirstOrDefault(X => X.Id == Id);
        }
    }
}
