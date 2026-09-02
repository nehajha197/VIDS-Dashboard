window.gisMap = null;
window.dotNetHelper = null;

window.registerDotNetHelper = function (dotNetRef) {
    window.dotNetHelper = dotNetRef;
};
window.initGisMap = function (elementId, lat, lng, zoom) {
  window.gisMap = L.map(elementId).setView([lat, lng], zoom);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(window.gisMap);
};

var deviceIconPrefix = {
    "ATCC": "ATCC",
    "ECB": "ECB",
    "TMCS": "TMCS",
    "VIDES": "Vides",
    "VIDS": "Vids",
    "VMS": "VMS"
};

function getIconForDevice(deviceType, status) {
    var prefix = deviceIconPrefix[deviceType] || "VMS";
    var colorSuffix = (status === "Online") ? "Green" : "Red";
    var iconUrl = "/ATMSImg/" + prefix + "_" + colorSuffix + ".png";

    // Default size for all types
    var size = [48, 58];
    var anchor = [16, 32];

    // Per-type overrides
    if (deviceType === "ECB") {
        size = [65, 78];
        anchor = [24, 46];
    } else if (deviceType === "ATCC") {
        size = [65, 78];
        anchor = [24, 46];
    } else if (deviceType === "TMCS") {
        size = [54, 68];
        anchor = [24, 46];
    } else if (deviceType === "VIDES") {
        size = [65, 78];
        anchor = [24, 46];
    } else if (deviceType === "VMS") {
        size = [65, 80];
        anchor = [32, 80];
    }

    return L.icon({
        iconUrl: iconUrl,
        iconSize: size,
        iconAnchor: anchor,
        popupAnchor: [0, -32]
    });
}

// ---- Builds the inner "card" content for ONE device (reused for both single & multi-device popups) ----
        function buildDeviceCardInner(dev, uid) {
            var isOnline = dev.status === "Online";
            var accentColor = isOnline ? "#10b981" : "#ef4444";

            // CHANGED: Watch Live Feed button now shows ONLY for TMCS devices
            var showWatchLive = dev.deviceType === "TMCS";

            return (
        "<div style='background:linear-gradient(135deg,#1e3a8a,#2563eb); padding:12px 14px; border-radius:8px 8px 0 0;'>" +
        "<div style='display:flex; justify-content:space-between; align-items:center;'>" +
        "<div>" +
        "<div style='color:#c7d2fe; font-size:10px; font-weight:600; letter-spacing:0.5px;'>" + dev.deviceType + "</div>" +
        "<div style='color:white; font-size:16px; font-weight:700;'>" + dev.name + "</div>" +
        "</div>" +
        "<div style='display:flex; align-items:center; gap:5px;'>" +
        "<span style='width:8px; height:8px; border-radius:50%; background:" + accentColor + "; animation:pulse_" + uid + " 1.5s infinite;'></span>" +
        "<span style='color:white; font-size:11px; font-weight:600;'>" + dev.status + "</span>" +
        "</div>" +
        "</div>" +
        "</div>" +

        //"<div style='display:flex; background:#f8fafc; border-bottom:1px solid #e5e7eb;'>" +
        //"<button onclick=\"navigator.clipboard.writeText('" + dev.deviceEndpoint + "'); this.innerText='Copied!'; setTimeout(()=>this.innerText='Copy IP',1200);\" " +
        //"style='flex:1; border:none; background:none; padding:8px 4px; font-size:11px; color:#374151; cursor:pointer; border-right:1px solid #e5e7eb;'>Copy IP</button>" +
        //"<button onclick=\"window.gisMap.setView([" + dev.latitude + "," + dev.longitude + "], 18);\" " +
        //"style='flex:1; border:none; background:none; padding:8px 4px; font-size:11px; color:#374151; cursor:pointer; border-right:1px solid #e5e7eb;'>Zoom In</button>" +
        //"<button onclick=\"var d=document.getElementById('details_" + uid + "'); d.style.display = d.style.display==='none' ? 'block' : 'none';\" " +
        //"style='flex:1; border:none; background:none; padding:8px 4px; font-size:11px; color:#374151; cursor:pointer;'>Details</button>" +
        //"</div>" +

        "<div style='padding:10px 14px; font-size:12px; color:#374151;'>" +
        "<div style='display:flex; justify-content:space-between; margin-bottom:4px;'>" +
        "<span style='color:#9ca3af;'>IP</span><span style='font-weight:600;'>" + (dev.deviceEndpoint || "N/A") + "</span>" +
        "</div>" +

        "<div style='display:flex; justify-content:space-between; margin-bottom:4px;'>" +  
        "<span style='color:#9ca3af;'>Direction</span><span style='font-weight:600;'>" + (dev.direction || "N/A") + "</span>" +
        "</div>" +

        "<div style='display:flex; justify-content:space-between;'>" +   
        "<span style='color:#9ca3af;'>Location</span><span style='font-weight:600;'>" + dev.latitude.toFixed(6) + ", " + dev.longitude.toFixed(6) + "</span>" +
        "</div>" +
        "</div>" +   

                // Only rendered when showWatchLive is true (i.e., deviceType === "TMCS")
                (showWatchLive ?
        "<div style='padding:0 14px 14px;'>" +
        "<button onclick=\"window.watchLiveVideo && window.watchLiveVideo('" + dev.deviceEndpoint + "', '" + dev.name + "')\" " +
        "onmouseover=\"this.style.transform='translateY(-2px)'; this.style.boxShadow='0 4px 10px rgba(37,99,235,0.4)';\" " +
        "onmouseout=\"this.style.transform='translateY(0)'; this.style.boxShadow='none';\" " +
        "style='width:100%; background:#2563eb; color:white; border:none; border-radius:7px; padding:9px; " +
        "font-size:12px; font-weight:700; cursor:pointer; transition:all 0.15s ease; display:flex; align-items:center; justify-content:center; gap:6px;'>" +
        "▶ Watch Live Feed" +
        "</button>" +
        "</div>" 
                    : "<div style='padding-bottom:10px;'></div>"
                ) +

        "<style>@keyframes pulse_" + uid + " { 0% { box-shadow: 0 0 0 0 " + accentColor + "88; } 70% { box-shadow: 0 0 0 6px " + accentColor + "00; } 100% { box-shadow: 0 0 0 0 " + accentColor + "00; } }</style>"
    );
}

