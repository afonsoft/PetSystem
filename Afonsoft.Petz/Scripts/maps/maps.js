var mapview;
var idInfoBoxAberto;
var geocoder;
var infoBox = [];
var markers = [];
var infowindowFind;
var defaultOptions;

function initializeView(id) {
    var latlng = new google.maps.LatLng(-23.562295, -46.6364956);
    defaultOptions = {
        zoom: 8,
        center: latlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        mapTypeControl: true,
        mapTypeControlOptions: { style: google.maps.MapTypeControlStyle.HORIZONTAL_BAR, position: google.maps.ControlPosition.TOP_CENTER },
        zoomControl: true,
        zoomControlOptions: { position: google.maps.ControlPosition.LEFT_CENTER },
        scaleControl: true,
        streetViewControl: true,
        streetViewControlOptions: { position: google.maps.ControlPosition.LEFT_TOP }
    };
    infowindow = new google.maps.InfoWindow;
    geocoder = new google.maps.Geocoder();
    mapview = new google.maps.Map(document.getElementById(id), defaultOptions);
}

function abrirInfoBox(id, marker) {
    if (typeof (idInfoBoxAberto) == 'number' && typeof (infoBox[idInfoBoxAberto]) == 'object') {
        if (marker.getAnimation() !== null) {
            marker.setAnimation(null);
        }
        infoBox[idInfoBoxAberto].close();
    }

    infoBox[id].open(mapview, marker);
    idInfoBoxAberto = id;
}

function carregarPontos(pontos) {
    var latlngbounds = new google.maps.LatLngBounds();
    $.each(pontos, function (index, ponto) {
        var marker = new google.maps.Marker({ position: new google.maps.LatLng(ponto.Latitude, ponto.Longitude), title: "" + ponto.title + "", icon: "" + ponto.icon + "", draggable: true, animation: google.maps.Animation.DROP });
        var myOptions = { content: "<p><b>" + ponto.title + "</b></p><p>" + ponto.Descricao + "</p>", pixelOffset: new google.maps.Size(-150, 0) };
        infoBox[ponto.Id] = new InfoBox(myOptions);
        infoBox[ponto.Id].marker = marker;
        infoBox[ponto.Id].listener = google.maps.event.addListener(marker, 'click', function (e) { abrirInfoBox(ponto.Id, marker); });
        markers.push(marker);
        latlngbounds.extend(marker.position);
    });

    var markerCluster = new MarkerClusterer(mapview, markers, { imagePath: '/Markers/m' });
    mapview.fitBounds(latlngbounds);
    mapview.setZoom(mapview.getZoom() - 4);
}

function pesquisarPonto(address, LatId, LngId) {
    geocoder.geocode({ 'address': address }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            mapview.setCenter(results[0].geometry.location);
            mapview.setZoom(15);
            var marker = new google.maps.Marker({
                map: mapview,
                position: results[0].geometry.location,
                title: "" + address + "",
                draggable: true,
                animation: google.maps.Animation.DROP
            });
            infowindow.setContent(results[0].formatted_address);
            infowindow.open(mapview, marker);
            if (LatId != null && LatId != '')
                document.getElementById(LatId).value = results[0].geometry.location.G;
            if (LngId != null && LngId != '')
                document.getElementById(LngId).value = results[0].geometry.location.K;
        } else {
            alert("Localização não foi bem sucedida pelo seguinte motivo: " + status);
        }
    });
}

function showMapInfo(id, lat, lng, title, icon) {
    var latlng = new google.maps.LatLng(lat, lng);
    mapview.setCenter(latlng);
    mapview.setZoom(17);
    if (infoBox[id].marker == 'undefined') {
        var marker = new google.maps.Marker({
            position: new google.maps.LatLng(lat, lng),
            title: "" + title + "",
            icon: "" + icon + "",
            draggable: true,
            animation: google.maps.Animation.DROP
        });
        abrirInfoBox(id, marker);
    } else {
        abrirInfoBox(id, infoBox[id].marker);
    }
}