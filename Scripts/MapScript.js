var source, destination;
var directionsDisplay = new google.maps.DirectionsRenderer();
var directionsService = new google.maps.DirectionsService();

google.maps.event.addDomListener(window, 'load', initMap);

function initMap() {
    google.maps.event.addDomListener(window, 'load', function () {
        directionsDisplay = new google.maps.DirectionsRenderer({ 'draggable': true });
    });

    var map;
    var myLatlng = new google.maps.LatLng(18.9750, 72.8258);
    var mapOptions = {
        zoom: 12,
        center: myLatlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        mapTypeControl: false
    };

    map = new google.maps.Map(document.getElementById('map'), mapOptions);
    directionsDisplay.setMap(map);
    directionsDisplay.setPanel(document.getElementById('dvPanel'));
    infoWindow = new google.maps.InfoWindow;

    // Here the map will be located to the place were you are
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var pos = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };
            console.log(pos);
            // Here we set a marker at the place were we are
            var marker = new google.maps.Marker({
                position: pos,
                map: map,
                title: 'Marker'
            });
            marker.setMap(map);

            infoWindow.setPosition(pos);
            map.setCenter(pos);
        }, function () {
            handleLocationError(true, infoWindow, map.getCenter());
        });
    } else {
        // Browser doesn't support Geolocation
        handleLocationError(false, infoWindow, map.getCenter());
    }
}

function GetRoute() {
    //********* DIRECTIONS AND ROUTE **********************//
    $("#dvLocation").empty();
    calculateAndDisplayRoute(directionsService, directionsDisplay);
}
//This function will get the routs between two places
function calculateAndDisplayRoute(directionsService, directionsDisplay) {
    source = document.getElementById("travelfrom").value;
    destination = document.getElementById("travelto").value;
    var selectedMode = $('input[name="travelMode"]').val();

    var request = {
        origin: source,
        destination: destination,
        travelMode: google.maps.TravelMode[selectedMode]
    };
    directionsService.route(request, function (response, status) {
        if (status == google.maps.DirectionsStatus.OK) {
            directionsDisplay.setDirections(response);
        }
    });


    //********* DISTANCE AND DURATION **********************//
    var service = new google.maps.DistanceMatrixService();
    var selectedMode = $('input[name="travelMode"]').val();

    service.getDistanceMatrix({
        origins: [source],
        destinations: [destination],
        travelMode: google.maps.TravelMode[selectedMode],
        unitSystem: google.maps.UnitSystem.METRIC,
        avoidHighways: false,
        avoidTolls: false
    },
        callback
    );
}
// get distance results
function callback(response, status) {
    if (status != google.maps.DistanceMatrixStatus.OK) {
        $("#result").html(err);
    } else {
        var origin = document.getElementById("travelfrom").value;
        var destination = document.getElementById("travelto").value;
        if (response.rows[0].elements[0].status === "NOT_FOUND") {
            $("#result").html(
                "Gör en ny sökning. Det finns inga vägar mellan " +
                origin +
                " and " +
                destination
            );
        } else {
            $("#result").hide();
            var distance = response.rows[0].elements[0].distance.text;
            var origin = response.originAddresses;
            var destination = response.destinationAddresses;
            var headerDistance = document.getElementById("hdDistance");
            var dvDistance = document.getElementById("dvDistance");
            var footerDistance = document.getElementById("ftDistance");
            var headerOrigin = document.getElementById("hdOrigin");
            var dvOrigin = document.getElementById("dvOrigin");
            var headerDestination = document.getElementById("hdDestination");
            var dvDestination = document.getElementById("dvDestination");
            headerDistance.innerHTML = "";
            headerDistance.innerHTML += "<span class='origin' > <b> Avstånd: </b>";
            dvDistance.innerHTML = "";
            dvDistance.innerHTML += distance;
            footerDistance.innerHTML = "";
            footerDistance.innerHTML = "</span >";
            headerOrigin.innerHTML = "";
            headerOrigin.innerHTML = "<b>Startpunkt:</b >";
            dvOrigin.innerHTML = "";
            dvOrigin.innerHTML += origin;
            headerDestination.innerHTML = "";
            headerDestination.innerHTML = "<b>Slutpunkt:</b >";
            dvDestination.innerHTML = "";
            dvDestination.innerHTML += destination;
        }
    }
}

$(function () {
    //This make the texbox writeble
    $("#travelfrom").on('click focusin', function () {
        this.value = '';
        $(".error").remove();
    });
    $("#travelto").on('click focusin', function () {
        $(".error").remove();
    });
    //When click Get Location button the latitude and longitude will be set in the Travel From textbox
    $("#showMe").on("click", function () {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (position) {
                var geolocation = new google.maps.LatLng(
                    position.coords.latitude, position.coords.longitude);
           
                var datastring = { 'Latitude': position.coords.latitude, 'Longitude': position.coords.longitude }
                var data = [datastring.Latitude, datastring.Longitude];

                $.ajax({
                    url: 'Home/GetAdress',
                    type: 'POST',
                    async: false,
                    contentType: 'application/json; charset=UTF-8',
                    data: JSON.stringify(data),
                    success: function (data, textStatus, xhr) {
                        var obj = JSON.parse(data);
                        $("#travelfrom").val(obj["City"]);
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        console.log(XMLHttpRequest.responseText);
                    }
                });
              
                //$("#travelfrom").val(position.coords.latitude + "," + position.coords.longitude);
                showMe
            })
        }
    })

});