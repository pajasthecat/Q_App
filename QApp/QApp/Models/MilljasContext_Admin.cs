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
        

        public async Task AddTeller(AdminCreateVM viewModel)
        {

         var  aspNetUser = await userManager.FindByNameAsync(viewModel.UserName);
         await userManager.AddToRoleAsync(aspNetUser, "Teller");

            User user = new User
            {
                AspNetUserId = aspNetUser.Id,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName
            };

            User.Add(user);
           await SaveChangesAsync();

        }

        public async Task<List<AdminHomeVM>> ShowTellers()
        {

         var temp =  User.Select(s => new AdminHomeVM
         {
             UserName = "",
             FirstName = s.FirstName,
             LastName = s.LastName,
             AspNetUserId = s.AspNetUserId


         }).ToList();

            foreach (var item in temp)
            {
                var user = await userManager.FindByIdAsync(item.AspNetUserId);
                item.UserName = user.UserName;
               
            }
            return temp;
        }

        public AdminUpdateVM ShowTellerToUpdate(AdminHomeVM viewModel)
        {
            AdminUpdateVM tellerToUpdate = new AdminUpdateVM
            {
                UserName = viewModel.UserName,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName
            };

            return tellerToUpdate;
        }

        //public async Task UpdateTeller(AdminEditVM viewModel)
        //{

        //}

    }
}
