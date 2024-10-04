using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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


        #region Users
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
                    Role = await _userRepository.GetUserRoleAsync(user),
                    Image = user.Image
                });
            }

            return View(model);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userRepository.GetUserByIdAsync(id.Value);

            if (user == null)
            {
                return NotFound();
            }

            var userDetails = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                CompleteName = user.CompleteName,
                Role = await _userRepository.GetUserRoleAsync(user),
                Image = user.Image
            };

            return View(userDetails);
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
        public async Task<IActionResult> Create(RegisterViewModel model, IFormFile? file, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
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
                AddErrors(result);
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
                return NotFound();
            }

            var role = await _userRepository.GetUserRoleAsync(userDB);

            var model = new RegisterViewModel
            {
                Id = userDB.Id,
                UserName = userDB.UserName ?? String.Empty,
                CompleteName = userDB.CompleteName,
                Image = userDB.Image,
                SelectedRole = role
            };

            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Roles"] = new SelectList(await _userRepository.GetAllRolesAsync(), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RegisterViewModel model, Guid id, IFormFile? file, string returnUrl = null)
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
                user.CompleteName = model.CompleteName;
                user.Email = model.UserName;

                if (file != null)
                {
                    user.Image = await _uploadService.UploadPhoto(file);
                }
                else
                {
                    user.Image = model.Image;
                }

                var result = await _userRepository.UpdateAsync(user);

                if (result.Succeeded)
                {
                    if (model.SelectedRole != null)
                    {
                        await _userRepository.RemoveUserRolesAsync(user);
                        await _userRepository.AddUserRoleAsync(user, model.SelectedRole);
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["Roles"] = new SelectList(await _userRepository.GetAllRolesAsync(), "Id", "Name");
            return View(model);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userRepository.GetUserByIdAsync(id.Value);

            if (user == null)
            {
                return NotFound();
            }

            var userDetails = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                CompleteName = user.CompleteName,
                Role = await _userRepository.GetUserRoleAsync(user),
                Image = user.Image
            };

            return View(userDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user != null)
            {
                var result = await _userRepository.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(Guid id)
        {
            var userDB = await _userRepository.GetUserByIdAsync(id);
            if (userDB == null)
            {
                return NotFound();
            }

            var model = new ChangePasswordViewModel
            {
                Id = userDB.Id,
            };

            ViewBag.Confirm = TempData["Confirm"];
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await _userRepository.GetUserByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(model.Password))
            {
                var removePasswordResult = await _userRepository.RemovePasswordAsync(user);
                if (!removePasswordResult.Succeeded)
                {
                    AddErrors(removePasswordResult);
                    return View(model);
                }

                var addPasswordResult = await _userRepository.AddPasswordAsync(user, model.Password);
                if (!addPasswordResult.Succeeded)
                {
                    AddErrors(addPasswordResult);
                    return View(model);
                }
            }

            var updateResult = await _userRepository.UpdateAsync(user);
            if (updateResult.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            AddErrors(updateResult);

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Login

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _userRepository.LoginAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userRepository.LogoutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #endregion

        #region Helpers
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
        #endregion
    }
}
