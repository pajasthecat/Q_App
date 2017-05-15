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

            //Hämtar den teller som matchar med den som klickar på knappens id
            User user = User.Single(i => i.AspNetUserId == aspUserId);
            //Kollar vilken counter den tellern är på
            int counterId = Counter.Where(c => c.TellerId == user.Id).Select(ci => ci.Id).First();

            //Hittar kortet som ska "stängas"
            Card cardToClose = Card.OrderBy(ci => ci.Id).Where(c => c.CounterId == counterId).LastOrDefault();

            //Sätter serviceEnd på kortet som ska stängas
            if (cardToClose != null)
            {
                cardToClose.ServiceEnd = DateTime.Now;
                //cardToClose.SessionId += "-done";
            }

            Card.Find(nextCardID).CounterId = counterId;
            Card.Find(nextCardID).TellerId = user.Id;
            Card.Find(nextCardID).ServiceStart = DateTime.Now;
            Counter.Find(counterId).CardId = nextCardID;
            //cardToClose.SessionId += "-done";
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

            //Ska kolla om personen (med samma session) redan står i kö.
            //bool alreadyInQueue = Card.Where(c => c.SessionId == sessionId).Count()> 0;

            //Card card = Card.Single(c => c.SessionId == sessionId);

            //Kollar att det finns en kö och att personen inte redan har ett card
            if (queueIsActive /*&& !alreadyInQueue*/)
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
                //Skicka felmeddelande ---- behövs ej?? Kunden har ju redan ett könummer
            }
        }

        public void RemoveTellerFromQueue(string aspUserId)
        {
            User user = User.SingleOrDefault(i => i.AspNetUserId == aspUserId);

            //Sätter queueid till null på den counter som trycker på knappen
            Counter counter = Counter.SingleOrDefault(t => t.TellerId == user.Id);

            //Om sista kassan stänger hittar vi alla cards i kön som inte fått hjälp och nullar deras session-id
            bool isLastCounter = Counter.Where(c => c.TellerId != null).Count() == 1;

            if (isLastCounter)
            {
                var lastCardsInQueue = Card.Where(c => c.CounterId == null).ToList();

                foreach (var item in lastCardsInQueue)
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
            //VI kollar om kassa 1 är ledig. Måste kolla om NÅGON kassa är öppen och om personen redan står i en kassa
            //När personal2 loggar in ska den inte starta en ny kö och heller inte ställa si 

            // Hämtar alla kassors qId till en lista 
            List<Counter> counters = Counter.Select(c => new Counter
            {
                QueueId = c.QueueId
            }).ToList();

            int activeCounters = 0;

            //Borde gå att snygga till
            //För varje kassa som har ett queueId plussar vi på activeCounters
            foreach (var counter in counters)
            {
                if (counter.QueueId != null)
                {
                    activeCounters++;
                }
            }

            User user = User.SingleOrDefault(i => i.AspNetUserId == aspUserId);

            //Om inga kassor är aktiva skapar vi ny kö och bemannar den
            if (activeCounters == 0)
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
                //Kollar om första kassans tellerid stämmer med det userid som öppnar kassan
                //bool tellerAlreadyActive = Counter.FirstOrDefault().TellerId == user.Id;
                Counter counter = Counter.FirstOrDefault(tid => tid.TellerId == user.Id);

                if (counter == null)
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

        //Antal nummer före i kön måste tas genom att kolla hur många cards med tidigare
        //nummer än en själv det finns som saknar counterid.
        //Alternativt loopa över counters och se vilket senaste numret är och jämföra det med mitt egna,
        public CustomerIndexVM GetPositionInQueue(string sessionId)
        {
            int cardsBeforeYou = 0;
            //bool haveIBeenHelped = false;

            Card card = null;

            card = Card.Single(c => c.SessionId == sessionId/* || c.SessionId == sessionId + "-done"*/);
            cardsBeforeYou = Card.Where(cn => cn.Id < card.Id && cn.CounterId == null).Count();

            //if (card.CounterId != null)
            //{
            //    haveIBeenHelped = true;
            //}

            CustomerIndexVM viewModel = new CustomerIndexVM();
            viewModel.NumbersLeftInQueue = cardsBeforeYou;
            //viewModel.HaveIBeenHelped = haveIBeenHelped;
            return viewModel;
        }

        //Metod för att lämna kö
        //public CustomerIndexVM LeaveCustomerQueue(string sessionId)
        //{
        //    Card card = Card.Where(sid => sid.SessionId == sessionId).First();
        //    card.CounterId = 999999;

        //    SaveChanges();

        //    CustomerIndexVM viewModel = new CustomerIndexVM();
        //    viewModel.CardNumber = 0;
        //    return viewModel;
        //}
    }
}
