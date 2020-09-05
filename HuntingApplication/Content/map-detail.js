var map;
var infoWindow;
var mapAreaArray = [];
var mapItemArray = [];
var defaultLatLng = new google.maps.LatLng(24.886436490787712, -70.2685546875);
var mapEditorSettings = {
    isEditMode: false,
};

//#region UpdateMap Functions

function createMapAreaLegend(mapAreaLegend, mapArea, index)
{
    mapAreaLegend.append(
    '<div class="row">\n' +
    '   <div class="col-md-12 map-legend-item">\n' +
    '       <button class="btn btn-md btn-default map-area-focus name short-button" data-index="' + index + '">\n' +
    '           ' + mapArea.title + '\n' +
    '       </button>\n' +
    '       <input type="hidden" class="description map-area-hidden" name="description" data-index="' + index + '" value="' + mapArea.description + '">\n' +
    '       <div class="pull-right action-buttons">\n' +
    '           <button data-toggle="modal" data-target="#rename-modal" data-index="' + index + '" data-type="area" class="btn btn-md btn-default"><span class="glyphicon glyphicon-pencil"></span></button>\n' +
    '           <button class="btn btn-md btn-default map-area-remove" data-index="' + index + '"><span class="glyphicon glyphicon-trash"></span></button>\n' +
    '       </div>\n' +
    '   </div>\n' +
    '</div>\n');
}

function createMapItemLegend(mapItemLegend, mapItem, index)
{
    mapItemLegend.append(
        '<div class="row">\n' +
        '   <div class="col-md-12 map-legend-item">\n' +
        '       <button class="btn btn-md btn-default map-item-focus name short-button" data-index="' + index + '">\n' +
        '           ' + mapItem.title + '\n' +
        '       </button>\n' +
        '       <input type="hidden" class="description map-item-hidden" name="description" data-index="' + index + '" value="' + mapItem.description + '">\n' +
        '       <div class="pull-right action-buttons">\n' +
        '           <button data-toggle="modal" data-target="#rename-modal" data-index="' + index + '" data-type="item" class="btn btn-md btn-default"><span class="glyphicon glyphicon-pencil"></span></button>\n' +
        '           <button class="btn btn-md btn-default map-item-remove" data-index="' + index + '"><span class="glyphicon glyphicon-trash"></span></button>\n' +
        '       </div>\n' +
        '   </div>\n' +
        '</div>\n');
}

function createUpdateLegend() {
    var mapAreaLegend = $('.map-area-legend');
    for (var index = 0; index < mapAreaArray.length; index++) {
        var mapArea = mapAreaArray[index];
        createMapAreaLegend(mapAreaLegend, mapArea, index);
    }
    for (var index = 0; index < mapItemArray.length; index++) {
        var mapItem = mapItemArray[index];
        var itemTypeLegend = $('.map-item-legend-' + mapItem.itemtype);
        createMapItemLegend(itemTypeLegend, mapItem, index);
    }
}

function focusUpdateMapArea(current) {
    for (var index = 0; index < mapAreaArray.length; index++) {
        var mapArea = mapAreaArray[index];
        mapArea.setEditable(mapArea == current);
    }
}

function findNewName(itemArray, prefix) {
    var newName = prefix;
    var newIndex = 1;
    var exists;
    do {
        exists = false;
        for (var index = 0; index < itemArray.length; index++) {
            if (newName + newIndex === itemArray[index].title) {
                newIndex++;
                exists = true;
                break;
            }
        }
    }
    while (exists);
    return newName + newIndex;
}

