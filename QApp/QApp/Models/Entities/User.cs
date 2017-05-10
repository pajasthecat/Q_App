using System;
using System.Collections.Generic;

namespace QApp.Models.Entities
{
    public partial class User
    {
        public User()
        {
            Card = new HashSet<Card>();
        }

        public int Id { get; set; }
        public string AspNetUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Card> Card { get; set; }
    }
}
