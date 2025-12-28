using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork,IMapper mapper)
		{
			_unitOfWork = unitOfWork;
            _mapper = mapper;
        }
		public bool CreateTrainer(CreateTrainerViewModel createdTrainer)
		{
			try
			{
				var Repo = _unitOfWork.GetRepository<Trainer>();

				if (IsEmailExists(createdTrainer.Email) || IsPhoneExists(createdTrainer.Phone)) return false;
				var Trainer = _mapper.Map<Trainer>(createdTrainer);


                Repo.Add(Trainer);

				return _unitOfWork.SaveChanges() > 0;


			}
			catch (Exception)
			{
				return false;
			}
		}

		public IEnumerable<TrainerViewModel> GetAllTrainers()
		{
			var Trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
			if (Trainers is null || !Trainers.Any()) return [];

			return _mapper.Map<IEnumerable<TrainerViewModel>>(Trainers);
        }

		public TrainerViewModel? GetTrainerDetails(int trainerId)
		{
			var Trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
			if (Trainer is null) return null;


			return _mapper.Map<TrainerViewModel>(Trainer);
        }
		public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId)
		{
			var Trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
			if (Trainer is null) return null;

			return _mapper.Map<TrainerToUpdateViewModel>(Trainer);
        }
		public bool RemoveTrainer(int trainerId)
		{
			var Repo = _unitOfWork.GetRepository<Trainer>();
			var TrainerToRemove = Repo.GetById(trainerId);
			if (TrainerToRemove is null || HasActiveSessions(trainerId)) return false;
			Repo.Delete(TrainerToRemove);
			return _unitOfWork.SaveChanges() > 0;
		}

		public bool UpdateTrainerDetails(TrainerToUpdateViewModel updatedTrainer, int trainerId)
		{
			var Repo = _unitOfWork.GetRepository<Trainer>();
			var TrainerToUpdate = Repo.GetById(trainerId);

			if (TrainerToUpdate is null || IsEmailExists(updatedTrainer.Email) || IsPhoneExists(updatedTrainer.Phone)) return false;
_mapper.Map(updatedTrainer, TrainerToUpdate);
            Repo.Update(TrainerToUpdate);
			return _unitOfWork.SaveChanges() > 0;
		}

		#region Helper Methods

		private bool IsEmailExists(string email)
		{
			var existing = _unitOfWork.GetRepository<Member>().GetAll(
				m => m.Email == email).Any();
			return existing;
		}

		private bool IsPhoneExists(string phone)
		{
			var existing = _unitOfWork.GetRepository<Member>().GetAll(
				m => m.Phone == phone).Any();
			return existing;
		}

		private bool HasActiveSessions(int Id)
		{
			var activeSessions = _unitOfWork.GetRepository<Session>().GetAll(
			   s => s.TrainerId == Id && s.CreatedAt > DateTime.Now).Any();
			return activeSessions;
		}
		#endregion
    }
}
