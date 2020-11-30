using ForexMasters_site.Models.Data;
using ForexMasters_site.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForexMasters_site.Controllers
{
    [Authorize]
    public class MasterclassController : Controller
    {
        private IRepositoryWrapper _repository;
        private UserManager<IdentityUser> _userManager;
        public MasterclassController(IRepositoryWrapper repository, UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            User ActiveUser = _repository.User.FindAll()
                                              .Where(u => u.Email == user.Email)
                                              .FirstOrDefault();
            return View(ActiveUser);
        }

        public IActionResult ViewFlashcards()
        {
            var Flashcards = _repository.Flashcard.FindAll();
            return View(Flashcards);
        }

        public IActionResult Courses()
        {
            var Categories = _repository.Category.FindAll();
            return View(Categories);
        }

        public IActionResult Topics(int id)
        {
            var topics = _repository.Topic.FindAll().Where(t => t.CategoryID == id).OrderBy(t => t.Name);
            if (User.IsInRole("Admin"))
                return View(topics);
            else
            {
                bool approved = _repository.User.FindByCondition(u => u.Name == User.Identity.Name)
                                            .FirstOrDefault()
                                            .isActive;

                if (approved)
                    return View(topics);
                else
                    return Redirect("/Masterclass/AccessDenied");
            }
        }
        public IActionResult PDFs()
        {
            var documents = _repository.Document.FindAll().Where(d => d.TopicID == null).OrderBy(d => d.Name);
            if (User.IsInRole("Admin"))
                return View(documents);
            else
            {
                bool approved = _repository.User.FindByCondition(u => u.Name == User.Identity.Name)
                                            .FirstOrDefault()
                                            .isActive;

                if (approved)
                    return View(documents);
                else
                    return Redirect("/Masterclass/AccessDenied");
            }
        }
        public IActionResult EnterPassword(string id)
        {
            Topic topic = _repository.Topic.FindByCondition(t => t.TopicID == id).FirstOrDefault();
            return View(topic);
        }
        [HttpPost]
        public IActionResult EnterPassword(string id, string password)
        {
            Topic topic = _repository.Topic.FindByCondition(t => t.TopicID == id).FirstOrDefault();
            
            if (password.CompareTo(topic.Password) == 0)
                return RedirectToAction("TopicDetail", topic);
            else
            {
                ViewBag.Message = "failed";
                return View(topic);
            }


        }
        public IActionResult TopicDetail(Topic topic)
        {
            ViewBag.Videos = _repository.Video.FindByCondition(v => v.TopicID == topic.TopicID);
            ViewBag.Documents = _repository.Document.FindByCondition(d => d.TopicID == topic.TopicID);
            return View(topic);
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
