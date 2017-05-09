using System;
using System.Collections.Generic;

namespace QApp.Models.Entities
{
    public partial class QueueUser
    {
        public int Id { get; set; }
        public int QueueNumber { get; set; }
        public int Qid { get; set; }

        public virtual Queue Q { get; set; }
    }
}
