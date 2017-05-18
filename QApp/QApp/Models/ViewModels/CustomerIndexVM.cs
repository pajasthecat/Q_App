using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QApp.Models.ViewModels
{
    public class CustomerIndexVM
    {
        public int CardNumber { get; set; }
        //public int NumbersLeftInQueue { get; set; }
        public bool MyTurn { get; set; }
        public string Message { get; set; }
        public bool OpenQueue { get; set; }


    }
}
