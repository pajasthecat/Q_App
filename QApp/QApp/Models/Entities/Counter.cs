using System;
using System.Collections.Generic;

namespace QApp.Models.Entities
{
    public partial class Counter
    {
        public int Id { get; set; }
        public string CounterName { get; set; }
        public int? QueueId { get; set; }
        public int? CardId { get; set; }

        public virtual Card Card { get; set; }
        public virtual Queue Queue { get; set; }
    }
}
