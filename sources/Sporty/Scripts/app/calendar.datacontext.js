window.entitiesApp = window.entitiesApp || {};

/* datacontext: data access and model management layer */
window.entitiesApp.datacontext = (function () {
    var datacontext = {
        getExercises: getExercises,
        createWeek: createWeek,
        createFavorite: createFavorite,
        removeSession: removeSession,
        updateSessionDate: updateSessionDate,
        updateSessionWeek: updateSessionWeek
    };

    return datacontext;

    var weeks;
    //#region Private Members
    function getExercises(monthindex, year, weeksObs, monthNameObs, monthIdObs, yearObs, favoritesObs, errorObservable) {
        return ajaxRequest("get", calendarUrl(monthindex, year))
            .done(getSucceeded)
            .fail(getFailed);

        function getSucceeded(data) {
            var mappedWeeks = $.map(data.weeks,
                function (list) {
                    return new createWeek(list);
                });

            weeksObs(mappedWeeks);
            weeks = weeksObs;
            monthNameObs(data.monthName);
            monthIdObs(data.month);

            if (data.favorites) {
                var mappedFavs = $.map(data.favorites,
                    function (list) {
                        return new createFavorite(list);
                    })

                favoritesObs(mappedFavs);
            }


            yearObs(data.year);
        }

        function getFailed(error) {
            console.log("failed:" + error);
            errorObservable("Error retrieving todo lists: " + error.message);
        }
    }

    function updateFavorite(favId) {
        //UpdateFavorite
        var urlData = "?id=" + favId;
        return ajaxRequest("post", "/" + calendarMode + "/api/calendar/updatefavorite" + urlData)
                .done(getSucceeded)
                .fail(getFailed);
        function getSucceeded(data) {
            callback(data);
        }

        function getFailed() {
            errorObservable("Error retrieving data.");
        }

        function getSucceeded(data) {
            return true;
        }

        function getFailed(error) {
            console.log("failed:" + error);
            return false;
        }
    }

    function removeSession(sessionId) {
        return ajaxRequest("delete", "/" + calendarMode + "/api/calendar/" + sessionId)
            .done(getSucceeded)
            .fail(getFailed);

        function getSucceeded(data) {
            return true;
        }

        function getFailed(error) {
            console.log("failed:" + error);
            return false;
        }
    }

    function createWeek(data) {
        return new datacontext.week(data);
    }

    function createFavorite(data) {
        return new datacontext.favorite(data);
    }

    function updateSessionWeek(session, dayId, sessionId, oldWeekNumber, newWeekNumber) {
        var isFavorite = true;
        if (oldWeekNumber != undefined) {
            for (var i = 0; i < weeks().length; i++) {
                if (weeks()[i].number() == oldWeekNumber) {
                    weeks()[i].weekSummary.removeSession(session);
                }
            }
            isFavorite = false;
        }
        for (var i = 0; i < weeks().length; i++) {
            if (weeks()[i].number() == newWeekNumber) {
                weeks()[i].weekSummary.addSession(session);
            }
        }
        updateSessionDate(dayId, sessionId, isFavorite, function (newId) {
            //console.log("callback" + newId);
        });
    };

    function updateSessionDate(dayId, sessionId, shouldCopy, callback) {
        //var urlData = {
        //    "dayId": dayId,
        //    "sessionId": sessionId,
        //    "shouldCopy": true
        //};
        var urlData = "?dayId=" + dayId + "&sessionId=" + sessionId + "&shouldCopy=" + shouldCopy;
        return ajaxRequest("post", "/" + calendarMode + "/api/calendar/UpdateSessionDate" + urlData)
                .done(getSucceeded)
                .fail(getFailed);
        function getSucceeded(data) {
            callback(data);
        }

        function getFailed() {
            errorObservable("Error retrieving data.");
        }
    }
    
    // Private
    function clearErrorMessage(entity) { entity.errorMessage(null); }
    function ajaxRequest(type, url, data, dataType) { // Ajax helper
        var options = {
            dataType: dataType || "json",
            contentType: "application/json",
            cache: false,
            type: type,
            data: data ? data.toJson() : null
        };
        var antiForgeryToken = $("#antiForgeryToken").val();
        if (antiForgeryToken) {
            options.headers = {
                'RequestVerificationToken': antiForgeryToken
            }
        }
        return $.ajax(url, options);
    }

    // routes
    function calendarUrl(month, year) {
        var m = "month=" + (month || "");
        var y = "year=" + (year || "");
        var splitter = "";
        if (month > 0) splitter = "&";
        console.log(calendarMode);
        return "/" + calendarMode + "/api/calendar?" + m + splitter + y;
    }


})();