using ForexMasters_site.Models.Data;
using ForexMasters_site.Models.Entities;
using Microsoft.AspNetCore.Authorization;
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
        public MasterclassController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
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
            var Topics = _repository.Topic.FindAll().Where(t => t.CategoryID == id);
            return View(Topics);
        }
        public IActionResult TopicDetail(string id)
        {
            Topic topic = _repository.Topic.FindByCondition(t => t.TopicID == id).FirstOrDefault();
            ViewBag.Videos = _repository.Video.FindByCondition(v => v.TopicID == id);
            ViewBag.Documents = _repository.Document.FindByCondition(d => d.TopicID == id);
            return View(topic);
        }
    }
}
