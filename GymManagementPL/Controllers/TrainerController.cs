using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementBLL.ViewModels.TrainerViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize (Roles="SuperAdmin")]
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }
        #region Get all Trainers
        public IActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();
            return View(trainers);
        }
        #endregion
        #region Get Trainer Details 
        public IActionResult TrainerDetails(int id)
        {
            if (id <= 0)
            {
               TempData["ErrorMessage"] = "Id of Trainer can not be 0 or negative number ";

                return RedirectToAction(nameof(Index));
            }
            var Trainer = _trainerService.GetTrainerDetails(id);
            if (Trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer not found";

                return RedirectToAction(nameof(Index));
            }
            return View(Trainer);
        }
        #endregion

        #region CreateTrainer
         public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateTrainer(CreateTrainerViewModel createTrainerViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check data and Missing fields");
                return View(nameof(Create), createTrainerViewModel);
            }
            bool Result = _trainerService.CreateTrainer(createTrainerViewModel);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";
            //return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer failed to Create, check phone and email";
            //return View(nameof(Create), createTrainerViewModel);
                
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Update Trainer
        public IActionResult TrainerEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Trainer can not be 0 or negative number ";
                return RedirectToAction(nameof(Index));
            }
            var Trainer = _trainerService.GetTrainerToUpdate(id);
            if (Trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found";
                return RedirectToAction(nameof(Index));
            }
            return View(Trainer);
        }
        [HttpPost]
        public IActionResult TrainerEdit(TrainerToUpdateViewModel viewModel, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            bool Result = _trainerService.UpdateTrainerDetails(viewModel, id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer failed to Update, check phone and email";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete Trainer
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Trainer can not be 0 or negative number ";
                return RedirectToAction(nameof(Index));
            }
            var Trainer = _trainerService.GetTrainerDetails(id);
            if (Trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainerId = id;
            return View();
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var Result = _trainerService.RemoveTrainer(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer failed to Delete";
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
