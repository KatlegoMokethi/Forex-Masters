using ForexMasters_site.Models.Data;
using ForexMasters_site.Models.Entities;
using ForexMasters_site.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
                        return Redirect("/Masterclass/Index");
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
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                if (await _roleManager.FindByNameAsync(_role) == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(_role));
                }

                //Check if user already exists in the database...IEnumerable<User>
                //var Users = _repositoryWrapper.User.FindByCondition(u => u.Email == registerModel.Email);
                //if (Users != null)
                //{
                //    ModelState.AddModelError("", "Email Already Exits!");
                //    return View(registerModel);
                //}

                //Assign required user details
                var user = new IdentityUser
                {
                    UserName = registerModel.Name,
                    Email = registerModel.Email
                };

                //Create new user
                var result = await _userManager.CreateAsync(user, registerModel.Password);

                if (result.Succeeded)
                {
                    //Add default "Student" role to the user
                    await _userManager.AddToRoleAsync(user, _role);

                    //Record the user details to User model
                    User newUser = new User()
                    {
                        Name = registerModel.Name,
                        Surname = registerModel.Surname,
                        Country = registerModel.Country,
                        Email = registerModel.Email,
                        PictureFile = registerModel.PictureFile,
                        Password = registerModel.Password
                    };

                    //Save new user to the database
                    _repositoryWrapper.User.Create(newUser);
                    _repositoryWrapper.User.Save();

                    //Go Login
                    return RedirectToAction("Login", "Account");
    }
                else
                {
                    ModelState.AddModelError("", "Unable to register new user");
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
