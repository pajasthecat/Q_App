using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using QApp.Models.ViewModels;
using QApp.Models.Entities;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QApp.Controllers
{   
    [Authorize]
    public class TellerController : Controller
    {
        UserManager<IdentityUser> userManager;
        SignInManager<IdentityUser> signInManager;
        RoleManager<IdentityRole> roleManager;
        //Behövs när vi genrerar tabellerna
        IdentityDbContext identityContext;
        MilljasContext context;

        public TellerController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager,
            IdentityDbContext identityContext, MilljasContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            //Behövs när vi genrerar tabellerna
            this.identityContext = identityContext;
            this.context = context;
        }

        public IActionResult Queue()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Queue(TellerQueueVM viewModel)
        {
            string aspUserId = userManager.GetUserId(HttpContext.User);
            context.RemoveTellerFromQueue(aspUserId);
            return RedirectToAction(nameof(Home));
        }

        public TellerQueueVM HelpNextCustomer()
        {
            TellerQueueVM tellerQueueVM = new TellerQueueVM();

            string aspUserId = userManager.GetUserId(HttpContext.User);
            
            return context.HelpNextCustomer(aspUserId);
        }

        // GET: /<controller>/
        public IActionResult Home()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Home(AdminHomeVM viewModel)
        {
            string aspUserId = userManager.GetUserId(HttpContext.User); 
            context.PopulateQueue(aspUserId);
            return RedirectToAction(nameof(Queue));
        }

        //Flytta till Admincontroller för att regga nya tellers
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> Register()
        //{
        //    //Skapar tabeller för användarhantering med Identity
        //    //var result = identityContext.Database.EnsureCreated();

        //    //Användes när vi skapade Admin - rollen
        //    //var teller = new IdentityRole("Teller");
        //    //var temp = await roleManager.CreateAsync(teller);

        //    //var teller1 = await userManager.CreateAsync(new IdentityUser("1"), "Teller123!");
        //    //var teller2 = await userManager.CreateAsync(new IdentityUser("2"), "Teller123!");
        //    //var teller3 = await userManager.CreateAsync(new IdentityUser("3"), "Teller123!");


        //    ////Skapa en Admin - användare av en befintlig användare. Kommer behöva dessa när det är dags att lägga till användare
        //    //var user1 = await userManager.FindByNameAsync("1");
        //    //var user2 = await userManager.FindByNameAsync("2");
        //    //var user3 = await userManager.FindByNameAsync("3");

        //    //var temp1 = await userManager.AddToRoleAsync(user1, "Teller");
        //    //var temp2 = await userManager.AddToRoleAsync(user2, "Teller");
        //    //var temp3 = await userManager.AddToRoleAsync(user3, "Teller");


        //    //return View();
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> Register(UserRegisterVM viewModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(viewModel);
        //    }

        //    var result = await userManager.CreateAsync(new IdentityUser(viewModel.UserName), viewModel.Password);

        //    if (!result.Succeeded)
        //    {
        //        ModelState.AddModelError("Username", result.Errors.First().Description);
        //        return View(viewModel);
        //    }

        //    return RedirectToAction(nameof(Home));
        //}

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(TellerLogInVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var result = await signInManager.PasswordSignInAsync(viewModel.UserName, viewModel.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("Username", "Ogiltigt användarnamn eller lösenord");
                return View(viewModel);
            }

            return RedirectToAction(nameof(Home));
        }
    }
}
