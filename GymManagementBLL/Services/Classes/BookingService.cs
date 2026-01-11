using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.BookingViewModel;
using GymManagementBLL.ViewModels.MemberShipViewModel;
using GymManagementBLL.ViewModels.SessionViewModel;
using GymManagementDAL.Entities;
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

        public bool CreateBooking(CreateBookingViewModel model)
        {
            var session=_unitOfWork.SessionRepository.GetById(model.SessionId);
            if(session is null || session.StartDate<=DateTime.UtcNow)
            {
                return false;
            }
            var membershipRepo = _unitOfWork.MemberShipRepository;
            var activeMembership = membershipRepo.GetFirstOrDefault(m=>m.MemberId==model.MemberId && m.Status.ToLower()== "active");
            if (activeMembership is null)
            {
                return false; // member does not have an active membership
            }
            var sessionRepo = _unitOfWork.SessionRepository;
            var bookedSlots = sessionRepo.GetCountOfBookedSlots(model.SessionId);
            var availableSlots = session.Capacity - bookedSlots;
            if (availableSlots == 0)
            {
                return false;
            }
            var booking =  _mapper.Map<MemberSession>(model);
            booking.IsAttended = false;
            _unitOfWork.BookingRepository.Add(booking);
            return _unitOfWork.SaveChanges() > 0;
        }
        public IEnumerable<MemberToSelectListViewModel> GetMemberForDropDowon(int id)
        {
            var bookingRepo = _unitOfWork.BookingRepository;
            var bookedmemberIds = bookingRepo.GetAll(s=>s.Id==id).Select(b=>b.MemberId).ToList();
            var memberAvailaleToBook = _unitOfWork.GetRepository<Member>().GetAll(m=>!bookedmemberIds.Contains(m.Id));
            var memberToSelectListViewModels = _mapper.Map<IEnumerable<MemberToSelectListViewModel>>(memberAvailaleToBook);
            return memberToSelectListViewModels;
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
