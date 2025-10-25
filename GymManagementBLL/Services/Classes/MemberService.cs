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
    internal class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _memberRepository;
        private readonly IGenericRepository<MemberShip> _membershipRepository;
        private readonly IPlanRepository _planRepository;
        private readonly IGenericRepository<HealthRecord> _helathRedcordRepository;

        public MemberService(IGenericRepository<Member> memberRepository,IGenericRepository<MemberShip> membershipRepository,IPlanRepository planRepository, IGenericRepository<HealthRecord> helathRedcordRepository)
        {
            _memberRepository = memberRepository; 
            _membershipRepository = membershipRepository;
            _planRepository = planRepository;
            _helathRedcordRepository = helathRedcordRepository;
        }

        public IEnumerable<MemberViewModel> GetAllMember()
        {
            var Members = _memberRepository.GetAll();
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
            var MemberViewModels = Members.Select(X => new MemberViewModel()
            {
                Id = X.Id,
                Name = X.Name,
                Email = X.Email,
                Phone = X.Phone,
                Photo = X.Photo,
                Gender= X.Gender.ToString(),
            });
            #endregion
            return MemberViewModels;
        }
        public bool CreateMember(CreateMemberViewModel createMember)
        {
            try
            {
                var EmailExists = _memberRepository.GetAll(X => X.Email == createMember.Email).Any();
                var PhoneExists = _memberRepository.GetAll(X => X.Phone == createMember.Phone).Any();

                //mapping from viewmodel to model to add 
                var member = new Member()
                {
                    Name = createMember.Name,
                    Email = createMember.Email,
                    Phone = createMember.Phone,
                    Gender = createMember.Gender,
                    DateOfBirth = createMember.DateOfBirth,
                    Address = new Address()
                    {
                        BuildingNumber = createMember.BuildingNumber,
                        City = createMember.City,
                        Street = createMember.Street
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Height = createMember.HealthRecordViewModel.Height,
                        Weight = createMember.HealthRecordViewModel.Weight,
                        BloodType = createMember.HealthRecordViewModel.BloodType,
                        Note = createMember.HealthRecordViewModel.Note,
                    }
                };
                return _memberRepository.Add(member) > 0;
            }
            catch 
            {
                return false;
            }
        }

        public MemberViewModel? GetMemberDetails(int MemberId)
        {
            var member = _memberRepository.GetById(MemberId);
            if (member == null) return null;

            // Member - MemberViewModel => Manual Mapping
            var viewModel = new MemberViewModel()
            {

                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}"
            };
            

            var ActivememberShip = _membershipRepository
                            .GetAll(X => X.MemberId == MemberId && X.Status == "Active").FirstOrDefault();

            if (ActivememberShip is not null)
            {
                viewModel.MemberShipStartDate = ActivememberShip.CreatedAt.ToShortDateString();
                viewModel.MemberShipEndDate = ActivememberShip.EndDate.ToShortDateString();
                var plan = _planRepository.GetById(ActivememberShip.PlanId);
                viewModel.PlanName = plan?.Name;
            }
            return viewModel;
        }

        public HealthRecordViewModel? GetMemberHealthRecordDetails(int MemberId)
        {
            var MemberHealthRecordRepository = _helathRedcordRepository.GetById(MemberId);
            if (MemberHealthRecordRepository == null) return null;
            // HealthRecord => HealthRecordViewModel mannual mapping
            return new HealthRecordViewModel()
            {
                Height = MemberHealthRecordRepository.Height,
                Weight = MemberHealthRecordRepository.Weight,
                BloodType = MemberHealthRecordRepository.BloodType,
                Note = MemberHealthRecordRepository.Note,
            };
        }
    }
}
