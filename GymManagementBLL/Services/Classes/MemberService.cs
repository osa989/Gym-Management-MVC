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
    }
}
