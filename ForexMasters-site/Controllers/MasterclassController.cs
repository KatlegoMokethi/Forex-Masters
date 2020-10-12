using ForexMasters_site.Models.Data;
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
    }
}
