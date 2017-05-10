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

        public void CreateQueue()
        {
            Queue queue = new Queue();
            queue.Name = DateTime.Now.ToString();
            Queue.Add(queue);
            //SaveChanges();

            var counterToUpdate = Counter.Find(1);
            counterToUpdate.QueueId = queue.Id;
            SaveChanges();

        }
    }
}
