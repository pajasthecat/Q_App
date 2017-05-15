using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QApp.Models.ViewModels
{
    public class TellerLogInVM
    {
        [Required]
        public string Användarnamn { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Lösenord { get; set; }
    }
}
