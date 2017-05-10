using System;
using System.Collections.Generic;

namespace QApp.Models.Entities
{
    public partial class Teller
    {
        public Teller()
        {
            Card = new HashSet<Card>();
        }

        public int Id { get; set; }
        public string TellerNumber { get; set; }
        public int? Qid { get; set; }
        public int? Cid { get; set; }

        public virtual ICollection<Card> Card { get; set; }
        public virtual Card C { get; set; }
        public virtual Queue Q { get; set; }
    }
}
