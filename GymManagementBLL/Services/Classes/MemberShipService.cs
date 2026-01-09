using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberShipViewModel;
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
    }
}
