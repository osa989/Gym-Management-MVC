using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Member : GymUser
    {
        // created at => join date
        public string Photo { get; set; } = null!;

         public ICollection<MemberSession> MemberSession { get; set; } = null!;
        #region Relations
        #region Member - HealthRecord
        public HealthRecord HealthRecord { get; set; } = null!;
        #endregion
        #region Member - Membership
        public ICollection<MemberShip> MemberShip { get; set; } = null!;
        #endregion

        #region Member - MemberSession 
        #endregion
        #endregion
    }
}
