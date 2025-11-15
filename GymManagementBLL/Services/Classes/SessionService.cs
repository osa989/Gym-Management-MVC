using AutoMapper;
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
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            //var Sessions = _unitOfWork.GetRepository<Session>().GetAll();
            var Sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategories();
            if (Sessions is null || !Sessions.Any()) return [];


            #region Mannual Mapping
            //// Mapping Session entities to SessionViewModel
            //return Sessions.Select(X => new SessionViewModel()
            //{
            //    Id = X.Id,
            //    Capacity = X.Capacity,
            //    Description = X.Description,
            //    EndDate = X.EndDate,
            //    StartDate = X.StartDate,
            //    TrainerName = X.SessionTrainer.Name,
            //    CategoryName = X.SessionCategory.CategoryName, //by default not loaded 
            //    AvailableSlots = X.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(X.Id),
            //}); 
            #endregion
            #region Automatic Mapping
            var MappedSessions = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(Sessions);
            return MappedSessions;
            #endregion

        }

        public SessionViewModel? GetSessionById(int id)
        {
            var Session = _unitOfWork.SessionRepository.GetSessionByIdWithTrainerAndCategories(id);
            if (Session is null) return null;
                var MappedSession = _mapper.Map<Session, SessionViewModel>(Session);
            return MappedSession;
        }
    }
}
