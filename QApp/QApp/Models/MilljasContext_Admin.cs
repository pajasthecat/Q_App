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
    public partial class MilljasContext : DbContext
    {


        public async Task AddTeller(AdminCreateVM viewModel)
        {

            var aspNetUser = await userManager.FindByNameAsync(viewModel.UserName);
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

            var temp = User.Select(s => new AdminHomeVM
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

        public async Task<AdminUpdateVM> ShowTellerToUpdate(string aspNetUserId)
        {
            var aspNetUser = await userManager.FindByIdAsync(aspNetUserId);

            var user = User.Where(u => u.AspNetUserId == aspNetUserId).SingleOrDefault();

            AdminUpdateVM tellerToUpdate = new AdminUpdateVM
            {
                UserName = aspNetUser.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName

            };

            return tellerToUpdate;
        }

        public async Task UpdateTeller(AdminUpdateVM viewModel, string aspNetUserId)
        {

            //Måste ändra hitta lösning för att byta lösenord och username
            var aspNetUser = await userManager.FindByIdAsync(aspNetUserId);

            var user = User.Where(u => u.AspNetUserId == aspNetUserId).SingleOrDefault();

            if (viewModel.UserName != null && viewModel.UserName != aspNetUser.UserName)
            {

                aspNetUser.UserName = viewModel.UserName;

            }

            if (viewModel.Password != null)
            {
              
                await userManager.RemovePasswordAsync(aspNetUser);
                await userManager.AddPasswordAsync(aspNetUser, viewModel.Password);
                //await userManager.ChangePasswordAsync(aspNetUser, aspNetUser.PasswordHash, viewModel.Password);
                

            }

            if (viewModel.FirstName != null)
            {
                user.FirstName = viewModel.FirstName;
            }

            if (viewModel.LastName != null)
            {
                user.LastName = viewModel.LastName;
            }

            var y = await userManager.UpdateAsync(aspNetUser);

            User.Update(user);
            SaveChanges();

        }

    }
}
