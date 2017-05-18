using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QApp.Models.Entities
{
    public partial class MilljasContext : DbContext
    {
        public TellerQueueVM HelpNextCustomer(string aspUserId)
        {
            TellerQueueVM tellerQueueVM = new TellerQueueVM();

            bool isLastCard = true;

            //Hämtar den teller som matchar med den som klickar på knappens id
            User user = User.Single(i => i.AspNetUserId == aspUserId);
            //Kollar vilken counter den tellern är på
            //int counterId = Counter.Where(c => c.TellerId == user.Id).Select(ci => ci.Id).First();

            Counter counter = Counter.Where(c => c.TellerId == user.Id).Single();
            Card card = Card.OrderBy(c => c.Id).Where(cntr => cntr.QueueId == counter.QueueId && cntr.CounterId == null).FirstOrDefault();

            //Hitta nästa card som inte är kopplad till en counter..  OCH KOLLA KÖ-ID OCKSÅ
            //Card card = Card.OrderBy(c => c.Id).Where(i => i.CounterId == null).FirstOrDefault();



            if (card != null)
            {
                isLastCard = false;

                // Ta ut cardets id och nummer och tilldela variablerna
                int nextCardID = card.Id;
                int nextCardNumber = card.CardNumber;

                Card.Find(nextCardID).CounterId = counter.Id; //Ändrat denna från counterId
                Card.Find(nextCardID).TellerId = user.Id;
                Card.Find(nextCardID).ServiceStart = DateTime.Now;
                Counter.Find(counter.Id).CardId = nextCardID; //Ändrat inparameter från counterId

                tellerQueueVM.CardNumber = nextCardNumber;
            }

            //Hittar kortet som ska "stängas"
            Card cardToClose = Card.OrderBy(ci => ci.Id).Where(c => c.CounterId == counter.Id).LastOrDefault(); //Ändrat från counterId

            //Sätter serviceEnd på kortet som ska stängas
            if (cardToClose != null)
            {
                cardToClose.ServiceEnd = DateTime.Now;
            }

            SaveChanges();

            tellerQueueVM.isLastCard = isLastCard;
            return tellerQueueVM;
        }


        public void RemoveTellerFromQueue(string aspUserId)
        {
            //TellerQueueVM viewModel = new TellerQueueVM();
            //string message = null;

            //viewModel.CustomersLeftInQueue = 0;

            User user = User.SingleOrDefault(i => i.AspNetUserId == aspUserId);

            //Sätter queueid till null på den counter som trycker på knappen
            Counter counter = Counter.SingleOrDefault(t => t.TellerId == user.Id);

            //Kort i kassan
            //Lagt till onsdag kväll för att sätta service-end när ett kort står i kassan när den stänger
            Card cardServed = Card.OrderBy(c => c.Id).Where(c => c.CounterId == counter.TellerId && c.SessionId != null).LastOrDefault();

            ////Om sista kassan stänger hittar vi alla cards i kön som inte fått hjälp och nullar deras session-id
            //bool isLastCounter = Counter.Where(c => c.TellerId != null).Count() == 1;

            //if (isLastCounter)
            //{
            var lastCardsInQueue = Card.Where(c => c.CounterId == null && c.QueueId == counter.QueueId).ToList(); //kolla att det är min kö också

            //    //Detta borde tala om hur många kunder som är kvar i kön när jag väl vill stänga sista kassan.
            //    int customersLeftInQueue = Card.Count(c => c.QueueId == counter.QueueId && c.ServiceStart != null);
            //    message = $"Obs! Det står {customersLeftInQueue} kunder kvar i kön. Vill du verkligen stänga den?";
            //    viewModel.CustomersLeftInQueue = customersLeftInQueue;

            foreach (var item in lastCardsInQueue)
            {
                item.SessionId = null;
            }
            //}

            counter.QueueId = null;
            counter.TellerId = null;
            counter.CardId = null;
            cardServed.ServiceEnd = DateTime.Now; //Lagt till onsdag kväll för att sätta service-end när ett kort står i kassan när den stänger
            SaveChanges();

            //viewModel.Message = message;  //Lagt till denna för att informera om personer i kön 

            //return viewModel;
        }



        public TellerQueueVM CheckCounter(string aspUserId)
        {
            TellerQueueVM viewModel = new TellerQueueVM();

            string message = null;
            viewModel.CustomersLeftInQueue = 0;
            bool isLastCounter = false;

            User user = User.SingleOrDefault(i => i.AspNetUserId == aspUserId);
            Counter counter = Counter.SingleOrDefault(t => t.TellerId == user.Id);

            //Om det är sista countern som har teller-id sätt isLastCounter till true
            if (Counter.Where(c => c.TellerId != null).Count() == 1)
            {
                isLastCounter = true;

                //Behöver jag sessionid != null?
                int customersLeftInQueue = Card.Count(c => c.QueueId == counter.QueueId && c.ServiceStart == null && c.SessionId != null);


                message = $"Obs! Det står {customersLeftInQueue} kunder kvar i kön. Vill du verkligen stänga den?";

                viewModel.isLastCounter = isLastCounter;
                viewModel.CustomersLeftInQueue = customersLeftInQueue;
                viewModel.Message = message;

            }

            return viewModel;
        }


        //Vi stoppar in aspnetUserId här
        public void PopulateQueue(string aspUserId)
        //public TellerQueueVM PopulateQueue(string aspUserId)
        {

            //User user = User.SingleOrDefault(i => i.AspNetUserId == aspUserId);

            //int activeCounters = 0;
            //activeCounters = Counter.Count(c => c.QueueId != null);

            //if (activeCounters == 0)
            //{
            //    Queue queue = new Queue();
            //    queue.Name = DateTime.Now.ToString();
            //    Queue.Add(queue);

            //    Counter counterToOpen = Counter.OrderBy(c => c.Id).First(c => c.QueueId == null);
            //    counterToOpen.QueueId = queue.Id;
            //    counterToOpen.TellerId = user.Id;
            //    SaveChanges();
            //}
            //else
            //{
            //    bool tellerAlreadyActive = Counter.Where(c => c.TellerId == user.Id) != null;

            //    if (!tellerAlreadyActive)
            //    {
            //        Counter counterToOpen = Counter.OrderBy(c => c.Id).First(c => c.QueueId == null);
            //        Queue queueToJoin = Queue.OrderBy(c => c.Id).Last();

            //        counterToOpen.QueueId = queueToJoin.Id;
            //        counterToOpen.TellerId = user.Id;
            //        SaveChanges();
            //    }

            //}

            //TellerQueueVM viewModel = new TellerQueueVM();

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
                //viewModel.MyCounter = Counter.Find(1).CounterName;
                SaveChanges();
            }
            else
            {
                //Kollar om första kassans tellerid stämmer med det userid som öppnar kassan
                //bool tellerAlreadyActive = Counter.FirstOrDefault().TellerId == user.Id;
                //Ska vi verkligen titta på den första bara?
                Counter counter = Counter.FirstOrDefault(tid => tid.TellerId == user.Id);

                if (counter == null)
                {
                    // Sorterar kötabell på id och tar den senast skapade kön
                    int activeQueue = Queue.OrderBy(q => q.Id).Select(p => p.Id).LastOrDefault();

                    // Tar nästa lediga kassa och sätter teller id till user id samt queueid till den aktiva köns id
                    //DENNA VERKAR FEL?? DEN SÄTTER PERSON 2 I FÖRSTA KASSAN ALLTID
                    Counter.Where(q => q.QueueId == null).First().QueueId = activeQueue;
                    Counter.Where(q => q.TellerId == null).First().TellerId = user.Id;
                    //viewModel.MyCounter = counter.CounterName; //LAGT TILL DENNA FÖR ATT VISA UPP VILKEN KASSA JAG STÅR I
                    SaveChanges();
                }
                else
                {
                    //TODO Skicka tillbaka att tellern redan bemannar en kassa
                }


            }
            //return viewModel;
        }


        //Ska visa hur många kunder som står i kö
        public TellerQueueVM CustomersInQueue(string aspUserId)
        {
            //int customersInQueue = 0;
            TellerQueueVM viewModel = new TellerQueueVM();

            User user = User.SingleOrDefault(i => i.AspNetUserId == aspUserId);
            Counter counter = Counter.SingleOrDefault(t => t.TellerId == user.Id);

            //Uppdaterat denna, var den som bråkade när någon gick ur kön
            int customersInQueue = Card.Count(c => c.QueueId == counter.QueueId && c.ServiceStart == null && c.SessionId != null);

            viewModel.TellerName = $"{user.FirstName} {user.LastName}";
            viewModel.MyCounter = counter.CounterName; //För att visa upp min counter
            viewModel.CustomersLeftInQueue = customersInQueue;

            return viewModel;
        }
    }
}
