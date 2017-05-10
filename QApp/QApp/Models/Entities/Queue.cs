using System;
using System.Collections.Generic;

namespace QApp.Models.Entities
{
    public partial class Queue
    {
        public Queue()
        {
            Card = new HashSet<Card>();
            Teller = new HashSet<Teller>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Card> Card { get; set; }
        public virtual ICollection<Teller> Teller { get; set; }
    }
}
