using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.TaskManager.Extensions;
using MVC.TaskManager.Repositories.Interface;
using MVC.TaskManager.ViewModels;

namespace MVC.TaskManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsersAsync();

            var model = new List<UserViewModel>();

            foreach (var user in users)
            {
                model.Add(new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = await _userRepository.GetUserRoleAsync(user)
                });
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Register(Guid? id, string returnUrl = null)
        {
            if (id.HasValue)
            {
                var userDB = await _userRepository.GetUserByIdAsync(id.Value);
                if (userDB == null)
                {
                    return RedirectToAction("Index", "Account");
                }

                var role = await _userRepository.GetUserRoleAsync(userDB);

                var model = new RegisterViewModel
                {
                    UserName = userDB.UserName ?? String.Empty,
                    Email = userDB.Email,
                    SelectedRole = role
                };

                ViewData["ReturnUrl"] = returnUrl;
                ViewData["Roles"] = new SelectList(await _userRepository.GetAllRolesAsync(), "Id", "Name");
                return View(model);
            }
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Roles"] = new SelectList(await _userRepository.GetAllRolesAsync(), "Id", "Name");
            return View(new RegisterViewModel());
        }
    }
}
