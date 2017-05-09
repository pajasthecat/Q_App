using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QApp.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QApp.Controllers
{
    public class AdminController : Controller
    {
        // GET: /<controller>/
        public IActionResult Home()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Home(AdminHomeVM viewModel)
        {

            return View();
        }
    }
}
