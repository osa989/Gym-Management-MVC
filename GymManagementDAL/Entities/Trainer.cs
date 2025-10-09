using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    internal class Trainer : GymUser
    {
        // HireDate = > CreatedAt of baseEntity 
        public Specialties Specialties { get; set; }

        public ICollection<Session> TrainerSessions { get; set; } = null!;
    }
}
