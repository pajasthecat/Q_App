using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QApp.Models.ViewModels;
using QApp.Models.Entities;
using Microsoft.AspNetCore.Http;

namespace QApp.Controllers
{
    public class CustomerController : Controller
    {
        MilljasContext context;

        public CustomerController(MilljasContext context)
        {
            this.context = context;
        }
        //Detta gör
        [Route("")]
        public IActionResult Index()
        {
            HttpContext.Session.SetString("Kalle", "Anka");
            return View(context.GetCardNumber(HttpContext.Session.Id));
        }
        
        public CustomerIndexVM GetCustomerCardNumber() 
        {
            HttpContext.Session.SetString("Kalle", "Anka");
            context.AddCustomerToQueue(HttpContext.Session.Id);
            return context.GetCardNumber(HttpContext.Session.Id);
        }

        public CustomerIndexVM ShowPositionInQueue()
        {
            HttpContext.Session.SetString("Kalle", "Anka");
            return context.GetPositionInQueue(HttpContext.Session.Id);
             
        }

        public void LeaveCustomerQueue()
        {
            HttpContext.Session.SetString("Kalle", "Anka");
            context.LeaveCustomerQueue(HttpContext.Session.Id);
        }
    }
}
