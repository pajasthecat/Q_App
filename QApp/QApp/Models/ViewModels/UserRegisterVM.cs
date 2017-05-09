﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QApp.Models.ViewModels
{
    public class UserRegisterVM
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Lösenorden matchar inte")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string Street { get; set; }
        //public int ZipCode { get; set; }
        //public string City { get; set; }
        //public string Country { get; set; }
    }
}
