using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    internal interface ITrainerRepositories
    {
        IEnumerable<Trainer> GetAll();

        Trainer? GetById(int id);

        int Add(Trainer member);

        int Update(Trainer member);

        int Delete(int Id);
    }
}
