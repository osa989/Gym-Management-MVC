using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.BookingViewModel;
using GymManagementBLL.ViewModels.SessionViewModel;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<MemberForSessionViewModel> GetAllMemberSession(int sessionId)
        {
            var bookingRepo = _unitOfWork.BookingRepository;
            var MemberforSession = bookingRepo.GetSessionById(sessionId);
            var memberForSessionViewModels = _mapper.Map<IEnumerable<MemberForSessionViewModel>>(MemberforSession);
            return memberForSessionViewModels;
        }

        public IEnumerable<SessionViewModel> GetAllSessionsWithTrainersAndCategory()
        {
            var sessionRepo = _unitOfWork.SessionRepository;
            var sessions= sessionRepo.GetAllSessionsWithTrainerAndCategories();
            var SessionViewModels = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);
            foreach (var sessionViewModel in SessionViewModels)
            {
                var bookedSlots = sessionRepo.GetCountOfBookedSlots(sessionViewModel.Id);
                sessionViewModel.AvailableSlots = sessionViewModel.Capacity - bookedSlots;
            }
            return SessionViewModels;

        }
    }
}