function addNewMapAreaClick(event)
{
    var button = $(this);
    infoWindow.close(map);
    focusUpdateMapArea(null);

    var bounds = map.getBounds();
    var east = bounds.getNorthEast().lat();
    var west = bounds.getSouthWest().lat();
    var north = bounds.getNorthEast().lng();
    var south = bounds.getSouthWest().lng();
    var latDistance = Math.abs(east - west) / 4;
    var lngDistance = Math.abs(south - north) / 4;
    var center = new google.maps.LatLng((east + west) / 2, (north + south) / 2);

    var index = mapAreaArray.length;
    // Construct the polygon.
    var mapArea = new google.maps.Polygon({
        paths: [
            new google.maps.LatLng(center.lat() - latDistance, center.lng() - lngDistance),
            new google.maps.LatLng(center.lat() + latDistance, center.lng() - lngDistance),
            new google.maps.LatLng(center.lat() + latDistance, center.lng() + lngDistance),
            new google.maps.LatLng(center.lat() - latDistance, center.lng() + lngDistance),
        ],
        strokeColor: '#FF0000',
        strokeOpacity: 0.8,
        strokeWeight: 3,
        fillColor: '#FF0000',
        fillOpacity: 0.35,
        editable: true,
        draggable: false,
        map: map,
        title: findNewName(mapAreaArray, button.data("prefix")),
        description: '',
        dbid: null, 
        arrayindex: index,
    });
    mapAreaArray.push(mapArea);
    google.maps.event.addListener(mapArea, 'click', onMapAreaClick);
    var mapAreaLegend = $('.map-area-legend');
    createMapAreaLegend(mapAreaLegend, mapArea, index);
}

function addNewMapItemClick(event)
{
    var button = $(this);
    infoWindow.close(map);
    focusUpdateMapArea(null);

    var bounds = map.getBounds();
    var east = bounds.getNorthEast().lat();
    var west = bounds.getSouthWest().lat();
    var north = bounds.getNorthEast().lng();
    var south = bounds.getSouthWest().lng();
    var center = new google.maps.LatLng((east + west) / 2, (north + south) / 2);

    var index = mapItemArray.length;
    // Construct the polygon.
    var mapItem = new google.maps.Marker({
        position: center,
        draggable: true,
        map: map,
        title: findNewName(mapItemArray, button.data("prefix")),
        description: '',
        itemtype: button.data("type"),
        arrayindex: index,
    });

    mapItemArray.push(mapItem);
    google.maps.event.addListener(mapItem, 'click', onMapItemClick);
    var itemTypeLegend = $('.map-item-legend-' + mapItem.itemtype);
    createMapItemLegend(itemTypeLegend, mapItem, index);
}

function removeMapAreaClick(event) {
    var index = $(this).data('index');
    mapAreaArray[index].setVisible(false);
    $(this).closest('div.row').remove();
}

function removeMapItemClick(event)
{
    var index = $(this).data('index');
    mapItemArray[index].setVisible(false);
    $(this).closest('div.row').remove();
}

function createSaveObject() {
    var saveObject = {
        mapAreaList: [],
        mapItemList: [],
    };
    for (var index = 0; index < mapAreaArray.length; index++) {
        var mapArea = mapAreaArray[index];
        var saveArea = {
            id: mapArea.dbid,
            title: mapArea.title,
            description: mapArea.description,
            polygon: mapArea.getPath().getArray(),
            isDeleted: mapArea.visible == false,
        };
        saveObject.mapAreaList.push(saveArea);
    }
    for (var index = 0; index < mapItemArray.length; index++) {
        var mapItem = mapItemArray[index];
        var saveItem = {
            id: mapItem.dbid,
            itemtype: mapItem.itemtype,
            title: mapItem.title,
            description: mapItem.description,
            position: mapItem.getPosition(),
            isDeleted: mapItem.visible == false,
        };
        saveObject.mapItemList.push(saveItem);
    }
    return saveObject;
}

function updateMapItemIds(mapAreaIdArray, mapItemIdArray)
{
    if (mapAreaIdArray.length != mapAreaArray.length || mapItemIdArray.length != mapItemArray.length)
    {
        alert('save fail');
        return false;
    }
    for (var index = 0; index < mapAreaArray.length; index++) {
        mapAreaArray[index].dbid = mapAreaIdArray[index];
    }
    for (var index = 0; index < mapItemArray.length; index++) {
        mapItemArray[index].dbid = mapItemIdArray[index];
    }
    return true;
}

function updateObjectName(objectType, index, name, description)
{
    if (objectType === 'item')
    {
        mapItemArray[index].title = name;
        mapItemArray[index].description = description;
        $('.map-item-focus[data-index=' + index + ']').html(name);
        $('input.map-item-hidden[data-index=' + index + ']').val(description);
    }
    if (objectType === 'area') {
        mapAreaArray[index].title = name;
        mapAreaArray[index].description = description;
        $('.map-area-focus[data-index=' + index + ']').html(name);
        $('input.map-area-hidden[data-index=' + index + ']').val(description);
    }
}

