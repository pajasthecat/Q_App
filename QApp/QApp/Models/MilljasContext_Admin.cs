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

        public void AddTeller(AdminEditVM viewModel)
        {
            

            User user = new User
            {

            };
             

        }
    }
}
