using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QApp.Models.ViewModels
{
    public class AdminCreateVM
    {
        [Required]
        public string UserName { get; set; }

        [RegularExpression(@"^[A-Z][A-Za-z0-9!@#$%^&*]*$",
        ErrorMessage = "Ogiltligt lösenord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Lösenorden matchar inte")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
