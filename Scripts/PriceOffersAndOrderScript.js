

// Closing form by close buttom
$("#close-btn").on("click", function () {
    $("#check-price-form").hide();
    $("#fromToDestination").show();
});

// Closing pricerequest by close buttom
function closePriceSuggestion() {
    $("#PriceSuggestion").hide();
    $("#fromToDestination").show();
};
// Closing booking confirmation by close buttom
function closeBookingConfirmation() {
    $("#OrderConfirmation").hide();
    $("#fromToDestination").show();
}

// Save pricerequest
function savePriceRequestToDatabase(el) {
    $.ajax({
        url: '/api/PriceOffer/PostPriceOffer',
        async: true,
        contentType: 'application/json; charset=UTF-8',
        type: 'POST',
        dataType: 'json',
        data: JSON.stringify(el),
        success: function (data, textStatus, xhr) {
            console.log(data);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest.responseText);
        }
    });
};

// Get current priceofferid
function GetPriceOfferById(id) {
    $.ajax({
        url: '/api/PriceOffer/GetPriceOffer?id=' + id,
        async: false,
        contentType: 'application/json; charset=UTF-8',
        type: 'GET',
        success: function (data, textStatus, xhr) {
            $("#latestid").text(data);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest.responseText);
        }
    });
}

//Get current id for price requests by current username
function GetLatestId() {
    $.ajax({
        url: 'home/GetLatestPriceOffer',
        async: false,
        contentType: 'application/json; charset=UTF-8',
        type: 'GET',
        success: function (data, textStatus, xhr) {
            $("#latestid").text(data);
            $("#priceoffer-id").attr("onclick", "GetPriceOfferById(" + data + ")");
            $("#priceoffer-id").append("<a target='_blank' href=" + location.origin + "/PriceSuggestions/Details/" + data + ">" + location.origin + "/PriceSuggestions/Details/" + data + "</a>");
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest.responseText);
        }
    });
}

// Order Movment
function Order() {
    var offerid = $("#latestid").text();
    var username = $("#Username").val();
    var model = { 'Id': offerid, 'UserName': username }
    var price = $("#price").text();
    if (price != 0) {
        $.ajax({
            cache: false,
            url: '/api/PriceOffer/Order',
            async: true,
            contentType: 'application/json; charset=UTF-8',
            type: 'POST',
            dataType: 'json',
            data: JSON.stringify(model),
            success: function (data, textStatus, xhr) {
                var el = data;
                $("#PriceSuggestion").hide();
                $("#OrderConfirmation").html("<div class='container'>"
                    + "<div class='row'><div class='col-xs-12 col-sm-12 col-md-12 col-lg-12'><br/><h1>Bokningsbekräftelse</h1>"
                    + "<br/><p><h4>Hej " + el.userName + " vi har tagit emot din bokning och ser fram emot att hjälpa dig med flytten.</h5>"
                    + "<br/><p><b>Bokningsnummer: </b>" + el.id
                    + "<p><b>Offert id: </b>" + el.offertId + "</p><br/>"
                    + "<p><h4><b>Vi kommer att kontakta dig i god tid innan inbokat datum för närmare beskrivning.</b></h4></p>"
                    + "<br/><p><H4>Tack för att du valt att ta hjälp utav oss för att underlätta din flytt!</h4></p>"
                    + "<input type='button' id='close-booking-confirmation-btn' onClick='closeBookingConfirmation()' value='Stäng' />"
                    + "</div></div></div>"
                )
                console.log(el);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(XMLHttpRequest.responseText);
            }
        });
    } else {
        var msg = $("#Message").text("Eftersom priset är 0 kr så går det inte att boka flyttningen, försök igen.");
        $("#Message").val(msg).data("value");
    }
};