//#endregion

function calculateBounds()
{
    var bounds = new google.maps.LatLngBounds();

    for (var index = 0; index < mapAreaArray.length; index++) {
        var vertices = mapAreaArray[index].getPath();
        for (var verticesIndex = 0; verticesIndex < vertices.getLength() ; verticesIndex++) {
            bounds.extend(vertices.getAt(verticesIndex));
        }
        google.maps.event.addListener(mapAreaArray[index], 'click', onMapAreaClick);
    }
    for (var index = 0; index < mapItemArray.length; index++) {
        bounds.extend(mapItemArray[index].getPosition());
        google.maps.event.addListener(mapItemArray[index], 'click', onMapItemClick);
    }
    if (mapAreaArray.length == 0 && mapItemArray.length == 0) {
        bounds.extend(new google.maps.LatLng(50.2239, 12.1950))
        bounds.extend(new google.maps.LatLng(49.5767, 18.7646))
    }
    return bounds;
}

function mapDetailInitialize() {
    var mapOptions = {
        zoom: 5,
        center: defaultLatLng,
    };

    map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

    // place to initialize all polygons and markers
    huntingMapDataInit(mapAreaArray, mapItemArray, mapEditorSettings);

    var bounds = calculateBounds();
    map.fitBounds(bounds);

    infoWindow = new google.maps.InfoWindow();

    onHuntingMapAfterInit(map, bounds);
    if (mapEditorSettings.isEditMode)
    {
        createUpdateLegend();
        $('.map-area-create').on("click", addNewMapAreaClick);
        $('.map-item-create').on("click", addNewMapItemClick);
        $('#map-legend').on("click", '.map-area-remove', removeMapAreaClick);
        $('#map-legend').on("click", '.map-item-remove', removeMapItemClick);
    }

    $('#map-legend').on("click", '.map-item-focus', function () {
        var index = $(this).data("index");
        var position = mapItemArray[index].getPosition();
        map.panTo(position);
        map.setZoom(16);
        if (mapEditorSettings.isEditMode) {
            focusUpdateMapArea(null);
        }
    });
    $('#map-legend').on("click", '.map-area-focus', function () {
        var index = $(this).data("index");
        var bounds = new google.maps.LatLngBounds();
        var vertices = mapAreaArray[index].getPath();
        for (var verticesIndex = 0; verticesIndex < vertices.getLength() ; verticesIndex++) {
            bounds.extend(vertices.getAt(verticesIndex));
        }
        map.fitBounds(bounds);
        if (mapEditorSettings.isEditMode) {
            focusUpdateMapArea(mapAreaArray[index]);
        }
    });
}

function onMapAreaClick(event) {
    if (mapEditorSettings.isEditMode)
    {
        focusUpdateMapArea(this);
    }
    contentString = '<b>' + this.title + '</b>';
    // Replace the info window's content and position.
    infoWindow.setContent(contentString);
    infoWindow.setPosition(event.latLng);
    infoWindow.open(map);

    var panel = $('#collapse-area');
    if (panel.hasClass('in') == false)
    {
        $('.header-map-area').trigger("click");
    }
    $('.map-area-focus[data-index=' + this.arrayindex + ']').effect("highlight", { color: 'red' }, 1000);
}

function onMapItemClick(event) {
    if (mapEditorSettings.isEditMode)
    {
        focusUpdateMapArea(null);
    }
    contentString = '<b>' + this.title + '</b>';
    // Replace the info window's content and position.
    infoWindow.setContent(contentString);
    infoWindow.setPosition(event.latLng);
    infoWindow.open(map);

    var panel = $('#collapse-' + this.itemtype);
    if (panel.hasClass('in') == false) {
        $('.header-map-item-' + this.itemtype).trigger("click");
    }
    $('.map-item-focus[data-index=' + this.arrayindex + ']').effect("highlight", { color: 'red' }, 1000);
}
