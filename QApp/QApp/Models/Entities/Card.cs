using System;
using System.Collections.Generic;

namespace QApp.Models.Entities
{
    public partial class Card
    {
        public Card()
        {
            Teller = new HashSet<Teller>();
        }

        public int Id { get; set; }
        public int CardNumber { get; set; }
        public int? Qid { get; set; }
        public int? Tid { get; set; }
        public DateTime CardCreateTime { get; set; }
        public DateTime TellerStartTime { get; set; }
        public DateTime TellerEndTime { get; set; }

        public virtual ICollection<Teller> Teller { get; set; }
        public virtual Queue Q { get; set; }
        public virtual Teller T { get; set; }
    }
}
