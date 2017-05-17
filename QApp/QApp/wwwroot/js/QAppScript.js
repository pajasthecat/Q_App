var interval = 0;
var queueinterval = 0;

$(document).ready(function () {
    queueinterval = setInterval(showcustomersinqueue, 3000);
});


function helpnextcustomer() {
    $.ajax({
        url: "helpnextcustomer",
        success: function (result) {
            $("#showCardNumberToTeller").html(result.cardNumber);

            //När noll personer är kvar i kön vill jag skriva ut det

            if (result.customersLeftInQueue == 0) {
                $("#customersInQueue").html("Nu står inga fler i kö!");
            }

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
            $("#customersInQueue").html(result.customersLeftInQueue);
        }
    });
};

//Lägg till kod om att det står personer kvar i kön
//SKa den varna och direkt redirecta till home eller varna och tvinga tellern att trycka på något?
function closecounter() {
    $.ajax({
        url: "/teller/closecounter",
        success: function (result) {

            if (queueinterval > 0)
                clearInterval(queueinterval);

            if (result.customersLeftInQueue > 0)
            {
                if (confirm(result.message) == true) {
                    window.location.href = "/teller/home";
                }
                else {
                    $("#openCounterButton").trigger("click");
                    queueinterval = setInterval(showcustomersinqueue, 3000);
                }
            }           
        }
    });

}



function joinqueue() {
    $.ajax({
        url: "/customer/GetCustomerCardNumber",
        success: function (result) {
            $("#showCardNumber").html(result.cardNumber);
            $("#showCardNumber").show();

            $("#joinQueueButton").addClass("button");
            $("#joinQueueButton").hide();
            $("#queuealert").show();
            //$("#leaveQueue").show();
            console.log(result.cardNumber);
            showposition();
            interval = setInterval(showposition, 3000) //Ändra tillbaka
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

            if (result.cardNumber == 0) {

                $("#showCardNumber").html(result.cardNumber);
                $("#showCardNumber").hide();
                $("#joinQueueButton").show();
                clearInterval(interval);
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

