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
    public class SessionService : ISessionService
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
        public bool CreateSession(CreateSessionViewModel createSession)
        {
            try
            {
                if (!IsTrainerExists(createSession.TrainerId)) return false;
                if (!IsCategoryExists(createSession.CategoryId)) return false;
                if (!IsValidDateRange(createSession.StartDate, createSession.EndDate)) return false;
                // CreateSessionViewModel- session 
                var MappedSession = _mapper.Map<CreateSessionViewModel, Session>(createSession);
                _unitOfWork.GetRepository<Session>().Add(MappedSession);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch 
            {

                return false;
            }

        }
        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
         var Session = _unitOfWork.SessionRepository.GetById(sessionId);
            if(!IsSessionAvailableForUpdating(Session!)) return null;
            return _mapper.Map<UpdateSessionViewModel>(Session!);//Map with destination only


        }

        public bool UpdateSession(UpdateSessionViewModel updateSession, int sessionId)
        {
            try
            {
                var Session = _unitOfWork.SessionRepository.GetById(sessionId);
                if (!IsSessionAvailableForUpdating(Session!)) return false;
                if (!IsTrainerExists(updateSession.TrainerId)) return false;
                if (!IsValidDateRange(updateSession.StartDate, updateSession.EndDate)) return false;
                _mapper.Map(updateSession, Session);
                Session!.UpdatedAt = DateTime.Now;
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
        public bool RemoveSession(int sessionId)
        {
            try
            {
                var Session = _unitOfWork.SessionRepository.GetById(sessionId);
                if (!IsSessionAvailableForRemoving(Session!)) return false;
                _unitOfWork.GetRepository<Session>().Delete(Session!);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch 
            {

               return false;
            }
        }
        #region Helpers  for validation
        private bool IsTrainerExists(int trainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(trainerId) is not null;
            
        }
        private bool IsCategoryExists(int categoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(categoryId) is not null;
        }

        private bool IsValidDateRange(DateTime startDate, DateTime endDate)
        {
            return startDate < endDate && startDate>DateTime.Now;
        }

        private bool IsSessionAvailableForRemoving(Session session)
        {
         
            if (session == null) return false;
            if (session.StartDate > DateTime.Now) return false; // Upcoming sessions cannot be updated unless there are no bookings
            if (session.StartDate <= DateTime.Now && session.EndDate >DateTime.Now) return false; // ongoing sessions cannot be updated
            var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id)>0;// A session is available for updating if there are no booked slots
            if (HasActiveBooking) return false;
          return true;
        }

        private bool IsSessionAvailableForUpdating(Session session)
        {

          
            if (session == null) return false;
            if (session.EndDate <= DateTime.Now) return false; // completed sessions cannot be updated
            if (session.StartDate <= DateTime.Now) return false; // ongoing sessions cannot be updated
            var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;// A session is available for updating if there are no booked slots
            if (HasActiveBooking) return false;
           return true;
        }



        #endregion
    }
}
