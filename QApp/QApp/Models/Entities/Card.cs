using System;
using System.Collections.Generic;

namespace QApp.Models.Entities
{
    public partial class Card
    {
        public Card()
        {
            Counter = new HashSet<Counter>();
        }

        public int Id { get; set; }
        public int CardNumber { get; set; }
        public int QueueId { get; set; }
        public int? CounterId { get; set; }
        public DateTime CardCreated { get; set; }
        public DateTime? ServiceStart { get; set; }
        public DateTime? ServiceEnd { get; set; }
        public int? TellerId { get; set; }
        public string SessionId { get; set; }

        public virtual ICollection<Counter> Counter { get; set; }
        public virtual Queue Queue { get; set; }
        public virtual User Teller { get; set; }
    }
}
