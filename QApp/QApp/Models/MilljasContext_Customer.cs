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
        //Kunden ställer sig i kö
        public void AddCustomerToQueue(string sessionId)
        {
            CustomerIndexVM viewModel = new CustomerIndexVM();

            bool queueIsActive = Counter.FirstOrDefault().QueueId != null;
            int activeQueue = Queue.OrderBy(q => q.Id).Select(p => p.Id).LastOrDefault();

            //Ska kolla om personen (med samma session) redan står i kö.
            bool alreadyInQueue = Card.Where(c => c.SessionId == sessionId).Count() > 0;

            //Kollar att det finns en kö och att personen inte redan har ett card
            if (queueIsActive && !alreadyInQueue)
            {
                Card card = new Card();
                //För att räkna ut könummer
                int cardsBeforeMe = Card.Where(c => c.QueueId == activeQueue).Count();
                             
                card.CardNumber = cardsBeforeMe + 1;
                card.CardCreated = DateTime.Now;
                card.QueueId = activeQueue;
                card.SessionId = sessionId;
                Card.Add(card);
                SaveChanges();
            }
            else if (!queueIsActive)
            {
                //Skicka på något sätt tillbaka ett meddelande. Kanske i JS genom en bool då vi inte returnerar vymodell?
            }
        }

        //Visar kundens könummer
        public CustomerIndexVM GetCardNumber(string sessionId)
        {
            CustomerIndexVM customerIndexVM = new CustomerIndexVM();
            try
            {
                //Returnerar könumret baserat på sessionId
                int cardNumber = Card.Where(s => s.SessionId == sessionId).Select(si => si.CardNumber).Single();
                customerIndexVM.CardNumber = cardNumber;
            }
            //Behöver vi denna?
            catch (Exception)
            {

            }
            return customerIndexVM;
        }

        //Visar kundens position i kön
        public CustomerIndexVM GetPositionInQueue(string sessionId)
        {
            CustomerIndexVM viewModel = new CustomerIndexVM();
            Card card = null;

            int cardsInQueueBeforeMe = 0;
            string message = null;
            bool myTurn = false;

            try
            {
                //Hämtar mitt card
                card = Card.Single(c => c.SessionId == sessionId);

                //Kollar hur många kort före mig som inte fått hjälp
                cardsInQueueBeforeMe = Card.Count(cn => cn.Id < card.Id && cn.ServiceStart == null && cn.QueueId == card.QueueId && cn.SessionId != null); //Lagt till koll mot kö-id

            }
            //Behövs denna?
            catch (Exception)
            {

            }

            //Om jag har ett kort
            if (card != null)
            {
                //Min tur om mitt kort fått CounterId
                myTurn = card.CounterId != null;

                viewModel.CardNumber = card.CardNumber;
                message = $"Det är {cardsInQueueBeforeMe + 1} personer före dig i kön.";

                if (myTurn)
                {
                    Counter counter = Counter.Find(card.CounterId);
                    message = $"Nu är det din tur! Gå till {counter.CounterName}";

                    //PATRIK - kolla denna
                    if (card.ServiceEnd != null)
                    {
                        card.SessionId = null;
                        SaveChangesAsync();
                        viewModel.CardNumber = 0;
                    }
                }
            }
            //Om jag inte har ett kort
            else
            {
                viewModel.CardNumber = 0;
                message = null;
            }

            viewModel.Message = message;
            viewModel.MyTurn = myTurn;
            return viewModel;
        }


        //När kunden går ur kön
        public void LeaveCustomerQueue(string sessionId)
        {
            //Hitta mitt kort
            Card card = Card.SingleOrDefault(c => c.SessionId == sessionId);

            //Om jag har ett kort sätts session-id till null
            if(card != null)
            {
                card.SessionId = null;
                SaveChanges();
            }
        }
    }
}

    
