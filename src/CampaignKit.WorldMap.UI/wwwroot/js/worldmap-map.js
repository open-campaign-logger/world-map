// Copyright 2017-2019 Jochen Linnemann, Cory Gill
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// **********************************************
//                 Globals
// **********************************************
var mapId, mapUserId, mapShare, map;
var featureGroup, drawControl;

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
<div id="popup_editor_spacer_{id}" class="popup_editor_spacer"></div>\
<div id="popup_editor_{id}" class="popup_editor"><br/><br/><br/><br/><br/></div>\
</div>';

// **********************************************
//              Map Event Handlers
// **********************************************

// This is used for testing purposes only.
function loadMarkers(share) {

    var requestUrl = rootUri;
    requestUrl += `/Map/MarkerData/${mapId}`;
    if (share) {
        requestUrl += `?shareKey=${share}`;
    }

    $.ajax({
        type: 'GET',
        url: requestUrl,
        contentType: 'application/json',
        dataType: 'json',
        success: function(response) {

            const data = JSON.parse(response);

            for (let drawnitem of data) {

                let layer;

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

                // Create the popup content
                popupContent = L.Util.template(template, drawnitem.properties);

                // Create a popup
                layer.bindPopup(popupContent)
                    .on('popupclose', popupClose)
                    .on('popupopen', popupOpen);

                // Copy the properties from the saved object
                layer.properties = drawnitem.properties;

                // Add the layer and the popup to the drawn items group
                featureGroup.addLayer(layer);

            };
        },
        failure: function(response) {
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
    if (user.profile.sub !== mapUserId) {
        return;
    }

    // Create a variable to hold map data to submit
    const data = [];

    // Create an array of layer items
    for (let drawnItem of featureGroup.getLayers()) {
        data.push(drawnItemToJSON(drawnItem));
    }

    // Prepare submission data
    // Note: model properties and values must be surrounded by quotes.
    const submissionData = {
        "MapId": mapId,
        "MarkerData": JSON.stringify(data)
    };

    // Post the data back to the controller
    console.log('Submitting form...');

    var requestUrl = rootUri;
    requestUrl += '/Map/MarkerData'

    $.ajax({
        url: requestUrl,
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify(submissionData),
        beforeSend: function(xhr) {
            xhr.setRequestHeader('Authorization', `Bearer ${user.access_token}`);
        },
        success: function(result) {
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

    const feature = {
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
    const layer = event.layer;

    // Calculate feature coordinates
    var latlngs;
    if (layer instanceof L.Polygon) {
        latlngs = layer._defaultShape ? layer._defaultShape() : layer.getLatLngs();
    } else {
        latlngs = [layer.getLatLng()];
    }

    // Format the coordinates
    const latlng = latlngs[0];

    // Add a properties array to the layer object
    layer.properties = {
        title: '',
        layerType: event.layerType,
        latlng: strLatLng(latlng),
        id: idLatLng(latlng),
        content: ''
    };

    // Create the popup content
    popupContent = L.Util.template(template, layer.properties);

    // Create a popup
    layer.bindPopup(popupContent)
        .on('popupclose', popupClose)
        .on('popupopen', popupOpen);

    // Add the layer and the popup to the drawn items group
    featureGroup.addLayer(layer);

    // Save marker data
    saveMarkers();

}

// Update the coordinates in the edited drawnItem's properties
function drawnItemEdited(event) {
    const layers = event.layers;

    layers.eachLayer(function(layer) {

        // Calculate feature coordinates
        var latlngs;
        if (layer instanceof L.Polygon) {
            latlngs = layer._defaultShape ? layer._defaultShape() : layer.getLatLngs();
        } else {
            latlngs = [layer.getLatLng()];
        }

        // Format the coordinates
        const latlng = latlngs[0];

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
    
    // The quill editor introduces a toolbar (div) which is about 75px in height.
    // This throws off leafletjs which originally sized the popup to not include the toolbar.
    // When a popup is opened near the top of the viewscreen leafletjs tries to pan the map 
    // to include the popup but is now unaware that the popup is larger and part of the popup
    // is opened out of the bounds of the viewscreen. To fix this problem a temporary spacer 
    // is added when the popup is first created and in this step must now be removed.

    // Remove temporary spacer
    $(`#popup_editor_spacer_${e.target.properties.id}`).remove();


    // Instantiate the popup editor
    quill = new Quill(`#popup_editor_${e.target.properties.id}`,
        {
            theme: 'snow'
        });

    // Load the editor contents
    quill.setContents(e.target.properties.content);
        
}

// Function to call when a popup is closed
function popupClose(e) {

    // Grab form field data
    e.target.properties.title = L.DomUtil.get(`popup_title_${e.target.properties.id}`).value;

    // Save the editor contents into the
    // the geoJSONMarker's properties
    e.target.properties.content = quill.getContents();
    quill = null;

    // Save marker data
    saveMarkers();
    
}

// **********************************************
//              Utility Functions
// **********************************************

// Truncate value based on number of decimals
var _round = function(num, len) {
    return Math.round(num * (Math.pow(10, len))) / (Math.pow(10, len));
};

// Helper method to format LatLng object (x.xxxxxx, y.yyyyyy)
var strLatLng = function(latlng) {
    return `(${_round(latlng.lat, 4)}, ${_round(latlng.lng, 4)})`;
};

// Helper method to format LatLng object (x.xxxxxx, y.yyyyyy)
var idLatLng = function(latlng) {
    return strLatLng(latlng).replace(/[\s\(\)\.]/g, '').replace(/[,]/g, '_');
};

// **********************************************
//              Main Functions
// **********************************************
function initMap(pMapId, pMapUserId, pMapShare, pWorldPath, pMaxZoomLevel, pNoWrap) {

    // Populate global variables
    mapId = pMapId;
    mapUserId = pMapUserId;
    mapShare = pMapShare;

    // Clear the map if it already exists
    if (map != undefined || map != null) {
        map.off();
        map.remove();
        $('#map').html('');
        $('#preMap').empty();
        $('<div id="map"></div>').appendTo('#preMap');
    }

    // Create the map box
    map = L.map('map').setView([0, 0], 2);

    L.tileLayer(pWorldPath + '/{z}_{x}_{y}.png',
        {
            attribution: 'Campaign Logger',
            maxZoom: pMaxZoomLevel,
            noWrap: pNoWrap
        }).addTo(map);

    // Add a feature group to the map to hold drawn items
    featureGroup = L.featureGroup().addTo(map);

    // Create a draw tool bar and bind it to the feature group
    drawControl = new L.Control.Draw({
        draw: {
            position: 'topleft',
            polygon: true,
            polyline: false,
            rectangle: true,
            circle: true
        },
        edit: {
            featureGroup: featureGroup
        }
    }).addTo(map);

    // Add a listener to the map for items that are  added by a user
    map.on(L.Draw.Event.CREATED, drawnItemCreated);

    // Update all layers with any changes made by a user
    map.on(L.Draw.Event.EDITED, drawnItemEdited);

    // Update all layers with any changes made by a user
    map.on(L.Draw.Event.DELETED, drawnItemEdited);

    // Import marker data
    loadMarkers(mapShare);
}