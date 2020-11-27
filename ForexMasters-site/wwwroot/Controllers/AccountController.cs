using ForexMasters_site.Models.Data;
using ForexMasters_site.Models.Entities;
using ForexMasters_site.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ForexMasters_site.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private string _role = "Student";
        private IRepositoryWrapper _repositoryWrapper;
        public AccountController(UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IRepositoryWrapper repositoryWrapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _repositoryWrapper = repositoryWrapper;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginModel
            {
                ReturnUrl = returnUrl
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user =
                await _userManager.FindByEmailAsync(loginModel.Email);

                if (user == null)
                {
                    user =
                await _userManager.FindByNameAsync(loginModel.Email);
                }
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user,
                    loginModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        if (user.Email.Contains("admin"))
                            return RedirectToAction("Index", "Admin");

                        return RedirectToAction("Index", "Masterclass");
                    }
                }
            }
            ModelState.AddModelError("", "Invalid email or password");
            return View(loginModel);
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterModel registerModel, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                //Assign required user details
                var user = new IdentityUser
                {
                    UserName = registerModel.Name,
                    Email = registerModel.Email,
                    PhoneNumber = registerModel.PhoneNumber
                };

                //Create student role
                if (await _roleManager.FindByNameAsync(_role) == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(_role));
                }

                //Create new user
                var result = await _userManager.CreateAsync(user, registerModel.Password);

                if (result.Succeeded)
                {
                    //Add default "Student" role to the user
                    await _userManager.AddToRoleAsync(user, _role);

                    User newUser = new User();
                    //Process user image
                    if (file != null)
                    {
                        //Get picture name
                        var PictureName = $"{file.FileName}";

                        //Set picture URL
                        var PictureURL = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\profiles", PictureName);
                        using (var stream = new FileStream(PictureURL, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        //:Process user image

                        //Record the user picture url
                        newUser.PictureURL = "/images/profiles/" + PictureName; //Get picture URL
                    }

                    //Record the user details to User model
                    newUser.Name = registerModel.Name;
                    newUser.Surname = registerModel.Surname;
                    newUser.Country = registerModel.Country;
                    newUser.Email = registerModel.Email;
                    newUser.PhoneNumber = registerModel.PhoneNumber;

                    //Save new user to the database
                    _repositoryWrapper.User.Create(newUser);
                    _repositoryWrapper.User.Save();

                    //Go Login
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", $"Unable to register new user: {result.Errors.ToString()}");
                }
            }
            return View(registerModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
