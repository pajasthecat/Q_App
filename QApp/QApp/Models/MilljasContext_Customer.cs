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
            bool alreadyInQueue = Card.Where(c => c.SessionId == sessionId).Count() > 0;

            //Card card = Card.Single(c => c.SessionId == sessionId);

            //Kollar att det finns en kö och att personen inte redan har ett card
            if (queueIsActive && !alreadyInQueue)
            {
                int numberOfCards = Card.Where(c => c.QueueId == activeQueue).Count();

                //Card lastCardNumber = Card.OrderBy(ci => ci.Id).Where(c => c.QueueId == activeQueue).LastOrDefault();

                Card card = new Card();

                //card.CardNumber = lastCardNumber.CardNumber +=1;
                card.CardNumber = numberOfCards + 1;
                card.CardCreated = DateTime.Now;
                card.QueueId = activeQueue;
                card.SessionId = sessionId;
                Card.Add(card);
                SaveChanges();
            }
            else //if (!queuisActive)
            {
                //"Det finns ingen aktiv kö, välkommen att försöka senare.";

                //LÄgg till ett message i vymodellen
            }
        }

        public CustomerIndexVM GetPositionInQueue(string sessionId)
        {
            int cardsBeforeYou = 0;
            string message = null;
            CustomerIndexVM viewModel = new CustomerIndexVM();
            bool myTurn = false;
            Card card = null;

            try
            {
                //Hämtar mitt card och kollar sedan hur många cards med id lägre än mitt som inte fått ngt counterId, alltså hjälp
                card = Card.Single(c => c.SessionId == sessionId);
                cardsBeforeYou = Card.Count(cn => cn.Id < card.Id && cn.CounterId == null && cn.QueueId == card.QueueId); //Lagt till koll mot kö-id

            }
            catch (Exception)
            {

            }

            if (card != null)
            {
                //En bool myTurn som blir true ifall mitt cardid är lika med card-id för senaste kortet med counter-id?
                myTurn = card.CounterId != null;
                viewModel.CardNumber = card.CardNumber;

                message = $"Det är {cardsBeforeYou + 1} personer före dig i kön.";

                if (myTurn)
                {

                    Counter counter = Counter.Find(card.CounterId);
                    message = $"Nu är det din tur! Gå till {counter.CounterName}";

                    if (card.ServiceEnd != null)
                    {
                        card.SessionId = null;
                        SaveChangesAsync();
                        viewModel.CardNumber = 0;

                    }
                }
            }
            else
            {
                viewModel.CardNumber = 0;
                message = null;
            }

            //viewModel.NumbersLeftInQueue = cardsBeforeYou;

            viewModel.Message = message;
            viewModel.MyTurn = myTurn;
            return viewModel;
        }
    }
}
