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

        public void PopulateQueue()
        {
            
            bool isFirstCounterFree = Counter.Find(1).QueueId == null;

            if (isFirstCounterFree)
            {
                Queue queue = new Queue();
                queue.Name = DateTime.Now.ToString();
                Queue.Add(queue);

                //Tar kassa 1 och sätter queueid till den nya köns id
                Counter.Find(1).QueueId = queue.Id;
                SaveChanges();
            }
            else
            {
               // Sorterar kötabell på id och tar den senast skapade kön
                int activeQueue = Queue.OrderBy(q => q.Id).Select(p => p.Id).LastOrDefault();
                // Tar nästa lediga kassa och sätter queueid till den aktiva köns id
                Counter.Where(q => q.QueueId == null).First().QueueId = activeQueue;
                SaveChanges();
            }


        }
    }
}
