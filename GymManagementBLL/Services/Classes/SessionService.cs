using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    internal class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SessionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            //var Sessions = _unitOfWork.GetRepository<Session>().GetAll();
            var Sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategories();
            if (Sessions is null || !Sessions.Any()) return [];


            // Mapping Session entities to SessionViewModel
            return Sessions.Select(X => new SessionViewModel()
            {
                Id = X.Id,
                Capacity= X.Capacity,
                Description = X.Description,
                EndDate = X.EndDate,
                StartDate = X.StartDate,
                TrainerName = X.SessionTrainer.Name,
                CategoryName = X.SessionCategory.CategoryName, //by default not loaded 
                AvailableSlots = X.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(X.Id),
            });


        }
    }
}
