using System;
using System.Collections.Generic;

namespace QApp.Models.Entities
{
    public partial class QueueTeller
    {
        public int Id { get; set; }
        public int TellerNumber { get; set; }
        public int Qid { get; set; }

        public virtual Queue Q { get; set; }
    }
}
