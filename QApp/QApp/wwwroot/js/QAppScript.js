﻿function helpnextcustomer() {
    $.ajax({
        url: "helpnextcustomer",
        success: function (result) {
            $("#showCardNumberToTeller").html(result.cardNumber);
        }
    });
}


var interval = 0;

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

