﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QApp.Models.ViewModels
{
    public class AdminHomeVM
    {
        
        public string UserName { get; set; }

       
        public string FirstName { get; set; }

        
        public string LastName { get; set; }

        public string AspNetUserId { get; set; }
    }
}
