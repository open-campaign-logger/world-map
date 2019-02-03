// **********************************************
//                 Globals
// **********************************************
var mapId = 1;

var template = '<div id="popup_{id}" class="popup">\
<div class="popup_section">\
<label for="popup_title_{id}" class="popup_label">Title</label></br>\
<input id="popup_title_{id}" class="popup_input" type="text" value="{title}"></input>\
</div>\
<div class="popup_section">\
<label for="popup_coordinates_{id}" class="popup_label">Coordinates</label>\
<input id="popup_coordinates_{id}" class="popup_input_short" type="text" value="{latlng}" disabled></input>\
</div>\
<div class="popup_section">\
<label for="popup_editor_{id}" class="popup_label">Description</label></br>\
<div id="popup_editor_{id}" class="popup_editor"></div>\
</div>';


// **********************************************
//              Map Event Handlers
// **********************************************

// This is used for testing purposes only.
function loadMarkers(secret) {

    var requestUrl = "/Map/MarkerData/" + mapId;
    if (secret) {
        requestUrl += "?secret=" + secret;
    }

    $.ajax({
        type: "GET",
        url: requestUrl,
        contentType: "application/json",
        dataType: "json",
        success: function (response) {

            var data = JSON.parse(response);

            for (var drawnitem of data) {

                var layer;

                if (drawnitem.properties.layerType == 'polygon') {
                    layer = L.polygon(drawnitem.latlngs);
                    layer.options.color = drawnitem.options.color;
                    layer.options.weight = drawnitem.options.weight;
                    layer.options.opacity = drawnitem.options.opacity;
                } else if (drawnitem.properties.layerType == 'rectangle') {
                    layer = L.rectangle(drawnitem.latlngs);
                    layer.options.color = drawnitem.options.color;
                    layer.options.weight = drawnitem.options.weight;
                    layer.options.opacity = drawnitem.options.opacity;
                } else if (drawnitem.properties.layerType == 'circle') {
                    layer = L.circle(drawnitem.latlngs);
                    layer.options.color = drawnitem.options.color;
                    layer.options.weight = drawnitem.options.weight;
                    layer.options.opacity = drawnitem.options.opacity;
                } else if (drawnitem.properties.layerType == 'marker') {
                    layer = L.marker(drawnitem.latlngs);
                }

                // Create a popup
                layer.bindPopup('')
                    .on('popupclose', popupClose)
                    .on('popupopen', popupOpen);

                // Copy the properties from the saved object
                layer.properties = drawnitem.properties;

                // Add the layer and the popup to the drawn items group
                featureGroup.addLayer(layer);

            };
        },
        failure: function (response) {
            alert(response);
        }
    });

}

// This is used for testing purposes only.
function saveMarkers() {

    // Verify that user has logged in
    if (!isAuthenticated) {
        return;
    }

    // Ensure that the user is the map owner
    if (userId !== mapUserId) {
        return;
    }

    // Create a variable to hold map data to submit
    var data = [];

    // Create an array of layer items
    for (var drawnItem of featureGroup.getLayers()) {
        data.push(drawnItemToJSON(drawnItem));
    }

    // Prepare submission data
    // Note: model properties and values must be surrounded by quotes.
    var submissionData = {
        "MapId": mapId,
        "MarkerData": JSON.stringify(data)
    }

    // Post the data back to the controller
    console.log('Submitting form...');
    $.ajax({
        url: '/Map/MarkerData',
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(submissionData),
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
        },
        success: function (result) {
            console.log('Data received: ');
            console.log(result);
        }
    });

}

// Convert the drawnItem to JSON so that it can be persisted
function drawnItemToJSON(layer) {

    // Calculate feature coordinates
    var latlngs;
    if (layer instanceof L.Polygon) {
        latlngs = layer._defaultShape ? layer._defaultShape() : layer.getLatLngs();
    } else {
        latlngs = layer.getLatLng();
    }

    var feature = {
        "options": layer.options,
        "properties": layer.properties,
        "latlngs": latlngs
    };

    return feature;

}

// **********************************************
//        Drawn Items Event Handlers
// **********************************************

// Set the drawnItem's properties and attach a popup
function drawnItemCreated(event) {

    // Retrieve the drawing layer from the event.
    var layer = event.layer;

    // Calculate feature coordinates
    var latlngs;
    if (layer instanceof L.Polygon) {
        latlngs = layer._defaultShape ? layer._defaultShape() : layer.getLatLngs();
    } else {
        latlngs = [layer.getLatLng()];
    }

    // Format the coordinates
    var latlng = latlngs[0];

    // Add a properties array to the layer object
    layer.properties = {
        title: "",
        layerType: event.layerType,
        latlng: strLatLng(latlng),
        id: idLatLng(latlng),
        content: ''
    };

    // Create a popup
    layer.bindPopup('')
        .on('popupclose', popupClose)
        .on('popupopen', popupOpen);

    // Add the layer and the popup to the drawn items group
    featureGroup.addLayer(layer);

    // Save marker data
    saveMarkers();

}

// Update the coordinates in the edited drawnItem's properties
function drawnItemEdited(event) {
    var layers = event.layers;

    layers.eachLayer(function (layer) {

        // Calculate feature coordinates
        var latlngs;
        if (layer instanceof L.Polygon) {
            latlngs = layer._defaultShape ? layer._defaultShape() : layer.getLatLngs();
        } else {
            latlngs = [layer.getLatLng()];
        }

        // Format the coordinates
        var latlng = latlngs[0];

        // Add a properties array to the layer object
        layer.properties.latlng = strLatLng(latlng);
        layer.properties.id = idLatLng(latlng);

    });

    // Save marker data
    saveMarkers();

}

// **********************************************
//             Popup Event Handlers
// **********************************************

// Function to call when a popup is opened
function popupOpen(e) {

    // Close any other open popups

    // Calculate the popup contents
    var popupContent = L.Util.template(template, e.target.properties);
    e.popup.setContent(popupContent);

    // Instantiate the popup editor
    quill = new Quill("#popup_editor_" + e.target.properties.id, {
        theme: 'snow'
    });

    // Load the editor contents
    quill.setContents(e.target.properties.content);

}

// Function to call when a popup is closed
function popupClose(e) {

    // Grab form field data
    e.target.properties.title = L.DomUtil.get('popup_title_' + e.target.properties.id).value;

    // Save the editor contents into the
    // the geoJSONMarker's properties
    e.target.properties.content = quill.getContents();
    quill = null;

    // Save marker data
    saveMarkers();

    // Clear the popup
    e.popup.setContent('');

}


// **********************************************
//              Utility Functions
// **********************************************

// Truncate value based on number of decimals
var _round = function (num, len) {
    return Math.round(num * (Math.pow(10, len))) / (Math.pow(10, len));
};

// Helper method to format LatLng object (x.xxxxxx, y.yyyyyy)
var strLatLng = function (latlng) {
    return "(" + _round(latlng.lat, 4) + ", " + _round(latlng.lng, 4) + ")";
};

// Helper method to format LatLng object (x.xxxxxx, y.yyyyyy)
var idLatLng = function (latlng) {
    return strLatLng(latlng).replace(/[\s\(\)\.]/g, "").replace(/[,]/g, "_");
};