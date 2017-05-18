var interval = 0;
var queueinterval = 0;

//TELLER
function helpnextcustomer() {
    $.ajax({
        url: "helpnextcustomer",
        success: function (result) {
            $("#displayToTeller").html(result.cardNumber);
            $("#displayToTeller").show();

            //Om det är sista kortet vill jag gömma siffran för aktuellt kort
            if (result.isLastCard == true) {
                $("#displayToTeller").hide();
                //$("#customersInQueue").hide(); 
            }
        }
    });
}

function showcustomersinqueue() {
    $.ajax({
        url: "/teller/CustomersInQueue",
        success: function (result) {

            $("#myCounter").html(result.myCounter);
            $("#tellerName").html(result.tellerName);



            //När noll personer är kvar i kön vill jag skriva ut det
            if (result.customersLeftInQueue == 0) {
                $("#customersInQueue").html("Inga personer i kö."); //Meddelandet visas jättekort, sen kommer 0 tillbaka

            }
            else {
                $("#customersInQueue").html(result.customersLeftInQueue + " personer står i kö.");

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

//CUSTOMER
function joinqueue() {
    $.ajax({
        url: "/customer/GetCustomerCardNumber",
        success: function (result) {

            // Flyttat till showposition else
            //$("#showCardNumber").html(result.cardNumber);
            //$("#showCardNumber").show();

            //Funkar när vi försöker gå med i en kö när kassan är stängd
            if (result.openQueue == false) {
                window.alert("Kassan är stängd, välkommen att försöka senare.")
            }

            $("#joinQueueButton").addClass("button");
            $("#joinQueueButton").hide();
            $("#queuealert").show();
            $("#leaveQueueButton").show();
            showposition();
            interval = setInterval(showposition, 3000);
        }
    });
}

function showposition() {
    $.ajax({
        url: "/customer/ShowPositionInQueue",
        success: function (result) {
            $("#queuealert").html(result.message);

            if (result.myTurn == true) {
                $("#leaveQueueButton").hide();
            }

            $("#showCardNumber").html(result.cardNumber);
            if (result.cardNumber == 0) {
                $("#showCardNumber").hide();
                $("#joinQueueButton").show();
                $("#leaveQueueButton").hide();
                //clearInterval(interval);
            }
            else {
                $("#showCardNumber").show();
            }

        }
    });
}

function leaveQueue() {
    $.ajax({
        url: "/customer/LeaveCustomerQueue",
        success: function () {
            $("#joinQueueButton").show();
            $("#leaveQueueButton").hide();
            $("#queuealert").hide();
        }
    });
}


