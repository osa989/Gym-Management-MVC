using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class MemberService : IMemberService
    {
      
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberService(IUnitOfWork unitOfWork,IMapper mapper)
        {
          _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<MemberViewModel> GetAllMember()
        {
            var Members = _unitOfWork.GetRepository<Member>().GetAll();
            if (Members == null || !Members.Any()) return []; //Enumerable.Empty<MemberViewModel>(); or this line they are the same  

            // Member -> MemberViewModel => Mapping
            #region Way01 Mannual Mapping
            //var MemberViewModels = new List<MemberViewModel>();
            //foreach (var Member in Members)
            //{
            //    var memberViewModel = new MemberViewModel()
            //    {
            //        Id = Member.Id,
            //        Name = Member.Name,
            //        Phone = Member.Phone,
            //        Email = Member.Email,
            //        Photo = Member.Photo,
            //        Gender = Member.Gender.ToString()
            //    };
            //    MemberViewModels.Add(memberViewModel);
            //} 
            #endregion
            #region Way02 Projection Method  
            //var MemberViewModels = Members.Select(X => new MemberViewModel()
            //{
            //    Id = X.Id,
            //    Name = X.Name,
            //    Email = X.Email,
            //    Phone = X.Phone,
            //    Photo = X.Photo,
            //    Gender = X.Gender.ToString(),
            //});
            #endregion
            var MemberViewModels= _mapper.Map<IEnumerable<MemberViewModel>>(Members);
            return MemberViewModels;
        }
        public bool CreateMember(CreateMemberViewModel createMember)
        {
            try
            {
                if (IsEmailExists(createMember.Email) || IsPhoneExists(createMember.Phone)) return false;
                //mapping from viewmodel to model to add 
                var MappedMember= _mapper.Map<CreateMemberViewModel,Member>(createMember);
                _unitOfWork.GetRepository<Member>().Add(MappedMember);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public MemberViewModel? GetMemberDetails(int MemberId) 
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member is null) return null;
            // Member - MemberViewModel => Manual Mapping
            var memberViewModel =_mapper.Map<MemberViewModel>(member);
        


            var ActivememberShip = _unitOfWork.GetRepository<MemberShip>()
                            .GetAll(X => X.MemberId == MemberId && X.Status == "Active").FirstOrDefault();

            if (ActivememberShip is not null)
            {
                memberViewModel.MemberShipStartDate = ActivememberShip.CreatedAt.ToShortDateString();
                memberViewModel.MemberShipEndDate = ActivememberShip.EndDate.ToShortDateString();
                var plan = _unitOfWork.GetRepository<Plan>().GetById(ActivememberShip.PlanId);
                memberViewModel.PlanName = plan?.Name;
            }
            return memberViewModel;
        }

        public HealthRecordViewModel? GetMemberHealthRecordDetails(int MemberId)
        {
            var MemberHealthRecordRepository = _unitOfWork.GetRepository<HealthRecord>().GetById(MemberId);
            if (MemberHealthRecordRepository == null) return null;
            // HealthRecord => HealthRecordViewModel mannual mapping
            return _mapper.Map<HealthRecordViewModel>(MemberHealthRecordRepository);
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member == null) return null;
            return _mapper.Map<MemberToUpdateViewModel>(member);
        }

        public bool UpdateMemberDetails(int Id, MemberToUpdateViewModel memberToUpdate)
        {
            try
            {
                if (IsEmailExists(memberToUpdate.Email) || IsPhoneExists(memberToUpdate.Phone)) return false;

                var MemberRepo = _unitOfWork.GetRepository<Member>();

                var member = MemberRepo.GetById(Id);
                if (member == null) return false;
                _mapper.Map(memberToUpdate, member);
                MemberRepo.Update(member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveMember(int MemberId)
        {
            try
            {
            var MemberRepo = _unitOfWork.GetRepository<Member>();
                var member = MemberRepo.GetById(MemberId);
                if (member == null) return false;
                var HasActiveMemberSession = _unitOfWork.GetRepository<MemberSession>().
                    GetAll(X => X.MemberId == MemberId && X.Session.CreatedAt > DateTime.Now == false).Any();
                if (HasActiveMemberSession) return false;

                //the delete behavior is cascade so if the member has a realtion with plan it will be deleted automatically 
                var MemberShipRepo = _unitOfWork.GetRepository<MemberShip>();
                var MemberShips = MemberShipRepo.GetAll(X => X.MemberId == MemberId);
                if (MemberShips.Any())
                {
                    foreach (var memberShip in MemberShips)

                        MemberShipRepo.Delete(memberShip);
                    
                }

                MemberRepo.Delete(member);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch 
            {

                return false;
            }
        }

        #region Helper Methods
        private bool IsEmailExists(string Email)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(X => X.Email == Email).Any();
        }
        private bool IsPhoneExists(string Phone)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(X => X.Phone == Phone).Any();
        }


        #endregion
    }
}
