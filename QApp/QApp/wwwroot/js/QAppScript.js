var interval = 0;
var queueinterval = 0;




function helpnextcustomer() {
    $.ajax({
        url: "helpnextcustomer",
        success: function (result) {
            $("#showCardNumberToTeller").html(result.cardNumber);

            //Om det är sista kortet vill jag gömma siffran för aktuellt kort
            if (result.isLastCard == true) {
                $("#showCardNumberToTeller").hide();
                //$("#customersInQueue").hide(); 
            }
        }
    });
}

function showcustomersinqueue() {
    $.ajax({
        url: "/teller/CustomersInQueue",
        success: function (result) {

            //När noll personer är kvar i kön vill jag skriva ut det
            if (result.customersLeftInQueue == 0) {
                $("#customersInQueue").html("Nu står inga fler i kö!"); //Meddelandet visas jättekort, sen kommer 0 tillbaka
            }
            else {
                $("#customersInQueue").html(result.customersLeftInQueue);
            }
        }
    });
};


function checkcounter() {
    $.ajax({
        url: "/teller/CheckCounter",
        success: function (result) {

            if (result.isLastCounter == false) {
                closecounter();
            }
            else if (result.isLastCounter == true && result.customersLeftInQueue == 0) {
                closecounter();
            }
            else if (result.isLastCounter == true && result.customersLeftInQueue > 0) {
                if (confirm(result.message) == true) {
                    closecounter();
                    window.location.href = "/teller/home";
                }
                else {

                }
            }

        }
    });
};


function closecounter() {
    $.ajax({
        url: "/teller/closecounter",
        success: function (result) {

            window.location.href = "/teller/home";

        }
    });
}




//Lägg till meddelande om ingen kö finns aktiv.. nu syns 0 snabbt och sen döljs den igen
function joinqueue() {
    $.ajax({
        url: "/customer/GetCustomerCardNumber",
        success: function (result) {

            // Flyttat till showposition else
            //$("#showCardNumber").html(result.cardNumber);
            //$("#showCardNumber").show();

            $("#joinQueueButton").addClass("button");
            $("#joinQueueButton").hide();
            $("#queuealert").show();
            //$("#leaveQueue").show();
            //console.log(result.cardNumber);
            showposition();
            interval = setInterval(showposition, 3000); //Ändra tillbaka
        }
    });
}

function showposition() {
    $.ajax({
        url: "/customer/ShowPositionInQueue",
        success: function (result) {
            //$("#showNumbersBeforeYou").html(result.numbersLeftInQueue);
            //console.log(result.numbersLeftInQueue);
            $("#queuealert").html(result.message);

            $("#showCardNumber").html(result.cardNumber);
            if (result.cardNumber == 0) {
                $("#showCardNumber").hide();
                $("#joinQueueButton").show();
                //clearInterval(interval);
            }
            else {
                $("#showCardNumber").show();
            }


            //if (result.numbersLeftInQueue > 0) {
            //    $("#queuealert").html("Nu är det " + result.numbersLeftInQueue + " personer före dig i kön");
            //    $("#queuealert").show();
            //}
            //else if (result.numbersLeftInQueue == 0) {
            //    $("#queuealert").html("Du är näst på tur!");
            //    $("#queuealert").show();
            //}


            //Lägg till att stänga av timern

        }
    });
}

    //Gör en funktion som tar emot signal om när personen får hjälp via en bool
    //Sätt då session id till null, nollställ sidan och redirecta till index

    //Funktion för att lämna kö
    //function leavequeue() {
    //    $.ajax({
    //        url: "LeaveCustomerQueue",
    //        success: function () {
    //            $("#joinQueue").show();
    //            $("#leaveQueue").hide();

    //        }
    //    });

    //}

    //Anrop till databas för att hålla kön uppdaterad

