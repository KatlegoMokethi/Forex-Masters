using ForexMasters_site.Models.Data;
using ForexMasters_site.Models.Entities;
using ForexMasters_site.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace ForexMasters_site.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private IUserValidator<IdentityUser> _userValidator;
        private IPasswordValidator<IdentityUser> _passwordValidator;
        private IPasswordHasher<IdentityUser> _passwordHasher;
        private IRepositoryWrapper _repositoryWrapper;

        public AdminController(UserManager<IdentityUser> userManager,
            IUserValidator<IdentityUser> userValidator,
            IPasswordValidator<IdentityUser> passValidator,
            IPasswordHasher<IdentityUser> passwordHasher,
            IRepositoryWrapper repositoryWrapper)
        {
            _userManager = userManager;
            _userValidator = userValidator;
            _passwordValidator = passValidator;
            _passwordHasher = passwordHasher;
            _repositoryWrapper = repositoryWrapper;
        }
        //CMS
        //Flashcard
        public IActionResult CreateFlashcard()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateFlashcard(FlashcardViewModel flashcardModel, IFormFile file)
        {
            //Check if picture is selected
            if (file == null)
            {
                ModelState.AddModelError("", "Error: Picture is not selected! Flashcard could not be created!");
                return View();
            }

            if (ModelState.IsValid)
            {
                //Set picture URL
                var PictureURL = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\images\flashcards", file.FileName);

                //Copy file to server/disk
                using (var stream = new FileStream(PictureURL, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                //Record flashcard details
                Flashcard flashcard = new Flashcard()
                {
                    Name = flashcardModel.Name,
                    Date = flashcardModel.Date,
                    PictureURL = "/images/flashcards/" + file.FileName //Get picture URL
                };

                //Save new flashcard to the database
                _repositoryWrapper.Flashcard.Create(flashcard);
                _repositoryWrapper.Flashcard.Save();

                //Done
                return Redirect("/Masterclass/Index");
            }

            ModelState.AddModelError("", "Error: Flashcard could not be created!");
            return View();
        }
        //:Flashcard
        //:CMS
        public ViewResult Index()
        {
            return View(_userManager.Users);
        }
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = model.Name,
                    Email = model.Email
                };
                IdentityResult result
                    = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View("Index", _userManager.Users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string email, string password)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = email;
                IdentityResult validEmail = await _userValidator.ValidateAsync(_userManager, user);

                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }

                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await _passwordValidator.ValidateAsync(_userManager, user, password);

                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }

                if ((validEmail.Succeeded && validPass == null)
                    || (validEmail.Succeeded && password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        //Update user details...

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(user);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
