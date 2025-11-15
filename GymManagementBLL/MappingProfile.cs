using AutoMapper;
using GymManagementBLL.ViewModels.SessionViewModel;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Session, SessionViewModel>().ForMember(dest=>dest.TrainerName, opt => opt.MapFrom(src => src.SessionTrainer.Name))
                                                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.SessionCategory.CategoryName))
                                                .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore());
        }
    }
}
 