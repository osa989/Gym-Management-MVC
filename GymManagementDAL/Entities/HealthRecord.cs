using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    internal class HealthRecord : BaseEntity
    {
        // Id = > FK === PK [Id] 
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public string BloodType { get; set; } = null!;  
        public string? Note { get; set; }
    }
}
