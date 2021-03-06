﻿using ForexMasters_site.Models.Data;
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
        public IActionResult DeleteFlashcard(int id)
        {
            Flashcard card = _repositoryWrapper.Flashcard.GetById(id);
            string deletepath = $@"wwwroot\{card.PictureURL}";
            System.IO.File.Delete(deletepath);
            _repositoryWrapper.Flashcard.Delete(card);
            _repositoryWrapper.Flashcard.Save();
            return Redirect("/Masterclass/ViewFlashcards");
        }
        //:Flashcard

        //Category
        private void PopulateCategoryDDL(object selectedCategory = null)
        {
            ViewBag.CategoryID = new SelectList(_repositoryWrapper.Category.FindAll(),
                "CategoryID", "CategoryName", selectedCategory);
            ViewBag.Categories = _repositoryWrapper.Category.FindAll().Count();
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

        public IActionResult DeleteCourse()
        {
            PopulateCategoryDDL();
            return View();
        }
        [HttpPost]
        public IActionResult DeleteCourse(Category category)
        {
            //Get Topics associated with the category
            List<Topic> Topics = _repositoryWrapper.Topic.FindByCondition(t => t.CategoryID == category.CategoryID).ToList();

            if (Topics.Count > 0)
            {
                string deletepath = string.Empty;
                //Delete each topic with its contents
                foreach (Topic topic in Topics)
                {
                    //Videos
                    List<Video> Videos = _repositoryWrapper.Video.FindByCondition(v => v.TopicID == topic.TopicID).ToList();
                    foreach (Video video in Videos)
                    {
                        deletepath = $@"wwwroot\{video.FileURL}";
                        System.IO.File.Delete(deletepath);
                        _repositoryWrapper.Video.Delete(video);
                    }
                    _repositoryWrapper.Video.Save();

                    //Documents
                    List<Document> Documents = _repositoryWrapper.Document.FindByCondition(d => d.TopicID == topic.TopicID).ToList();
                    foreach (Document document in Documents)
                    {
                        deletepath = $@"wwwroot\{document.FileURL}";
                        System.IO.File.Delete(deletepath);
                        _repositoryWrapper.Document.Delete(document);
                    }
                    _repositoryWrapper.Document.Save();

                    _repositoryWrapper.Topic.Delete(topic);
                    _repositoryWrapper.Topic.Save();
                }
            }
            _repositoryWrapper.Category.Delete(category);
            _repositoryWrapper.Category.Save();

            return Redirect("/Masterclass/Index");
        }
        //Topic
        public IActionResult CreateTopic()
        {
            PopulateCategoryDDL();
            return View();
        }
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
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

                //Save new topic to the database
                _repositoryWrapper.Topic.Create(topic);
                _repositoryWrapper.Topic.Save();

                //Topic Vidos:
                if (model.Videos != null & model.Videos.Count() > 0)
                {
                    foreach (IFormFile video in model.Videos)
                    {
                        //Set video name
                        string VideoName = $"{video.FileName}";

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
                        //topic.Videos.Add(videomodel);
                    }
                }

                //Topic Documents:
                if (model.Documents != null & model.Documents.Count() > 0)
                {
                    foreach (IFormFile document in model.Documents)
                    {
                        //Set document name
                        string DocumentName = $"{document.FileName}";

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
                        //topic.Documents.Add(documentmodel);
                    }
                }

                //Save topic changes to the database
                _repositoryWrapper.Topic.Update(topic);
                _repositoryWrapper.Topic.Save();

                //Done
                return Redirect("/Masterclass/Index");
            }

            ModelState.AddModelError("", "Error: Topic could not be created!");
            PopulateCategoryDDL();
            return View();
        }
        [HttpPost]
        public IActionResult DeleteTopic(string id)
        {
            Topic topic = _repositoryWrapper.Topic.FindByCondition(t => t.TopicID == id).FirstOrDefault();
            List<Video> Videos = _repositoryWrapper.Video.FindByCondition(v => v.TopicID == id).ToList();
            List<Document> Documents = _repositoryWrapper.Document.FindByCondition(d => d.TopicID == id).ToList();

            string deletepath = string.Empty;
            foreach (Video video in Videos)
            {
                deletepath = $@"wwwroot\{video.FileURL}";
                System.IO.File.Delete(deletepath);
                _repositoryWrapper.Video.Delete(video);
            }
            _repositoryWrapper.Video.Save();

            foreach (Document document in Documents)
            {
                deletepath = $@"wwwroot\{document.FileURL}";
                System.IO.File.Delete(deletepath);
                _repositoryWrapper.Document.Delete(document);
            }
            _repositoryWrapper.Document.Save();

            _repositoryWrapper.Topic.Delete(topic);
            _repositoryWrapper.Topic.Save();

            return Redirect("/Masterclass/Courses");
        }
        //:Topic

        //PDF
        public IActionResult CreatePDF()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreatePDF(PDFViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Topic Documents:
                if (model.Documents != null & model.Documents.Count() > 0)
                {
                    foreach (IFormFile document in model.Documents)
                    {
                        //Set document name
                        string DocumentName = $"{document.FileName}";

                        //Set document URL
                        string DocumentURL = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\documents", DocumentName);

                        //Copy file to server/disk
                        document.CopyTo(new FileStream(DocumentURL, FileMode.Create));

                        //Record document details
                        Document documentmodel = new Document()
                        {
                            Name = DocumentName,
                            FileURL = "/documents/" + DocumentName, //Get document URL
                        };

                        //Save new document to the database
                        _repositoryWrapper.Document.Create(documentmodel);
                        _repositoryWrapper.Document.Save();
                    }
                }

                //Done
                return Redirect("/Masterclass/PDFs");
            }

            ModelState.AddModelError("", "Error: Document(s) could not be added!");
            return View();
        }
        [HttpPost]
        public IActionResult DeletePDF(int id)
        {
            Document doc = _repositoryWrapper.Document.FindByCondition(d => d.DocumentID == id).FirstOrDefault();

            string deletepath = $@"wwwroot\{doc.FileURL}";
            System.IO.File.Delete(deletepath);
            _repositoryWrapper.Document.Delete(doc);
            _repositoryWrapper.Document.Save();

            return Redirect("/Masterclass/PDFs");
        }
        //:PDF

        //:CMS
        public ViewResult Index()
        {
            return View(_userManager.Users);
        }
        public ViewResult TopicsPasswords()
        {
            var topics = _repositoryWrapper.Topic.FindAll();
            return View(topics);
        }
        public IActionResult EditTopicPassword(string id)
        {
            Topic topic = _repositoryWrapper.Topic.FindByCondition(t => t.TopicID == id).FirstOrDefault();
            return View(topic);
        }

        [HttpPost]
        public IActionResult EditTopicPassword(string id, string name, string password)
        {
            Topic topic = _repositoryWrapper.Topic.FindByCondition(t => t.TopicID == id).FirstOrDefault();

            if (password != null)
                topic.Password = password;

            if(name != null)
                topic.Name = name;

            _repositoryWrapper.Topic.Update(topic);
            _repositoryWrapper.Topic.Save();

            return RedirectToAction("TopicsPasswords");
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
        public async Task<IActionResult> Delete(string email)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    User updateUser = _repositoryWrapper.User.FindByCondition(u => u.Email == email).FirstOrDefault();
                    _repositoryWrapper.User.Delete(updateUser);
                    _repositoryWrapper.User.Save();
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
        //--Edit User--
        public IActionResult EditUser(string email)
        {
            User user = _repositoryWrapper.User.FindByCondition(u => u.Email == email).FirstOrDefault();
            UsersViewModel model = new UsersViewModel()
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                IsActive = user.isActive,
                Balance = user.Balance,
                PhoneNumber = user.PhoneNumber
            };
            if (user != null)
            {
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UsersViewModel model, string email, string password, IFormFile file)
        {
            IdentityUser user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
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

                IdentityResult result = new IdentityResult();

                if (password != string.Empty)
                    result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    //Update user details...
                    User updateUser = _repositoryWrapper.User.FindByCondition(u => u.Email == email).FirstOrDefault();
                    updateUser.Balance = model.Balance;
                    updateUser.isActive = model.IsActive;

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
                        updateUser.PictureURL = "/images/profiles/" + PictureName; //Get picture URL
                    }

                    _repositoryWrapper.User.Update(updateUser);
                    _repositoryWrapper.User.Save();
                }

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(user);
        }
        //--:Edit User

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
