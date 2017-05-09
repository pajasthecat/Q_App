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
    [Authorize]
    public class UserController : Controller
    {
        UserManager<IdentityUser> userManager;
        SignInManager<IdentityUser> signInManager;
        RoleManager<IdentityRole> roleManager;
        //Behövs när vi genrerar tabellerna
        //IdentityDbContext identityContext;

        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager
            /*IdentityDbContext identityContext*/)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            //Behövs när vi genrerar tabellerna
            //this.identityContext = identityContext;

        }

        // GET: /<controller>/
        public IActionResult Home()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register()
        {
            // Skapar tabeller för användarhantering med Identity
            //var result= identityContext.Database.EnsureCreated(); 

            //Användes när vi skapade Admin-rollen
            //var Admin = new IdentityRole("Admin");
            // var temp =  await roleManager.CreateAsync(Admin);

            //var result = await userManager.CreateAsync(new IdentityUser("admin"), "Admin123//");

            //Skapa en Admin-användare av en befintlig användare. Kommer behöva dessa när det är dags att lägga till användare
            //var user = await userManager.FindByNameAsync("admin");
            //var temp =await userManager.AddToRoleAsync(user, "Admin");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegisterVM viewModel)
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

            return RedirectToAction(nameof(Home));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(UserLogInVM viewModel)
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
