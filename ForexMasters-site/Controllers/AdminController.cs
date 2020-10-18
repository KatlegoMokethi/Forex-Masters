using ForexMasters_site.Models.Data;
using ForexMasters_site.Models.Entities;
using ForexMasters_site.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        //Category
        private void PopulateCategoryDDL(object selectedCategory = null)
        {
            ViewBag.CategoryID = new SelectList(_repositoryWrapper.Category.FindAll(),
                "CategoryID", "CategoryName", selectedCategory);
        }
        public IActionResult CreateCategory()
        {
            PopulateCategoryDDL();
            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                //Save new category to the database
                _repositoryWrapper.Category.Create(category);
                _repositoryWrapper.Category.Save();

                //Done
                return Redirect("/Masterclass/Index");
            }

            ModelState.AddModelError("", "Error: Category could not be created!");
            return View();
        }
        //:Category

        //Topic
        public IActionResult CreateTopic()
        {
            PopulateCategoryDDL();
            return View();
        }
        [HttpPost]
        public IActionResult CreateTopic(TopicViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Record topic details
                Topic topic = new Topic()
                {
                    TopicID = $"{Guid.NewGuid().ToString()}_{DateTime.Now.Ticks}",
                    Name = model.TopicName,
                    Description = model.TopicDescription,
                    CategoryID = model.CategoryID,
                    Password = model.ConfirmPassword
                };

                //Topic Vidos:
                if (model.Videos != null & model.Videos.Count() > 0)
                {
                    foreach (IFormFile video in model.Videos)
                    {
                        //Set video name
                        string VideoName = $"{video.FileName}_{Guid.NewGuid().ToString()}";

                        //Set video URL
                        string VideoURL = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\videos", VideoName);

                        //Copy file to server/disk
                        video.CopyTo(new FileStream(VideoURL, FileMode.Create));

                        //Record video details
                        Video videomodel = new Video()
                        {
                            Name = VideoName,
                            FileURL = "/videos/" + VideoName, //Get video URL
                            TopicID = topic.TopicID
                        };

                        //Save new video to the database
                        _repositoryWrapper.Video.Create(videomodel);
                        _repositoryWrapper.Video.Save();

                        //Add video to the topic
                        topic.Videos.Add(videomodel);
                    }
                }

                //Topic Documents:
                if (model.Documents != null & model.Documents.Count() > 0)
                {
                    foreach (IFormFile document in model.Documents)
                    {
                        //Set document name
                        string DocumentName = $"{document.FileName}_{Guid.NewGuid().ToString()}";

                        //Set document URL
                        string DocumentURL = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\documents", DocumentName);

                        //Copy file to server/disk
                        document.CopyTo(new FileStream(DocumentURL, FileMode.Create));

                        //Record document details
                        Document documentmodel = new Document()
                        {
                            Name = DocumentName,
                            FileURL = "/documents/" + DocumentName, //Get document URL
                            TopicID = topic.TopicID
                        };

                        //Save new document to the database
                        _repositoryWrapper.Document.Create(documentmodel);
                        _repositoryWrapper.Document.Save();

                        //Add document to the topic
                        topic.Documents.Add(documentmodel);
                    }
                }
                //Save new category to the database
                _repositoryWrapper.Topic.Create(topic);
                _repositoryWrapper.Topic.Save();

                //Done
                return Redirect("/Masterclass/Index");
            }

            ModelState.AddModelError("", "Error: Topic could not be created!");
            return View();
        }
        //:Topic

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
