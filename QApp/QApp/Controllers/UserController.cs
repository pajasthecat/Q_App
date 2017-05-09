using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using QApp.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QApp.Controllers
{
    public class UserController : Controller
    {
        UserManager<IdentityUser> userManager;
        SignInManager<IdentityUser> signInManager;
        //Behövs när vi genrerar tabellerna
        //IdentityDbContext identityContext;

        public UserController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager 
            /*IdentityDbContext identityContext*/)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            //Behövs när vi genrerar tabellerna
            //this.identityContext = identityContext;

        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            // Skapar tabeller för användarhantering med Identity
           //var result= identityContext.Database.EnsureCreated(); 

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegisterVM register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            var result = await userManager.CreateAsync(new IdentityUser(register.UserName), register.Password);

            return RedirectToAction(nameof(Index));
        }


    }
}
