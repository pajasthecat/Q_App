using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QApp.Models.ViewModels;
using QApp.Models.Entities;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QApp.Controllers
{
    public class CustomerController : Controller
    {
        MilljasContext context;

        public CustomerController(MilljasContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            
            return View(context.GetCardNumber(HttpContext.Session.Id));
        }

        
        public CustomerIndexVM GetCustomerCardNumber() 
        {

            context.AddCustomerToQueue(HttpContext.Session.Id);
            //HttpContext.Session.SetString("CardNumber", viewModel.CardNumber.ToString());
            return context.GetCardNumber(HttpContext.Session.Id);
        }
    }
}
