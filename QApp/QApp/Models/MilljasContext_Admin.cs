using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QApp.Models.Entities;
using QApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QApp.Models.Entities
{
    public partial class MilljasContext :DbContext
    {
        

        public async Task AddTeller(AdminEditVM viewModel)
        {

         var  aspNetUser = await userManager.FindByNameAsync(viewModel.UserName);
         var teller = await userManager.AddToRoleAsync(aspNetUser, "Teller");

            User user = new User
            {
                AspNetUserId = aspNetUser.Id,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName
            };

            User.Add(user);
           await SaveChangesAsync();

        }
    }
}
