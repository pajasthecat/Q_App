using System;
using System.Collections.Generic;

namespace QApp.Models.Entities
{
    public partial class Queue
    {
        public Queue()
        {
            QueueTeller = new HashSet<QueueTeller>();
            QueueUser = new HashSet<QueueUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<QueueTeller> QueueTeller { get; set; }
        public virtual ICollection<QueueUser> QueueUser { get; set; }
    }
}
