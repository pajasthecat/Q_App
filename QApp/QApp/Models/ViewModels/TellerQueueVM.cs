using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QApp.Models.ViewModels
{
    public class TellerQueueVM
    {
        public int CardNumber { get; set; }
        public bool isLastCounter { get; set; }
        public bool isLastCard { get; set; } //lägger till för att hantera sista kortet i javascriptet
        public int CustomersLeftInQueue { get; set; }
        public string Message { get; set; }
        public string MyCounter { get; set; }
        public string TellerName { get; set; }
    }
}
