using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QApp.Models.ViewModels;
using QApp.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QApp.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        UserManager<IdentityUser> userManager;
        SignInManager<IdentityUser> signInManager;
        RoleManager<IdentityRole> roleManager;
        //Behövs när vi genrerar tabellerna
        IdentityDbContext identityContext;
        MilljasContext context;

        public AdminController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager,
            IdentityDbContext identityContext, MilljasContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            //Behövs när vi genrerar tabellerna
            this.identityContext = identityContext;
            this.context = context;
        }

        //Flytta till Admincontroller för att regga nya tellers
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Edit()
        {
            #region Skapa Admin
            // Skapar tabeller för användarhantering med Identity
            //var result = identityContext.Database.EnsureCreated();

            //Användes när vi skapade Admin - rollen

            //var admin1 = await userManager.CreateAsync(new IdentityUser("4"), "Admin123!");

            //Skapa en Admin - användare av en befintlig användare. Kommer behöva dessa när det är dags att lägga till användare
            //var user1 = await userManager.FindByNameAsync("4");

            //var temp1 = await userManager.AddToRoleAsync(user1, "Admin");
            #endregion



            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminEditVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var result = await userManager.CreateAsync(new IdentityUser(viewModel.UserName), viewModel.Password);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("Username", result.Errors.First().Description);
                return View(viewModel);
            }

            
            context.AddTeller(viewModel);

            return RedirectToAction(nameof(Home));
        }

        public IActionResult Home()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Home(AdminHomeVM viewModel)
        {
            //context.PopulateQueue();
            return View();
        }
    }
}
