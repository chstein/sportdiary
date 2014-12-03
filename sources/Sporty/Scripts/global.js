function secondsToHms(d) {
    d = Number(d);
    var h = Math.floor(d / 3600);
    var m = Math.floor(d % 3600 / 60);
    var s = Math.floor(d % 3600 % 60);
    return ((h > 0 ? h + ":" : "") + (m > 0 ? (h > 0 && m < 10 ? "0" : "") + m + ":" : "0:") + (s < 10 ? "0" : "") + s);
}

//Functions for modal
function displayError(message, status) {
    var $dialog = $(message);
    $dialog
        .dialog({
            modal: true,
            title: status + ' Error',
            buttons: {
                Ok: function () {
                    $(this).dialog("close");
                }
            }
        });
    return false;
};


//function showDetails(tag, event, target) {
//    $(".Exercise").draggable("disable");
//    $(".dayContainer").droppable("disable");
//    event.preventDefault();
//    var $url = $(tag).attr('href');
//    if ($url === undefined) $url = "Create/";
//    var $title = $(tag).attr('title');
//    var $dialog = $('<div></div>');
//    var $classname = $(tag).attr("class");

//    //check if create / new button was clicked
//    if ($classname == "aCreate") {
//        var clickedFromCalendar = $(tag).prev().html();
//        if (clickedFromCalendar !== undefined) {
//            $("#currentDay").val(clickedFromCalendar);
//        }
//    }
//    //check if addbutton in every daycontainer was clicked
//    if ($classname.indexOf("dayContainer") > -1) {
//        var day = $(tag).find(".day").html();
//        if (day !== undefined) {
//            $("#currentDay").val(day);
//        }
//    }
//    $dialog.empty();
//    setTimeout(function () { $dialog.dialog('open'); }, 100);
//    $dialog
//        .load($url)
//        .dialog({
//            autoOpen: true,
//            title: $title,
//            width: 570,//'auto',
//            modal: true,
//            minHeight: 200,
//            show: 'fade',
//            hide: 'fade',
//            position: [100, 10],
//            close: function () {
//                $(this).empty();
//                $(".Exercise").draggable("enable");
//                $(".dayContainer").droppable("enable");
//            }
//            //, open: function(){$(".dialog-form").appendTo("#DetailsForm");}
//        });
//    $dialog.dialog("option", "buttons", {
//        "Save": function () {
//            var dlg = $(this);
//            var $frm = $("#DetailsForm");
//            $.saveDetails(dlg, $frm);


//        },
//        "Cancel": function () {
//            $("#ui-datepicker-div").hide();
//            $(this).dialog("close");
//        }
//    });
//}

//facebook
function loadFacebook(d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/en_US/all.js#xfbml=1";
    fjs.parentNode.insertBefore(js, fjs)


}

$(document).ready(function () {
    Globalize.culture('de-DE');

    //Tell the validator, for example,
    // that we want numbers parsed a certain way!
    $.validator.methods.number = function (value, element) {
        console.log("number");
        if (Globalize.parseFloat(value)) {
            return true;
        }
        return false;
    };

    $.validator.methods.min = function (value, element, param) {
        console.log("min");
        if (value) {
            value = Globalize.parseFloat(value);
            return value >= param;
        }
        return true;
    };

    //Fix the range to use globalized methods
    $.extend($.validator.methods, {
        range: function (value, element, param) {
            //Use the Globalization plugin to parse the value
            var val = Globalize.parseFloat(value);
            return this.optional(element) || (val >= param[0] && val <= param[1]);
        }
    });

    $.validator.addMethod(
        "regex",
        function (value, element, regexp) {
            var re = new RegExp(regexp);
            return this.optional(element) || re.test(value);
        },
        "Please check your input."
);

    $(document).ajaxStart(function () {
        $("#loadingInfo").show();
    });

    $(document).ajaxStop(function () {
        $("#loadingInfo").hide();
    });

    function updateTips(t) {
        tips.text(t).effect("highlight", {}, 1500);
    }

    var url = location.href;
    if (url.indexOf("PlanCalendar") > 0) {
        $("#NavigationPlanCalendar").addClass("current");
    } else if (url.indexOf("Calendar") > 0) {
        $("#NavigationCalendar").addClass("current");
    } else if (url.indexOf("Exercise") > 0) {
        $("#NavigationExercise").addClass("current");
    } else if (url.indexOf("Report") > 0) {
        $("#NavigationReport").addClass("current");
    } else if (url.indexOf("Goal") > 0) {
        $("#NavigationGoal").addClass("current");
    } else if (url.indexOf("Plan") > 0) {
        $("#NavigationPlan").addClass("current");
    } else {
        $("#NavigationHome").addClass("current");
    }


});

$.loadExerciseTableView = function () {
    $("#tableview").load("/Exercise/ExerciseTableView/",
        function (html) {
            $('#tableview')[0].value = html;
        });
};

