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

        public MemberService(IGenericRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
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

    }
}
