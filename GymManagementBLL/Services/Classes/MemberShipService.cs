using AutoMapper;
using AutoMapper.Execution;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberShipViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class MemberShipService : IMemberShipServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberShipService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public IEnumerable<MemberShipViewModel> GetAllMemberShipWithMemberAndPlans(Func<MemberShipViewModel, bool>? Condition = null)
        {
             var memberShips = _unitOfWork.MemberShipRepository.GetAllMemberShipWithMemberAndPlans(m=>m.Status.ToLower()=="active");
            var memberShipViewModels = _mapper.Map<IEnumerable<MemberShipViewModel>>(memberShips);
            return memberShipViewModels;

        }
        public bool CreateMemberShip(MemberShipViewModel model)
        {
         // make sure member and plan exist
          // and member cannot have morethan one active membership
          if(!IsMemberExist(model.MemberId) || !IsPlanExist(model.PlanId) || HasActiveMemberShip(model.MemberId))
            {
                return false;
            }
            var memberShip = _unitOfWork.MemberShipRepository;
            var membershipToCreate = _mapper.Map<MemberShip>(model);
            var plan = _unitOfWork.GetRepository<Plan>().GetById(model.PlanId);
            membershipToCreate.EndDate= DateTime.UtcNow.AddDays(plan!.DurationDays);
            memberShip.Add(membershipToCreate);
            return _unitOfWork.SaveChanges() > 0;
        }

        #region helper method
        private bool IsMemberExist(int memberId) => _unitOfWork.GetRepository<GymManagementDAL.Entities.Member>().GetById(memberId) != null;
        private bool IsPlanExist(int planId) => _unitOfWork.GetRepository<GymManagementDAL.Entities.Plan>().GetById(planId) != null;
        private bool HasActiveMemberShip(int planId) => _unitOfWork.MemberShipRepository.GetAllMemberShipWithMemberAndPlans(m => m.MemberId == planId && m.Status.ToLower() == "active").Any();

        public IEnumerable<PlanToSelectListViewModel> GetAllPlansToDropDown()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll(p=>p.IsActive);
            var planSelectList = _mapper.Map<IEnumerable<PlanToSelectListViewModel>>(plans);
            return planSelectList;
        }

      public IEnumerable<MemberToSelectListViewModel> GetAllMembersToDropDown()
        {
           
            var members = _unitOfWork.GetRepository<GymManagementDAL.Entities.Member>().GetAll();
            var memberSelectList = _mapper.Map<IEnumerable<MemberToSelectListViewModel>>(members);
            return memberSelectList;
        }

        #endregion
    }
}
