﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        UserManager<IdentityUser> userManager;
        RoleManager<IdentityRole> roleManager;

        public MilljasContext(DbContextOptions<MilljasContext> options, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : base(options)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;

        }

        //FLYTTAT TILL TELLER-CONTEXT
        //public TellerQueueVM HelpNextCustomer(string aspUserId)
        //{
        //    TellerQueueVM tellerQueueVM = new TellerQueueVM();

        //    bool isLastCard = true;

        //    //Hämtar den teller som matchar med den som klickar på knappens id
        //    User user = User.Single(i => i.AspNetUserId == aspUserId);
        //    //Kollar vilken counter den tellern är på
        //    //int counterId = Counter.Where(c => c.TellerId == user.Id).Select(ci => ci.Id).First();

        //    Counter counter = Counter.Where(c => c.TellerId == user.Id).Single();
        //    Card card = Card.OrderBy(c => c.Id).Where(cntr => cntr.QueueId == counter.QueueId && cntr.CounterId == null).FirstOrDefault();

        //    //Hitta nästa card som inte är kopplad till en counter..  OCH KOLLA KÖ-ID OCKSÅ
        //    //Card card = Card.OrderBy(c => c.Id).Where(i => i.CounterId == null).FirstOrDefault();

        //    if (card != null)
        //    {
        //        isLastCard = false;

        //        // Ta ut cardets id och nummer och tilldela variablerna
        //        int nextCardID = card.Id;
        //        int nextCardNumber = card.CardNumber;

        //        Card.Find(nextCardID).CounterId = counter.Id; //Ändrat denna från counterId
        //        Card.Find(nextCardID).TellerId = user.Id;
        //        Card.Find(nextCardID).ServiceStart = DateTime.Now;
        //        Counter.Find(counter.Id).CardId = nextCardID; //Ändrat inparameter från counterId

        //        tellerQueueVM.CardNumber = nextCardNumber;
        //    }

        //    //Hittar kortet som ska "stängas"
        //    Card cardToClose = Card.OrderBy(ci => ci.Id).Where(c => c.CounterId == counter.Id).LastOrDefault(); //Ändrat från counterId

        //    //Sätter serviceEnd på kortet som ska stängas
        //    if (cardToClose != null)
        //    {
        //        cardToClose.ServiceEnd = DateTime.Now;
        //    }

        //    SaveChanges();

        //    tellerQueueVM.isLastCard = isLastCard;
        //    return tellerQueueVM;
        //}


        //FLYTTAT TILL CUSTOMER-CONTEXT
        //public CustomerIndexVM GetCardNumber(string sessionId)
        //{
        //    CustomerIndexVM customerIndexVM = new CustomerIndexVM();
        //    try
        //    {
        //        int cardNumber = Card.Where(s => s.SessionId == sessionId).Select(si => si.CardNumber).Single();
        //        customerIndexVM.CardNumber = cardNumber;
        //    }
        //    catch (Exception)
        //    {

        //    }

        //    return customerIndexVM;
        //}

        //FLYTTAT TILL CUSTOMER-CONTEXT
        //public void AddCustomerToQueue(string sessionId)
        //{
        //    int activeQueue = Queue.OrderBy(q => q.Id).Select(p => p.Id).LastOrDefault();

        //    bool queueIsActive = Counter.FirstOrDefault().QueueId != null;

        //    //Ska kolla om personen (med samma session) redan står i kö.
        //    bool alreadyInQueue = Card.Where(c => c.SessionId == sessionId).Count()> 0;

        //    //Card card = Card.Single(c => c.SessionId == sessionId);

        //    //Kollar att det finns en kö och att personen inte redan har ett card
        //    if (queueIsActive && !alreadyInQueue)
        //    {
        //        int numberOfCards = Card.Where(c => c.QueueId == activeQueue).Count();

        //        //Card lastCardNumber = Card.OrderBy(ci => ci.Id).Where(c => c.QueueId == activeQueue).LastOrDefault();

        //        Card card = new Card();

        //        //card.CardNumber = lastCardNumber.CardNumber +=1;
        //        card.CardNumber = numberOfCards + 1;
        //        card.CardCreated = DateTime.Now;
        //        card.QueueId = activeQueue;
        //        card.SessionId = sessionId;
        //        Card.Add(card);
        //        SaveChanges();
        //    }
        //    else //if (!queuisActive)
        //    {
        //        //"Det finns ingen aktiv kö, välkommen att försöka senare.";

        //        //LÄgg till ett message i vymodellen
        //    }
        //}

        //FLYTTAT TILL TELLER-CONTEXT
        //public void RemoveTellerFromQueue(string aspUserId)
        //{
        //    //TellerQueueVM viewModel = new TellerQueueVM();
        //    //string message = null;

        //    //viewModel.CustomersLeftInQueue = 0;

        //    User user = User.SingleOrDefault(i => i.AspNetUserId == aspUserId);

        //    //Sätter queueid till null på den counter som trycker på knappen
        //    Counter counter = Counter.SingleOrDefault(t => t.TellerId == user.Id);

        //    ////Om sista kassan stänger hittar vi alla cards i kön som inte fått hjälp och nullar deras session-id
        //    //bool isLastCounter = Counter.Where(c => c.TellerId != null).Count() == 1;

        //    //if (isLastCounter)
        //    //{
        //    var lastCardsInQueue = Card.Where(c => c.CounterId == null && c.QueueId == counter.QueueId).ToList(); //kolla att det är min kö också

        //    //    //Detta borde tala om hur många kunder som är kvar i kön när jag väl vill stänga sista kassan.
        //    //    int customersLeftInQueue = Card.Count(c => c.QueueId == counter.QueueId && c.ServiceStart != null);
        //    //    message = $"Obs! Det står {customersLeftInQueue} kunder kvar i kön. Vill du verkligen stänga den?";
        //    //    viewModel.CustomersLeftInQueue = customersLeftInQueue;

        //    foreach (var item in lastCardsInQueue)
        //    {
        //        item.SessionId = null;
        //    }
        //    //}

        //    counter.QueueId = null;
        //    counter.TellerId = null;
        //    counter.CardId = null;
        //    SaveChanges();

        //    //viewModel.Message = message;  //Lagt till denna för att informera om personer i kön 

        //    //return viewModel;
        //}


        //FLYTTAT TILL TELLER-CONTEXT
        //Ska kolla om det finns personer kvar i kön när sista tellern stänger sin counter
        //public TellerQueueVM CheckCounter(string aspUserId)
        //{
        //    TellerQueueVM viewModel = new TellerQueueVM();

        //    string message = null;
        //    viewModel.CustomersLeftInQueue = 0;
        //    bool isLastCounter = false;

        //    User user = User.SingleOrDefault(i => i.AspNetUserId == aspUserId);
        //    Counter counter = Counter.SingleOrDefault(t => t.TellerId == user.Id);

        //    //Om det är sista countern som har teller-id sätt isLastCounter till true
        //    if (Counter.Where(c => c.TellerId != null).Count() == 1)
        //    {
        //        isLastCounter = true;
        //        int customersLeftInQueue = Card.Count(c => c.QueueId == counter.QueueId && c.ServiceStart == null);
        //        message = $"Obs! Det står {customersLeftInQueue} kunder kvar i kön. Vill du verkligen stänga den?";

        //        viewModel.isLastCounter = isLastCounter;
        //        viewModel.CustomersLeftInQueue = customersLeftInQueue;
        //        viewModel.Message = message;

        //    }

        //    return viewModel;
        //}



        //FLYTTAT TILL TELLER-CONTEXT
        ////Vi stoppar in aspnetUserId här
        //public void PopulateQueue(string aspUserId)
        //{
        //    // Hämtar alla kassors qId till en lista 
        //    List<Counter> counters = Counter.Select(c => new Counter
        //    {
        //        QueueId = c.QueueId
        //    }).ToList();

        //    int activeCounters = 0;

        //    //Borde gå att snygga till
        //    //För varje kassa som har ett queueId plussar vi på activeCounters
        //    foreach (var counter in counters)
        //    {
        //        if (counter.QueueId != null)
        //        {
        //            activeCounters++;
        //        }
        //    }

        //    User user = User.SingleOrDefault(i => i.AspNetUserId == aspUserId);

        //    //Om inga kassor är aktiva skapar vi ny kö och bemannar den
        //    if (activeCounters == 0)
        //    {
        //        Queue queue = new Queue();
        //        queue.Name = DateTime.Now.ToString();
        //        Queue.Add(queue);

        //        Counter.Find(1).TellerId = user.Id;
        //        //Tar kassa 1 och sätter queueid till den nya köns id
        //        Counter.Find(1).QueueId = queue.Id;
        //        SaveChanges();
        //    }
        //    else
        //    {
        //        //Kollar om första kassans tellerid stämmer med det userid som öppnar kassan
        //        //bool tellerAlreadyActive = Counter.FirstOrDefault().TellerId == user.Id;
        //        //Ska vi verkligen titta på den första bara?
        //        Counter counter = Counter.FirstOrDefault(tid => tid.TellerId == user.Id);

        //        if (counter == null)
        //        {
        //            // Sorterar kötabell på id och tar den senast skapade kön
        //            int activeQueue = Queue.OrderBy(q => q.Id).Select(p => p.Id).LastOrDefault();

        //            // Tar nästa lediga kassa och sätter teller id till user id samt queueid till den aktiva köns id
        //            Counter.Where(q => q.QueueId == null).First().QueueId = activeQueue;
        //            Counter.Where(q => q.TellerId == null).First().TellerId = user.Id;
        //            SaveChanges();
        //        }
        //        else
        //        {
        //            //TODO Skicka tillbaka att tellern redan bemannar en kassa
        //        }
        //    }
        //}

        //FLYTTAT TILL CUSTOMER-CONTEXT
        //public CustomerIndexVM GetPositionInQueue(string sessionId)
        //{
        //    int cardsBeforeYou = 0;
        //    string message = null;
        //    CustomerIndexVM viewModel = new CustomerIndexVM();
        //    bool myTurn = false;
        //    Card card = null;

        //    try
        //    {
        //        //Hämtar mitt card och kollar sedan hur många cards med id lägre än mitt som inte fått ngt counterId, alltså hjälp
        //        card = Card.Single(c => c.SessionId == sessionId);
        //        cardsBeforeYou = Card.Count(cn => cn.Id < card.Id && cn.CounterId == null && cn.QueueId == card.QueueId); //Lagt till koll mot kö-id

        //    }
        //    catch (Exception)
        //    {

        //    }

        //    if (card != null)
        //    {
        //        //En bool myTurn som blir true ifall mitt cardid är lika med card-id för senaste kortet med counter-id?
        //        myTurn = card.CounterId != null;
        //        viewModel.CardNumber = card.CardNumber;

        //        message = $"Det är {cardsBeforeYou + 1} personer före dig i kön.";

        //        if (myTurn)
        //        {

        //            Counter counter = Counter.Find(card.CounterId);
        //            message = $"Nu är det din tur! Gå till {counter.CounterName}";

        //            if (card.ServiceEnd != null)
        //            {
        //                card.SessionId = null;
        //                SaveChangesAsync();
        //                viewModel.CardNumber = 0;

        //            }
        //        }
        //    }
        //    else
        //    {
        //        viewModel.CardNumber = 0;
        //        message = null;
        //    }

        //    //viewModel.NumbersLeftInQueue = cardsBeforeYou;

        //    viewModel.Message = message;
        //    viewModel.MyTurn = myTurn;
        //    return viewModel;
        //}

        //FLYTTAT TILL TELLER-CONTEXT
        ////Ska visa hur många kunder som står i kö
        //public TellerQueueVM CustomersInQueue(string aspUserId)
        //{
        //    int customersInQueue = 0;
        //    TellerQueueVM viewModel = new TellerQueueVM();

        //    User user = User.SingleOrDefault(i => i.AspNetUserId == aspUserId);
        //    Counter counter = Counter.SingleOrDefault(t => t.TellerId == user.Id);

        //    customersInQueue = Card.Count(c => c.QueueId == counter.QueueId && c.ServiceStart == null);

        //    viewModel.CustomersLeftInQueue = customersInQueue;

        //    return viewModel;
        //}

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





        //när vi ska få cardnumber--- hitta maxvärde och lägg på ett.. funkar ej?


        //Fixa så det på något sätt syns om det inte finns några personer i kön i stället för att numret blir 0 för tellern. 
    }
}
