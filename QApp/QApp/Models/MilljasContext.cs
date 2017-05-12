using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QApp.Models.Entities;
using QApp.Models.ViewModels;
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

        public TellerQueueVM HelpNextCustomer(string aspUserId)
        {
            TellerQueueVM tellerQueueVM = new TellerQueueVM();

            //Card card = Card.Where(c => c.Id == nextCard).Single();

            //Hitta nästa card som inte är kopplad till en counter
            Card card = Card.OrderBy(c => c.Id).Where(i => i.CounterId == null).First();
            // Ta ut cardets id och nummer och tilldela variablerna
            int nextCardID = card.Id;
            int nextCardNumber = card.CardNumber;
            // ??
            User user = User.Single(i => i.AspNetUserId == aspUserId);
            int counterId = Counter.Where(c => c.TellerId == user.Id).Select(ci => ci.Id).First();

            //
            Card.Find(nextCardID).CounterId = counterId;
            Card.Find(nextCardID).TellerId = user.Id;
            Counter.Find(counterId).CardId = nextCardID;
            SaveChanges();

            tellerQueueVM.CardNumber = nextCardNumber;

            return tellerQueueVM;
        }

        public CustomerIndexVM GetCardNumber(string sessionId)
        {
            CustomerIndexVM customerIndexVM = new CustomerIndexVM();
            try
            {
                int cardNumber = Card.Where(s => s.SessionId == sessionId).Select(si => si.CardNumber).Single();
                customerIndexVM.CardNumber = cardNumber;
            }
            catch (Exception)
            {

            }

            return customerIndexVM;
        }

        public void AddCustomerToQueue(string sessionId)
        {
            int activeQueue = Queue.OrderBy(q => q.Id).Select(p => p.Id).LastOrDefault();

            bool queueIsActive = Counter.FirstOrDefault().QueueId != null;

            if (queueIsActive)
            {
                int numberOfCards = Card.Where(c => c.QueueId == activeQueue).Count();
                Card card = new Card();

                card.CardNumber = numberOfCards + 1;
                card.CardCreated = DateTime.Now;
                card.QueueId = activeQueue;
                card.SessionId = sessionId;
                Card.Add(card);
                SaveChanges();

            }
            else
            {
                //Skicka felmeddelande
            }
        }

        public void RemoveTellerFromQueue(string aspUserId)
        {
            User user = User.SingleOrDefault(i => i.AspNetUserId == aspUserId);

            //Sätter queueid till null på den counter som trycker på knappen
            Counter counter = Counter.SingleOrDefault(t => t.TellerId == user.Id);

            //När vi stänger sista kassan 
            //Selecta alla cards som har session id och saknar teller id
            //Skapa lista av cards och loopa igenom
            bool isLastCounter = Counter.Where(c => c.TellerId != null).Count() == 1;
            if (isLastCounter)
            {
                var test = Card.Where(c => c.CounterId == null).ToList();

                foreach (var item in test)
                {
                    item.SessionId = null;
                    
                }

            }

            counter.QueueId = null;
            counter.TellerId = null;
            counter.CardId = null;


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
