using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QApp.Models.ViewModels;
using QApp.Models.Entities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QApp.Controllers
{
    public class AdminController : Controller
    {
        MilljasContext context;

        public AdminController(MilljasContext context)
        {
            this.context = context;
        }

        // GET: /<controller>/
        public IActionResult Home()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Home(AdminHomeVM viewModel)
        {
            context.CreateQueue();
            return View();
        }
    }
}
