using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AccountViewModels;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<ApplicationUser> _signManager;

        public AccountController(IAccountService accountService ,SignInManager<ApplicationUser> signManager)
        {
            _accountService = accountService;
            _signManager = signManager;
        }

        #region Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(AccountViewModel accountViewModel)
        {
            if (!ModelState.IsValid) return View(accountViewModel);
            var user = _accountService.ValidateUser(accountViewModel);
            if (user is null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");
                return View(accountViewModel);
            }
            var Result = _signManager.PasswordSignInAsync(user, accountViewModel.Password, accountViewModel.RememberMe, false).Result;
            if (Result.IsNotAllowed) //correct but not allowed 
                ModelState.AddModelError("InvalidLogin", "You are not allowed to login. Please contact administrator.");
            if (Result.IsLockedOut) // incorrect login
                ModelState.AddModelError("InvalidLogin", "Invalid Email Or Password");
            if (Result.Succeeded)
                return RedirectToAction("Index", "Home");
            return View(accountViewModel);
        }
        #endregion

        //Login
        //Logout
        //AccessDenied
    }
}
