using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    internal class MemberShip:BaseEntity
    {
        // StartDate - CreatedAt of baseEntity
        public DateTime EndDate { get; set; }
        // Read only property 
        public string Status
        { 
            get
            {
                if (EndDate >= DateTime.Now)
                    return "Expired";
                else
                    return "Expired";
                
            } 
        }


        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public int PlanId { get; set; }
        public Plan Plan { get; set; } = null!;
    }
}
