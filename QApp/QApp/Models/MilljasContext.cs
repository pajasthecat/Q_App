using Microsoft.EntityFrameworkCore;
using QApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QApp.Models.Entities
{
    public partial class MilljasContext : DbContext
    {
        public MilljasContext(DbContextOptions<MilljasContext> options) : base(options)
        {

        }


        public void RemoveTellerFromQueue(string aspUserId)
        {
            User user = User.SingleOrDefault(i => i.AspNetUserId == aspUserId);

            //Sätter queueid till null på den counter som trycker på knappen
            Counter counter = Counter.SingleOrDefault(t => t.TellerId == user.Id);

            counter.QueueId = null;
            counter.TellerId = null;

            SaveChanges();
        }

        //Vi stoppar in aspnetUserId här
        public void PopulateQueue(string aspUserId)
        {

            bool isFirstCounterFree = Counter.Find(1).QueueId == null;
            User user = User.SingleOrDefault(i => i.AspNetUserId == aspUserId);

            if (isFirstCounterFree)
            {
                Queue queue = new Queue();
                queue.Name = DateTime.Now.ToString();
                Queue.Add(queue);

                Counter.Find(1).TellerId = user.Id;
                //Tar kassa 1 och sätter queueid till den nya köns id
                Counter.Find(1).QueueId = queue.Id;
                SaveChanges();
            }
            else
            {
                bool tellerAlreadyActive = Counter.FirstOrDefault().TellerId == user.Id;

                if (!tellerAlreadyActive)
                {
                    // Sorterar kötabell på id och tar den senast skapade kön
                    int activeQueue = Queue.OrderBy(q => q.Id).Select(p => p.Id).LastOrDefault();

                    // Tar nästa lediga kassa och sätter teller id till user id samt queueid till den aktiva köns id
                    Counter.Where(q => q.QueueId == null).First().QueueId = activeQueue;
                    Counter.Where(q => q.TellerId == null).First().TellerId = user.Id;
                    SaveChanges();
                }
                else
                {
                    //TODO Skicka tillbaka att tellern redan bemannar en kassa
                }
                
            }


        }
    }
}
