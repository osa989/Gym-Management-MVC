using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    internal class TrainerRepository : ITrainerRepositories
    {
        private readonly GymDbContext _dbContext;

        public TrainerRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(Trainer member)
        {
            throw new NotImplementedException();
        }

        public int Delete(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Trainer> GetAll()
        {
            throw new NotImplementedException();
        }

        public Trainer? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int Update(Trainer member)
        {
            throw new NotImplementedException();
        }
    }
}