function confirmDelete(message, callback) {
    var $deleteDialog = $('<div>M&ouml;chtest du den Eintrag l&ouml;schen ' + message + '?</div>');

    $deleteDialog
        .dialog({
            resizable: false,
            height: 140,
            title: "Eintrag l&ouml;schen?",
            modal: true,
            buttons: {
                "Delete": function () {
                    $(this).dialog("close");
                    callback.apply();
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            }
        });
}

;

function deleteRow($btn, callback) {
    $.ajax({
        url: $btn.attr('href'),
        type: 'POST',
        success: function (response) {
            if (callback) {
                callback.apply();
            }
            showInfoMessage(response);
        },
        error: function (xhr) {
            displayError(xhr.responseText, xhr.status); /* display errors in separate dialog */
        }
    });
    return false;
}

;

function showInfoMessage(message) {
    $("#ajaxResult").hide().html(message).fadeIn(300, function () {
        var e = this;
        setTimeout(function () { $(e).fadeOut(800); }, 2500);
    });
}

function deleteAttachment($btn) {
    $.ajax({
        url: $btn.attr('href'),
        success: function (response) {
            $btn.parent().fadeOut(400);
        },
        error: function (xhr) {
            displayError(xhr.responseText, xhr.status); /* display errors in separate dialog */
        }
    });
    return false;
}

;

function updateFavorite($btn) {
    $.ajax({
        url: $btn.attr('href'),
        success: function (response) {
            //remove element from list
            var planElement = $btn.closest(".Plan");
            console.log(planElement);
            console.log(planElement.length > 0);
            if (planElement.length > 0) {
                planElement.tooltip('hide');
                planElement.remove();
            }
            else {
                //isFavorite element
                $btn.parent().parent().children(".isFavorite").toggle();
                //reload fav bar
                $("#planview").load("/Plan/Favorites/",
                function (html) {
                    $('#planview')[0].value = html;
                });
            }
        },
        error: function (xhr) {
            displayError(xhr.responseText, xhr.status); /* display errors in separate dialog */
        }
        //type: 'POST',
    });
    return false;
}

;

// Validations
if ($.validator != null) {
    $.validator.addMethod("DEDate", function (value, element) {
        return testDatumsform(value);
    }, "*");
    $.validator.addMethod("time", function (value, element) {
        return testTimeform(value);
    }, "*");
}

function testTimeform(value) {
    var re = new RegExp("^([0-1][0-9]|[2][0-3]):([0-5][0-9])$");
    return value.match(re);
}

function testDatumsform(element) {
    var re = new RegExp("^([0-9]{2,2}[.]{1,1}){2,2}[0-9]{4,4}$");
    if (element.match(re)) {
        return testDatumlogisch(element);
    } else {
        return false;
    }
}


function testDatumlogisch(element) {
    var myDate = element.split(".");
    var tag = myDate[0];
    var monat = myDate[1];
    var jahr = myDate[2];
    var datum = tag + "." + monat + "." + jahr;

    var days = new Array("31", "28", "31", "30", "31", "30", "31", "31", "30", "31", "30", "31");
    if (isSchaltjahr(jahr) == "1") {
        days[1] = "29";
    }

    if (tag > 0 && tag < 32 &&
        monat > 0 && monat < 13 &&
            jahr > 2004 && jahr < 2021) {

        if (tag <= days[monat - 1]) {
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
}


function isSchaltjahr(jahr) {
    var eins = new Date(jahr, 1, 28).getTime();
    var zwei = new Date(jahr, 2, 1).getTime();
    var tage = (zwei - eins) / 86400000;
    var erg = "1";

    if (tage == 1) {
        erg = "0";
        return erg;
    } else {
        return erg;
    }
}

function InputNotNullValidation($input) {
    if ($input.val() == "") {
        $input.addClass("input-validation-error");
        return false;
    } else {
        $input.removeClass("input-validation-error");
        return true;
    }
}

;

function InputMustValidDate($input) {
    if (!ValidateDate(".", $input.val())) {
        $input.addClass("input-validation-error");
        return false;
    } else {
        $input.removeClass("input-validation-error");
        return true;
    }
}

function InputNullOrValidIntegerValue($input) {
    if ($input.val() == "" || IsValidNumber($input.val())) {
        $input.removeClass("input-validation-error");
        return true;
    } else {
        $input.addClass("input-validation-error");
        return false;
    }
}

function InputNullOrValidDecimalValue($input) {
    if ($input.val() == "" || IsValidDecimal($input.val())) {
        $input.removeClass("input-validation-error");
        return true;
    } else {
        $input.addClass("input-validation-error");
        return false;

    }
}

// Validation Helper

function ValidateDate(separator, date) {
    var date = explode(separator, date);
    var isValid = checkDate(date);
    return isValid;
}

function IsValidNumber(value) {
    var isValid = !isNaN(value);
    return isValid;
}

function IsValidDecimal(value) {
    var val = value.replace(".", "");
    var isValid = IsValidNumber(val);
    return isValid;
}

function explode(separator, date) {
    //mm/dd/yyyy
    var sepDate = date.split(separator);
    if (sepDate.length != 3) return false;

    var testDate = date.replace(separator, "");

    if (!isNaN(testDate)) return false;
    var dateObj = {
        Year: parseInt(sepDate[2], 10),
        //parseInt mit der Zahlenbasis 10 auch wenn z.B: 08 -> 8, Ohne 10 würde 0 rauskommen (Oktalsystem)
        Month: parseInt(sepDate[0], 10),
        Day: parseInt(sepDate[1], 10)
    };
    return dateObj;
}

function checkDate(date) {
    var isValid = false;
    isValid = (date.Year >= 1900 && date.Year <= 3000);
    isValid = isValid && (date.Month >= 1 && date.Month <= 12);
    isValid = isValid && (date.Day >= 1 && date.Day <= 31);
    return isValid;
}

// Errorhandler Elmah

// FROM: http://helephant.com/2007/05/diy-javascript-stack-trace

function logError(ex, stack) {
    if (ex == null) return;
    if (logErrorUrl == null) {
        alert('logErrorUrl must be defined.');
        return;
    }

    var url = ex.fileName != null ? ex.fileName : document.location;
    if (stack == null && ex.stack != null) stack = ex.stack;

    // format output
    var out = ex.message != null ? ex.name + ": " + ex.message : ex;
    out += ": at document path '" + url + "'.";
    if (stack != null) out += "\n  at " + stack.join("\n  at ");

    // send error message
    $.ajax({
        type: 'POST',
        url: logErrorUrl,
        data: { message: out }
    });
}

Function.prototype.trace = function () {
    var trace = [];
    var current = this;
    while (current) {
        trace.push(current.signature());
        current = current.caller;
    }
    return trace;
};
Function.prototype.signature = function () {
    var signature = {
        name: this.getName(),
        params: [],
        toString: function () {
            var params = this.params.length > 0 ?
                "'" + this.params.join("', '") + "'" : "";
            return this.name + "(" + params + ")";
        }
    };
    if (this.arguments) {
        for (var x = 0; x < this.arguments.length; x++)
            signature.params.push(this.arguments[x]);
    }
    return signature;
};
Function.prototype.getName = function () {
    if (this.name)
        return this.name;
    var definition = this.toString().split("\n")[0];
    var exp = /^function ([^\s(]+).+/;
    if (exp.test(definition))
        return definition.split("\n")[0].replace(exp, "$1") || "anonymous";
    return "anonymous";
};
window.onerror = function (msg, url, line) {
    logError(msg, arguments.callee.trace());
};

// calculations

$.calculateSpeedInKmh = function (timeInMin, distanceInKm) {
    var timeInHour = timeInMin / 60;
    return distanceInKm / timeInHour;
};

$.calculateSpeedInMinKm = function (timeInMin, distanceInKm) {
    return timeInMin / distanceInKm;
};

$.convertNumberToTime = function (timeInMin) {
    //4.5 => 4:30
    var minutes = Math.floor(timeInMin);
    var sec = ((timeInMin % 1) * 60).toFixed(0);
    if (sec == 60) {
        minutes++;
        sec = 0;
    }
    if (sec < 10) {
        sec = "0" + sec;
    }

    return minutes + ":" + sec;
};
$.parseTimeToMin = function (timeToParse) {
    //00:00:00 or 0:00:00
    //00:00 or 0:00
    // 00 (min)
    var time = 0;
    var pattern = /^(([0-9]?[0-9]):)?([0-5]?[0-9]):([0-5][0-9])$/;
    var match = pattern.exec(timeToParse);
    if (match != null && match.length == 5) {
        var hour = 0;
        if (match[1] != null) {
            hour = parseInt(match[1]);
        }
        var min = parseInt(match[3]);
        var sec = parseInt(match[4]);
        time = (hour * 60) + min + (sec / 60);
    } else {
        time = timeToParse;
    }
    return time;

};

$.calculateAndSetSpeedAndPace = function () {
    var durationVal = $("#Duration").val();
    var distanceVal = Globalize.parseFloat($("#Distance").val());
    if (distanceVal > 0 && (durationVal > 0 || durationVal != "")) {
        timeInMin = $.parseTimeToMin(durationVal);
        $("#Speed").val($.calculateSpeedInKmh(timeInMin, distanceVal).toFixed(2));
        $("#Pace").html($.convertNumberToTime($.calculateSpeedInMinKm(timeInMin, distanceVal).toFixed(2)) + " <small>[min/km]</small>");
    }
};


//function getVars(encodedVarLikeUrl) {
//    var vars = [], hash;
//    var hashes = encodedVarLikeUrl.slice(encodedVarLikeUrl.indexOf('?') + 1).split('&');
//    for (var i = 0; i < hashes.length; i++) {
//        hash = hashes[i].split('=');
//        vars.push(hash[0]);
//        vars[hash[0]] = hash[1];
//    }
//    return vars;
//}

//is called from details dialog (global.js)
//var exerciseDataLinkWithId;

function initializeMaps(exerciseDataLinkWithId) {

    $.ajax({
        url: exerciseDataLinkWithId,
        type: 'GET',
        success: function (data) {
            if (data["MapPoints"].length > 0) {
                $(".routeDialog").show();
                CreateMap(data);
            } else {
                $(".routeDialog").hide();
            }

            if (data["ChartSeries"].length > 0) {
                $("#chart").show();
                CreateChart(data);
            } else {
                $(".chartDialog").hide();
            }
            if (data["LapData"].length > 0) {
                $(".lapDataDialog").show();
                CreateLapTable(data);
            } else {
                $(".lapDataDialog").hide();
            }
            if (data.MapPoints.length > 0 && data.MapPoints[0]["Latitude"].length > 0 && data.MapPoints[0]["Longitude"].length > 0) {
                CreateWeather(data.MapPoints[0].Latitude, data.MapPoints[0].Longitude);
            }
        },
        error: function (xhr) {
            displayError(xhr.responseText);
        }
    });
}

function CreateLapTable(data) {
    var laps = data.LapData;
    //spalten hinzufügen die Werte enthalten
    for (var i = 0; i < laps.length; i++) {
        var rows = "<td> Runde " + i + "</td>" +
            "<td>" + (laps[i].DistanceMeters / 1000).toFixed(2) + " km</td>" +
            "<td>" + $.convertNumberToTime(laps[i].TotalTimeSeconds / 60) + "</td>" +
            "<td>" + $.convertNumberToTime($.calculateSpeedInMinKm(laps[i].TotalTimeSeconds / 60, laps[i].DistanceMeters / 1000).toFixed(2)) + " /km</td>" +
            "<td>" + laps[i].AverageHeartRateBpm + " bpm</td>" +
            "<td>" + laps[i].MaximumHeartRateBpm + " bpm</td>";
        $("<tr>" + rows + "</tr>").appendTo("#lapTable tbody");
    }
}

function CreateMap(data) {
    var map = new L.Map('map_canvas',
        { center: new L.LatLng(data.MapPoints[0].Latitude, data.MapPoints[0].Longitude) });

    var osmTile = "http://tile.openstreetmap.org/{z}/{x}/{y}.png";
    var osmCopyright = "Map data &copy; 2012 OpenStreetMap contributors";
    var osmLayer = new L.TileLayer(osmTile, { maxZoom: 18, attribution: osmCopyright });

    map.addLayer(osmLayer);

    var ggl = new L.Google();
    var ggl2 = new L.Google('ROADMAP');

    map.addControl(new L.Control.Layers({ 'OSM': osmLayer, 'Google': ggl, 'Google Road': ggl2 }, {}));
    // Define initial parameters:

    // location over which we will initially center the map
    //var loc = { 'lat': 40.754531, 'lon': -73.993113 },
    //    // extent to which we are initially zoommed
    //    zoomLevel = 3,
    //    // the maximum level of detail a user is allowed to see
    //    maxZoom = 15,
    //    // id of the element in which we will place the map
    //    mapID = 'map_canvas';

    //// Create the map object, setting the initial view:
    //var map = L.map('map_canvas').setView(
    //    [loc.lat, loc.lon],
    //    zoomLevel
    //);

    //// Instantiate a tile layer, directing Leaflet to use the Open Street Map (OSM) API to access map tiles:
    //L.tileLayer('http://{s}.tile.osm.org/{z}/{x}/{y}.png', {
    //    'maxZoom': maxZoom,
    //    'attribution': 'Map tiles by <a href="http://stamen.com">Stamen Design</a>, under <a href="http://creativecommons.org/licenses/by/3.0">CC BY 3.0</a>. Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a>, under <a href="http://creativecommons.org/licenses/by-sa/3.0">CC BY SA</a>'
    //}).addTo(map);

    //L.tileLayer('http://{s}.tile.cloudmade.com/26fb5288d4fc41579a654c348faa9e00/997/256/{z}/{x}/{y}.png', {
    //    attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://cloudmade.com">CloudMade</a>',
    //    maxZoom: 18
    //}).addTo(map);

    var routeLatLngs = [];
    for (var i = 0; i < data.MapPoints.length; i++) {
        var p = data.MapPoints[i];
        routeLatLngs.push(new L.LatLng(p.Latitude, p.Longitude));
    }
    var polyline = L.polyline(routeLatLngs, { color: 'red' }).addTo(map);


    // zoom the map to the polyline
    map.fitBounds(polyline.getBounds());
    //var myLatLng = new google.maps.LatLng(data.MapPoints[0].Latitude, data.MapPoints[0].Longitude);
    //var myOptions = {
    //    zoom: 14,
    //    center: myLatLng,
    //    mapTypeId: google.maps.MapTypeId.ROADMAP
    //};

    //var map = new google.maps.Map(document.getElementById("map_canvas"),
    //    myOptions);


    //var routeLatLngs = [];
    //for (var i = 0; i < data.MapPoints.length; i++) {
    //    p = data.MapPoints[i];
    //    routeLatLngs.push(new google.maps.LatLng(p.Latitude, p.Longitude));
    //    //console.log("lat:" + p.Latitude + "; lon: " + p.Longitude);
    //}

    //routePolyline = new google.maps.Polyline({
    //    path: routeLatLngs,
    //    strokeColor: "#0000FF",
    //    strokeweight: 4
    //});
    //    routePolyline.setMap(map);
}
function loadMapsScript(exerciseDataLink) {
    //exerciseDataLinkWithId = exerciseDataLink;

    initializeMaps(exerciseDataLink);

    //var script = document.createElement("script");
    //script.type = "text/javascript";
    //script.src = "http://maps.google.com/maps/api/js?sensor=false&callback=initializeMaps";
    //document.body.appendChild(script);
}
function CreateChart(data) {
    var chart;
    var highchartsOptions = {};

    highchartsOptions.chart = {
        renderTo: "chart",
        //margin: [0, 20, 30, 20],
        zoomType: "x",
        alignTicks: false
    };
    highchartsOptions.toolbar =
        {


            //    		        itemStyle: {
            //    		            margin: "0px 0px 0px 100px"
            //    		        }
        };
    highchartsOptions.title = { text: "" };
    highchartsOptions.credits = { enabled: false };
    highchartsOptions.legend = {
        borderWidth: 0,
        //                style: {
        //                    left: "auto",
        //                    right: "80px",
        //                    top: "-15px",
        //                    bottom: "auto"
        //                },
        itemHiddenStyle: {
            color: "#888"
        }
    };
    highchartsOptions.series = [];
    var seriesIndex = 0;
    for (var seriesId in data.ChartSeries) {
        if (data.ChartSeries[seriesId]["Type"] == "SPEED") {
            //var series = data.ChartSeries[seriesId];
            //chartDataSeriesArray[seriesIndex] = seriesToAdd;
            highchartsOptions.series[seriesIndex++] = {
                //id: 1,
                name: data.ChartSeries[seriesId].Type,
                data: data.ChartSeries[seriesId]["Points"],
                type: "line",
                color: "#FF3366",
                fillOpacity: 0.3,
                yAxis: 0,
                visible: false
            };
        } else if (data.ChartSeries[seriesId]["Type"] == "PACE") {
            //var series = data.ChartSeries[seriesId];
            //chartDataSeriesArray[seriesIndex] = seriesToAdd;
            highchartsOptions.series[seriesIndex++] = {
                //id: 1,
                name: data.ChartSeries[seriesId].Type,
                data: data.ChartSeries[seriesId]["Points"],
                type: "line",
                color: "#002EB8",
                fillOpacity: 0.25,
                yAxis: 1,
                visible: true
            };
        } else if (data.ChartSeries[seriesId]["Type"] == "HEARTRATE") {
            //var series = data.ChartSeries[seriesId];
            //chartDataSeriesArray[seriesIndex] = seriesToAdd;
            highchartsOptions.series[seriesIndex++] = {
                //id: 1,
                name: data.ChartSeries[seriesId].Type,
                data: data.ChartSeries[seriesId]["Points"],
                type: "line",
                color: "#FFCC33",
                fillOpacity: 0.2,
                yAxis: 2,
                visible: true
            };
        } else if (data.ChartSeries[seriesId]["Type"] == "ELEVATION") {
            //var series = data.ChartSeries[seriesId];
            //chartDataSeriesArray[seriesIndex] = seriesToAdd;
            highchartsOptions.series[seriesIndex++] = {
                //id: 1,
                name: data.ChartSeries[seriesId].Type,
                data: data.ChartSeries[seriesId]["Points"],
                type: "line",
                color: "#66FF33",
                yAxis: 3,
                visible: true
            };
        }
        else if (data.ChartSeries[seriesId]["Type"] == "CADENCE") {
            //var series = data.ChartSeries[seriesId];
            //chartDataSeriesArray[seriesIndex] = seriesToAdd;
            highchartsOptions.series[seriesIndex++] = {
                //id: 1,
                name: data.ChartSeries[seriesId].Type,
                data: data.ChartSeries[seriesId]["Points"],
                type: "line",
                color: "#FF9900",
                yAxis: 4,
                visible: true
            };
        }
    }

    highchartsOptions.xAxis =
        [{
            title: {
                enabled: false
            },
            labels: {
                formatter: function () { return (this.value == 0) ? this.value : this.value + " km"; }
            },
            lineColor: "#99A9B9"
        },
            {
                //HEARTRATE
                title: {
                    enabled: false
                },
                labels: {
                    formatter: function () { return (this.value == 0) ? this.value : this.value + " Min"; }
                },
                lineColor: "#99A9B9"
            }];
    highchartsOptions.yAxis =
        [
            {
                // SPEED
                id: "SPEED",
                gridLineWidth: 0,
                title: {
                    enabled: true,
                    text: "Speed",
                    style: {
                        color: '#FF3366'
                    }
                },
                labels: {
                    enabled: true,
                    style: {
                        color: '#FF3366'
                    }
                },
                //startOnTick: false,
                //endOnTick: false,
                style: {
                    color: '#FF3366'
                }
            }, {
                // PACE
                id: "PACE",
                gridLineWidth: 0,
                title: {
                    enabled: true,
                    text: "Pace",
                    style: {
                        color: '#002EB8'
                    }
                },
                labels: {
                    enabled: true,
                    style: {
                        color: '#002EB8'
                    }
                },
                maxPadding: 0.15,
                style: {
                    color: '#002EB8'
                }
            },

        {
            // HEARTRATE
            id: "HEARTRATE",
            gridLineWidth: 1,
            title: {
                enabled: true,
                text: "Heartrate",
                style: {
                    color: '#FFCC33'
                }
            },
            labels: {
                enabled: true,
                style: {
                    color: '#FFCC33'
                }
            },
            //startOnTick: false,
            //endOnTick: false,
            style: {
                color: '#FFCC33'
            },
            opposite: true
        }, {
            // ELEVATION
            id: "ELEVATION",
            gridLineWidth: 0,
            title: {
                enabled: true,
                text: "Elevation",
                style: {
                    color: '#66FF33'
                }
            },
            labels: {
                enabled: true,
                style: {
                    color: '#66FF33'
                }
            },
            //startOnTick: false,
            //endOnTick: false,
            style: {
                color: '#66FF33'
            },
            opposite: true
            //max: (chartData.series["ELEVATION"]) ? chartData.series["ELEVATION"].maxValY + ((chartData.series["ELEVATION"].maxValY - chartData.series["ELEVATION"].minValY) * 0.15) : null,
            //min: (chartData.series["ELEVATION"]) ? chartData.series["ELEVATION"].minValY : null
        }, {
            // CADENCE
            id: "CADENCE",
            gridLineWidth: 0,
            title: {
                enabled: true,
                text: "Cadence",
                style: {
                    color: '#FF9900'
                }
            },
            labels: {
                enabled: true,
                style: {
                    color: '#FF9900'
                }
            },
            style: {
                color: '#FF9900'
            },
            opposite: true
        }];

    highchartsOptions.plotOptions =
        {
            series: {
                marker: {
                    enabled: false,
                    symbol: "circle",
                    radius: 3,
                    states: {
                        hover: {
                            enabled: true
                        }
                    }
                },
                lineWidth: 2
                //point: {
                //events: {
                //mouseOver: function() {syncMapToChartHover(this.x);}
                //}
                //},
                //events: {
                //legendItemClick: function(event) {legendItemClicked(event, this);},
                //mouseOut: function(event) {if (rkMap) rkMap.currLocMarker.hide();}
                //}
            }
        };
    highchartsOptions.tooltip =
        {
            formatter: function () {
                var pointData;
                var xValue = this.x;

                data.ChartSeries[0].Points.forEach(function (element, index, array) {
                    if (element[0] == xValue) {
                        pointData = element[1];
                    }
                });

                var timeStr = ""; //(speedpointData) ? "<br/>Time: " + formatTime(pointData.time / 60.0) : "";
                var yValStr;
                if (pointData) {
                    yValStr = Highcharts.numberFormat(this.y, 2);
                } else {
                    yValStr = Highcharts.numberFormat(this.y, 0);
                }

                if (data.ChartSeries[this.series.index].Type == "PACE") {
                    yValStr = $.convertNumberToTime(yValStr);
                }

                return "<b>" + data.ChartSeries[this.series.index].Type + ": " + yValStr + " " + data.ChartSeries[this.series.index].UnitY
                    + "</b><br/>"
                    + "Distanz: " + Highcharts.numberFormat(this.x, 2) + " km"
                    + timeStr;
            }
        };
    if (chart) {
        chart.destroy();
    }

    chart = new Highcharts.Chart(highchartsOptions);
}

function CreateWeather(lat, lon) {

    //zusaätzlich noch aktuelles datum vergleichen, nur wenn heut auch die einheit absolviert wude, macht wetter sin
    var today = new Date();
    var dateExercise = $("#Date").datepicker("getDate");
    var timeDiff = (today - dateExercise) / 1000 / 60 / 60 / 24; //ms -> day
    if ($("#Temperature").val() != "" || $("#WeatherCondition").val() != undefined ||
        $("#WeatherNote").val() != "" || timeDiff >= 1) {
        return;
    }


    /* Configuration */

    var APPID = 't3COxJ7g';		// Your Yahoo APP id
    var DEG = 'c';		// c for celsius, f for fahrenheit

    // Mapping the weather codes returned by Yahoo's API
    // to the correct icons in the img/icons folder

    var weatherIconMap = [
		'storm', 'storm', 'storm', 'lightning', 'lightning', 'snow', 'hail', 'hail',
		'drizzle', 'drizzle', 'rain', 'rain', 'rain', 'snow', 'snow', 'snow', 'snow',
		'hail', 'hail', 'fog', 'fog', 'fog', 'fog', 'wind', 'wind', 'snowflake',
		'cloud', 'cloud_moon', 'cloud_sun', 'cloud_moon', 'cloud_sun', 'moon', 'sun',
		'moon', 'sun', 'hail', 'sun', 'lightning', 'lightning', 'lightning', 'rain',
		'snowflake', 'snowflake', 'snowflake', 'cloud', 'rain', 'snow', 'lightning'
    ];

    // Yahoo's PlaceFinder API http://developer.yahoo.com/geo/placefinder/
    // We are passing the R gflag for reverse geocoding (coordinates to place name)
    var geoAPI = 'http://where.yahooapis.com/geocode?location=' + lat + ',' + lon + '&flags=J&gflags=R&appid=' + APPID;

    // Forming the query for Yahoo's weather forecasting API with YQL
    // http://developer.yahoo.com/weather/

    var wsql = 'select * from weather.forecast where woeid=WID and u="' + DEG + '"',
        weatherYQL = 'http://query.yahooapis.com/v1/public/yql?q=' + encodeURIComponent(wsql) + '&format=json&callback=?',
        code, city, results, woeid;

    if (window.console && window.console.info) {
        console.info("Coordinates: %f %f", lat, lon);
    }

    // Issue a cross-domain AJAX request (CORS) to the GEO service.
    // Not supported in Opera and IE.
    $.getJSON(geoAPI, function (r) {

        if (r.ResultSet.Found == 1) {

            results = r.ResultSet.Results;
            city = results[0].city;
            code = results[0].statecode || results[0].countrycode;

            // This is the city identifier for the weather API
            woeid = results[0].woeid;

            // Make a weather API request:
            $.getJSON(weatherYQL.replace('WID', woeid), function (r) {

                if (r.query && r.query.count == 1) {

                    // Create the weather items in the #scroller UL

                    var item = r.query.results.channel.item.condition;

                    //console.log(r.query.results);
                    if (!item) {
                        showError("We can't find weather information about your city!");
                        if (window.console && window.console.info) {
                            console.info("%s, %s; woeid: %d", city, code, woeid);
                        }

                        return false;
                    }
                    $("#Temperature").val(item.temp);
                    MapWeatherCondition(item.code);
                    $("#WeatherNote").val(city);
                }
                else {
                    showError("Error retrieving weather data!");
                }
            });

        }

    }).error(function () {
        showError("Your browser does not support CORS requests!");
    });

}

/* Error handling functions */

function locationError(error) {
    switch (error.code) {
        case error.TIMEOUT:
            showError("A timeout occured! Please try again!");
            break;
        case error.POSITION_UNAVAILABLE:
            showError('We can\'t detect your location. Sorry!');
            break;
        case error.PERMISSION_DENIED:
            showError('Please allow geolocation access for this to work.');
            break;
        case error.UNKNOWN_ERROR:
            showError('An unknown error occured!');
            break;
    }

}

function MapWeatherCondition(weatherCode) {
    //0	tornado
    //1	tropical storm
    //2	hurricane
    //3	severe thunderstorms
    //4	thunderstorms
    //5	mixed rain and snow
    //6	mixed rain and sleet
    //7	mixed snow and sleet
    //8	freezing drizzle
    //9	drizzle
    //10	freezing rain
    //11	showers
    //12	showers
    //13	snow flurries
    //14	light snow showers
    //15	blowing snow
    //16	snow
    //17	hail
    //18	sleet
    //19	dust
    //20	foggy
    //21	haze
    //22	smoky
    //23	blustery
    //24	windy
    //25	cold
    //26	cloudy
    //27	mostly cloudy (night)
    //28	mostly cloudy (day)
    //29	partly cloudy (night)
    //30	partly cloudy (day)
    //31	clear (night)
    //32	sunny
    //33	fair (night)
    //34	fair (day)
    //35	mixed rain and hail
    //36	hot
    //37	isolated thunderstorms
    //38	scattered thunderstorms
    //39	scattered thunderstorms
    //40	scattered showers
    //41	heavy snow
    //42	scattered snow showers
    //43	heavy snow
    //44	partly cloudy
    //45	thundershowers
    //46	snow showers
    //47	isolated thundershowers
    //3200	not available
    //http://developer.yahoo.com/weather/
    var sportDiaryWeatherCode = 0;
    if (weatherCode <= 5 || weatherCode == 18 || weatherCode == 19 ||
        weatherCode == 39 || weatherCode == 40 || weatherCode == 41 || weatherCode == 49) {
        sportDiaryWeatherCode = 8;//"thunderstorms";
    }
    else if (weatherCode == 6 || (weatherCode >= 14 && weatherCode <= 17) ||
        weatherCode == 26 ||
        weatherCode == 43 || weatherCode == 44 || weatherCode == 45 || weatherCode == 48) {
        sportDiaryWeatherCode = 6;//"snow";
    }
    else if (weatherCode == 7 || weatherCode == 8 ||
       weatherCode == 24 || weatherCode == 25 || weatherCode == 36) {
        sportDiaryWeatherCode = 3;//"haze";
    }
    else if ((weatherCode >= 9 && weatherCode <= 13) ||
        weatherCode == 42 || weatherCode == 47) {
        sportDiaryWeatherCode = 2;// "drizzle";
    }
    else if ((weatherCode >= 20 && weatherCode <= 23) ||
        (weatherCode >= 27 && weatherCode <= 31) || weatherCode == 46) {
        sportDiaryWeatherCode = 1;// "cloudy";
    }
    else if ((weatherCode >= 32 && weatherCode <= 35)
        || weatherCode == 37) {
        sportDiaryWeatherCode = 7;//"sunny";
    }
    $('#SelectedWeatherConditionDdl').ddslick('select', { index: sportDiaryWeatherCode });
}
//function showError(msg) {
//    weatherDiv.addClass('error').html(msg);
//}

function getPathFromUrl() {
    if (window.location.pathname.indexOf("Exercise") >= 0) {
        console.log("Exercise/");
        return "Exercise/";
    }
    else {
        console.log("Plan/");

        return "Plan/";
    }
}


//function updateCalendar__(phases) {
//    $(".Exercise").tooltip();

//    $(".dayContainer").on("dblclick", function (event) { showDetails(this, event, "#tableInfo"); });
//    $(".deleteButton").on("click", function (event) {
//        event.preventDefault();
//        var $btn = $(this);
//        var $msg = $(this).attr("title");
//        var exercise = $(this).parents(".Exercise");
//        bootbox.confirm($msg, function (result) {
//            if (result) {
//                deleteRow($btn, function () {
//                    //update data table
//                    //$.updateTableData(currentParams);
//                    $(exercise).remove();
//                });
//            }
//        });
//    });
//    //$("#monthList > li > a").on("click", function () {
//    //    var monthName = $(this).attr("id");
//    //    var selectedMonth = monthName.substring("month".length);
//    //    var year = $("#CurrentYear").val();
//    //    $("#calendarview").load("Calendar/" + selectedMonth + "/" + year,
//    //        function (html) {
//    //            loadCalendarView(html);
//    //        });
//    //});
//    //$("#yearList > li > a").on("click", function () {
//    //    var year = $(this).attr("id");
//    //    var month = $("#CurrentMonth").val();
//    //    $("#calendarview").load("Calendar/" + month + "/" + year,
//    //        function (html) {
//    //            loadCalendarView(html);
//    //        });
//    //});
//    //$("#previousMonth").on("click", function () {
//    //    var month = $("#CurrentMonth").val();
//    //    var year = $("#CurrentYear").val();
//    //    if (month < 1 || month > 12)
//    //        month = null;
//    //    if (year < 1910 || year > 2018)
//    //        year = null;
//    //    month--;
//    //    if (month == 0) {
//    //        year--;
//    //        month = 12;
//    //    }
//    //    $("#calendarview").load("Calendar/" + month + "/" + year,
//    //        function (html) {
//    //            loadCalendarView(html);
//    //        });
//    //});
//    //$("#nextMonth").on("click", function () {
//    //    var month = $("#CurrentMonth").val();
//    //    var year = $("#CurrentYear").val();
//    //    if (month < 1 || month > 12)
//    //        month = null;
//    //    if (year < 2000 || year > 2050)
//    //        year = null;
//    //    month++;
//    //    if (month == 13) {
//    //        year++;
//    //        month = 1;
//    //    }
//    //    $("#calendarview").load("Calendar/" + month + "/" + year,
//    //        function (html) {
//    //            loadCalendarView(html);
//    //        });
//    //});


//    $(".phaseText").on("click", function () {
//        if ($(this).find("#phasesList").length > 0) return;
//        var control = '<select id="phasesList"></select>';
//        $(this).html(control);
//        $("#PhaseTemplate").tmpl(phases).appendTo("#phasesList");
//        $("#phasesList").focus();
//    });
//    //beim blättern wird sonst das Event mehrfach gebunden
//    $("#phasesList").on("die");

//    $("#phasesList").on("blur", function () {
//        var parent = $(this).parents(".phaseText");
//        var selectedId = $("#phasesList").val();
//        var currentYear = $("#CurrentYear").val();
//        var currentWeek = $(this).parents("th").find(".weekNumber").text();
//        $(parent).html($(parent).find("option:selected").text());
//        //post
//        $.ajax({
//            type: "POST",
//            url: "/" + getPathFromUrl() + "UpdateWeekPhase",
//            data: "phaseId=" + selectedId + "&weekNumber=" + currentWeek + "&currentYear=" + currentYear,
//            success: function (result) {
//                if (result.success) $("#feedback_status").attr("value", "Phase wurde gespeichert.");
//            },
//            error: function (req, status, error) {
//                alert("Sorry! Leider gabs ein Problem.");
//            }
//        });
//    });
//    var type = getPathFromUrl();
//    if (type.indexOf("Exercise") >= 0) {
//        $(".copyButton").on("click", function (event) {
//            var eclone = $(event.currentTarget).parents(".Exercise").clone();
//            var content = $(event.currentTarget).parents(".dayContent");
//            var exerciseId = eclone.attr("id");
//            eclone.appendTo(content);
//            eclone.attr("id", "session");
//            var dayId = $(event.currentTarget).parents(".dayContainer").attr("id"); //td mit tag

//            $.ajax({
//                type: "POST",
//                url: "/" + getPathFromUrl() + "UpdateSessionDate",
//                data: "dayId=" + dayId + "&sessionId=" + exerciseId + "&shouldCopy=" + true,
//                success: function (result) {
//                    if (result.success) {
//                        $("#feedback_status").attr("value", "Speichern...");
//                        eclone.attr("id", "session_" + result.exerciseId);
//                    }
//                },
//                error: function (req, status, error) {
//                    alert("Sorry! Leider gabs ein Problem.");
//                }
//            });
//        });
//    }

//    $(".Exercise").on("mouseover", function () {
//        console.log("m");
//        $(this).find(".itemMenu").show();
//    })
//    .on("mouseout", function () {
//        $(this).find(".itemMenu").hide();
//    });

//    $(".dayContainer").on("mouseover", function () {
//        $(this).find(".showDetails").show();
//    })
//    .on("mouseout", function () {
//        $(this).find(".showDetails").hide();
//    });
//    doExerciseSortable();

//}
function updatePlanAdditionals() {
    $(".Plan").tooltip();

    $("a.aUpdateFavorite").on("click", function (event) {
        event.preventDefault();
        updateFavorite($(this));
    });

    $("#planview").load("/Plan/Favorites/",
        function (html) {
            $('#planview')[0].value = html;
        });
    //$(window).resize(resizeFrame).resize();

    function resizeFrame() {
        var h = $(window).height();
        var w = $(window).width();
        $("#planslider").css('height', h - 100);
    }

    $(".copyButton").on("click", function (event) {
        var eclone = $(event.currentTarget).parents(".Exercise").clone();
        var content = $(event.currentTarget).parents(".dayContent");
        var exerciseId = eclone.attr("id");
        eclone.appendTo(content);
        eclone.attr("id", "session");
        var dayId = $(event.currentTarget).parents(".dayContainer").attr("id"); //td mit tag
        console.log(dayId);
        $.ajax({
            type: "POST",
            url: "/" + getPathFromUrl() + "UpdateSessionDate",
            data: "dayId=" + dayId + "&sessionId=" + exerciseId + "&shouldCopy=" + true,
            success: function (result) {
                if (result.success) {
                    $("#feedback_status").attr("value", "Speichern...");
                    eclone.attr("id", "session_" + result.planid);
                    //update links
                    var newId = { SessionId: result.planid };
                    //replace itemMenu
                    eclone.remove(".itemMenu");
                    $("#ItemMenu").tmpl(newId).appendTo(eclone);
                }
            },
            error: function (req, status, error) {
                alert("Sorry! Leider gabs ein Problem.");
            }
        });
    });
}

function doExerciseSortable() {
    $(".dayContent").sortable({
        connectWith: ".dayContent",
        //placeholder: "ui-state-highlight",
        //handle: ".copyButton",
        stop: function (event, ui) {
            //.Exercise oder Fav oder Copy
            console.log(ui.item.attr("class"));
            var shouldCopyFromFav = ui.item.attr("class").indexOf("isFavorite") > -1;
            var shouldCopyFromMenu = ui.item.attr("class").indexOf("copyButton") > -1;
            var shouldCopy = shouldCopyFromFav || shouldCopyFromMenu;
            var exerciseId;
            //get exerciseId from elements
            if (!shouldCopyFromMenu) {
                exerciseId = ui.item.attr("id");
            } else {
                exerciseId = ui.item.closest(".Exercise").attr("id");
            }
            var dayId = ui.item.closest(".dayContainer").attr("id"); //td mit tag
            $.ajax({
                type: "POST",
                url: "/" + getPathFromUrl() + "UpdateSessionDate",
                data: "dayId=" + dayId + "&sessionId=" + exerciseId + "&shouldCopy=" + shouldCopy,
                success: function (result) {
                    if (result.success) $("#feedback_status").attr("value", "Speichern...");
                    //$("#calendarview").load("/@HttpContext.Current.Request.Url.Segments[1]Calendar/",
                    //function (html) { $('#calendarview')[0].value = html; });
                },
                error: function (req, status, error) {
                    alert("Sorry! Leider gabs ein Problem.");
                }
            });
        }
    });

    $(".Exercise").disableSelection();
}

function doPlanSortable() {
    $(".dayContent").sortable({
        connectWith: ".dayContent",
        stop: function (event, ui) {
            console.log("stop");
            //.Exercise oder Fav oder Copy
            var shouldCopyFromFav = ui.item.attr("class").indexOf("IsFavorite") > -1;
            var exerciseId;
            //get exerciseId from elements
            if (!shouldCopyFromFav) {
                //move
                console.log("move");
                exerciseId = ui.item.attr("id");
                var dayId = ui.item.closest(".dayContainer").attr("id"); //td mit tag

                console.log(dayId);
                $.ajax({
                    type: "POST",
                    url: "/" + getPathFromUrl() + "UpdateSessionDate",
                    data: "dayId=" + dayId + "&sessionId=" + exerciseId + "&shouldCopy=" + false,
                    success: function (result) {
                        if (result.success) $("#feedback_status").attr("value", "Speichern...");
                    },
                    error: function (req, status, error) {
                        alert("Sorry! Leider gabs ein Problem.");
                    }
                });
            }
            else {
                //move from favorites
                console.log("favmove");

                var eclone = ui.item;
                var allClasses = ui.item.attr("class").split(" ");
                for (var i = 0; i < allClasses.length ; i++) {
                    if (allClasses[i].indexOf("session") == 0) {
                        exerciseId = allClasses[i];
                    }
                };
                var dayId = $(event.target).parents(".dayContainer").attr("id"); //td mit tag

                $.ajax({
                    type: "POST",
                    url: "/" + getPathFromUrl() + "UpdateSessionDate",
                    data: "dayId=" + dayId + "&sessionId=" + exerciseId + "&shouldCopy=" + true,
                    success: function (result) {
                        if (result.success) {
                            $("#feedback_status").attr("value", "Speichern...");
                            eclone.attr("id", "session_" + result.planid);
                            //remove all classes that are only relevant for favorite box
                            var newId = { SessionId: result.planid };
                            $("#ItemMenu").tmpl(newId).appendTo(eclone);
                            eclone.removeClass("IsFavorite");
                            eclone.removeClass("Plan");
                            eclone.removeClass("ui-draggable");
                            eclone.removeClass(exerciseId);
                        }
                    },
                    error: function (req, status, error) {
                        alert("Sorry! Leider gabs ein Problem.");
                    }
                });
            }
        }
    });
    $(".Exercise").disableSelection();
}

function loadCalendarView(html) {
    $('#calendarview')[0].value = html;
    var type = getPathFromUrl();
    if (type.indexOf("Exercise") >= 0) {
        doExerciseSortable();
        updateCalendar();
    }
    else {
        doPlanSortable();
        updateCalendar();
        updatePlanAdditionals();
    }
}