// ---- Builds the full popup for a location: single card, OR tab bar + multiple cards if devices overlap ----
function buildLocationPopupHtml(group, groupId) {
    var outerOpen = "<div style='width:270px; font-family:Segoe UI, Arial, sans-serif; border-radius:10px; overflow:hidden; box-shadow:0 2px 8px rgba(0,0,0,0.15);'>";
    var outerClose = "</div>";

    if (group.length === 1) {
        var uid = groupId + "_0";
        return outerOpen + buildDeviceCardInner(group[0], uid) + outerClose;
    }

    // Multiple devices at same location: tab bar + switchable cards
    var tabsHtml = "<div style='display:flex; background:#1e3a8a; padding:6px 6px 0;'>";
    var cardsHtml = "";

    group.forEach(function (dev, i) {
        var uid = groupId + "_" + i;
        var isActive = i === 0;

        tabsHtml +=
            "<button id='tab_" + uid + "' onclick=\"" +
            "document.querySelectorAll('.tabbtn_" + groupId + "').forEach(b=>{b.style.background='transparent'; b.style.color='#c7d2fe';});" +
            "document.querySelectorAll('.tabcard_" + groupId + "').forEach(c=>{c.style.display='none';});" +
            "this.style.background='white'; this.style.color='#1e3a8a';" +
            "document.getElementById('card_" + uid + "').style.display='block';" +
            "\" class='tabbtn_" + groupId + "' style='flex:1; border:none; border-radius:6px 6px 0 0; padding:7px 4px; font-size:11px; font-weight:700; cursor:pointer; " +
            "background:" + (isActive ? "white" : "transparent") + "; color:" + (isActive ? "#1e3a8a" : "#c7d2fe") + ";'>" +
            dev._label +
            "</button>";

        cardsHtml +=
            "<div id='card_" + uid + "' class='tabcard_" + groupId + "' style='display:" + (isActive ? "block" : "none") + ";'>" +
            buildDeviceCardInner(dev, uid) +
            "</div>";
    });

    tabsHtml += "</div>";

    var noticeHtml =
        "<div style='background:#eef2ff; color:#1e3a8a; font-size:10px; text-align:center; padding:4px; font-weight:600;'>" +
        group.length + " devices at this location" +
        "</div>";

    return outerOpen + noticeHtml + tabsHtml + cardsHtml + outerClose;
}

window.addCameraMarkers = function (devices) {
    // Assign a per-device-type sequential label: "VIDS 1", "VIDS 2", "ATCC 1", ...
    var typeCounters = {};
    devices.forEach(function (dev) {
        typeCounters[dev.deviceType] = (typeCounters[dev.deviceType] || 0) + 1;
        dev._label = dev.deviceType + " " + typeCounters[dev.deviceType];
    });

    // Group devices sharing the same (rounded) coordinates
    var locGroups = {};
    devices.forEach(function (dev) {
        var key = dev.latitude.toFixed(5) + "," + dev.longitude.toFixed(5);
        (locGroups[key] = locGroups[key] || []).push(dev);
    });

    var groupIndex = 0;
    Object.keys(locGroups).forEach(function (key) {
        var group = locGroups[key];
        var primary = group[0];
        var icon = getIconForDevice(primary.deviceType, primary.status);
        var marker = L.marker([primary.latitude, primary.longitude], { icon: icon }).addTo(window.gisMap);
        window.mapMarkers.push(marker);
        var groupId = "grp" + groupIndex + "_" + Math.floor(Math.random() * 10000);
        groupIndex++;

        var popupHtml = buildLocationPopupHtml(group, groupId);
        marker.bindPopup(popupHtml, { maxWidth: 300, minWidth: 270 });
    });
};
// ADDED: registers a click listener on the map that sends lat/lng to Blazor
window.enableMapClickToAdd = function () {
    window.gisMap.on('click', function (e) {
        if (window.dotNetHelper) {
            window.dotNetHelper.invokeMethodAsync('OnMapClicked', e.latlng.lat, e.latlng.lng);
        }
    });
};

// ADDED: clears all existing markers and re-adds fresh ones (used after inserting a new device)
window.mapMarkers = [];

window.clearAndReloadMarkers = function (devices) {
    window.mapMarkers.forEach(function (m) { window.gisMap.removeLayer(m); });
    window.mapMarkers = [];
    window.addCameraMarkers(devices);
};
window.watchLiveVideo = function (deviceEndpoint, deviceName) {
    if (window.dotNetHelper) {
        window.dotNetHelper.invokeMethodAsync('OpenLiveVideo', deviceEndpoint, deviceName);
    } else {
        console.error('dotNetHelper not registered yet');
    }
};