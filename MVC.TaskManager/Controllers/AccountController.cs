using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC.TaskManager.Extensions;
using MVC.TaskManager.Models.Users;
using MVC.TaskManager.Repositories.Interface;
using MVC.TaskManager.Services.Interface;
using MVC.TaskManager.ViewModels.AccountViewModel;

namespace MVC.TaskManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IUploadService _uploadService;

        public AccountController(IUserRepository userRepository, IUploadService uploadService)
        {
            _userRepository = userRepository;
            _uploadService = uploadService;
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
                    Email = user.Email,
                    CompleteName = user.CompleteName,   
                    Role = await _userRepository.GetUserRoleAsync(user)
                });
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Roles"] = new SelectList(await _userRepository.GetAllRolesAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterViewModel model, IFormFile file, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var user = new User
            {
                UserName = model.UserName,
                CompleteName = model.CompleteName,  
                Email = model.UserName,
                Image = await _uploadService.UploadPhoto(file)
            };

            var result = await _userRepository.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {

                if (model.SelectedRole != null)
                {
                    await _userRepository.RemoveUserRolesAsync(user);
                    await _userRepository.AddUserRoleAsync(user, model.SelectedRole);
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["Roles"] = new SelectList(await _userRepository.GetAllRolesAsync(), "Id", "Name");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id, string returnUrl = null)
        {
                var userDB = await _userRepository.GetUserByIdAsync(id);
                if (userDB == null)
                {
                    return RedirectToAction("Index", "Account");
                }

                var role = await _userRepository.GetUserRoleAsync(userDB);

                var model = new RegisterViewModel
                {
                    Id = userDB.Id, 
                    UserName = userDB.UserName ?? String.Empty,
                    CompleteName = userDB.CompleteName,
                    SelectedRole = role
                };

                ViewData["ReturnUrl"] = returnUrl;
                ViewData["Roles"] = new SelectList(await _userRepository.GetAllRolesAsync(), "Id", "Name");
                return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RegisterViewModel model, Guid id, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");

            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                user.UserName = model.UserName;
                user.Email = model.UserName;

                var result = await _userRepository.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["Roles"] = new SelectList(await _userRepository.GetAllRolesAsync(), "Id", "Name");
            return View(model);
        }

    }
}
