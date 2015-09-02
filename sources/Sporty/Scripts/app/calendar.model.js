(function (ko, datacontext) {
    datacontext.week = week;
    datacontext.favorite = favorite;

    function newSession(data) {
        var self = this;
        data = data || {};
        self.isFavorite = ko.observable(false);
        self.sessionId = ko.observable(data.sessionId);
        self.sportTypeName = ko.observable(data.sportTypeName);
        self.sportTypeId = ko.observable(data.sportTypeId);
        self.duration = ko.observable(data.duration);
        self.durationWithUnit = ko.computed(function () {
            if (self.duration())
                return self.duration() + " '";
            return "";
        });
        self.plannedDuration = ko.observable(data.plannedDuration);
        self.distance = ko.observable(data.distance);
        self.distanceWithUnit = ko.computed(function () {
            if (self.distance())
                return self.distance() + " km";
            return "";
        });
        self.plannedDistance = ko.observable(data.plannedDistance);
        self.zoneId = ko.observable(data.zoneId);
        self.zoneName = ko.observable(data.zoneName);
        self.heartrate = ko.observable(data.heartrate);
        self.heartrateWithUnit = ko.computed(function () {
            if (self.heartrate())
                return self.heartrate() + " bpm";
            return "";
        });
        self.isFavorite = ko.observable(data.isFavorite);
        self.discipline = ko.observable(data.discipline);

        self.showMenu = ko.observable(false);
        self.showItemMenu = function () {
            self.showMenu(true);
        };
        self.hideItemMenu = function () {
            self.showMenu(false);
        };

        self.toJson = function () { return ko.toJSON(self) };
    }

    function newDay(data, weekNumber, removeSession, addSession) {

        var self = this;
        data = data || {};
        self.currentDay = data.day;
        self.clientId = data.clientId;
        self.goalName = ko.observable(data.goalName);
        self.isDayInMonth = ko.observable(data.isDayInMonth);
        self.isGoalToday = ko.observable(data.isGoalToday);
        self.isDayToday = ko.observable(data.isDayToday);
        self.sessions = ko.observableArray(new importSession(data.sessions));
        self.sessions.parentId = self.clientId;
        self.sessions.weekNumber = weekNumber;

        self.showMenu = ko.observable(false);
        self.showAddMenu = function () {
            self.showMenu(true);
        };
        self.hideAddMenu = function () {
            self.showMenu(false);
        };
        self.createNewSesssionLink = ko.computed(function () {
            return "/" + calendarMode + "/create?defaultDate=" + self.clientId;
        });

        self.deleteSession = function (session, event) {
            //current session was deleted
            var msg = $(event.target).attr("title");
            bootbox.confirm(msg, function (boxResult) {
                if (boxResult) {
                    //remove from summary
                    removeSession(session);
                    //delete session
                    self.sessions.remove(session);
                    datacontext.removeSession(session.sessionId());
                }
            });
        };

        self.copySession = function (session) {
            //current session was copied
            var sessionClone = new newSession(ko.toJS(session));
            //add session to summary
            addSession(session);
            self.sessions.push(sessionClone);
            datacontext.updateSessionDate(self.clientId, sessionClone.sessionId(), true, function (newId) {
                sessionClone.sessionId(newId);
                //console.log("callback" + newId);
            });
        };
        self.toJson = function () { return ko.toJSON(self) };
    }

    function getNewSession(sessionItem) {
        return new session(sessionItem);
    }

    function newSummary(data, weekNumber) {
        var self = this;
        self.weekNumber = weekNumber;
        self.discipline = data.discipline;
        self.sportTypeId = ko.observable(data.sportTypeId);
        self.duration = ko.observable(data.duration);
        self.plannedDuration = ko.observable(data.plannedDuration);
        self.distance = ko.observable(data.distance);
        self.plannedDistance = ko.observable(data.plannedDistance);
        self.durationInTime = ko.computed(function () {
            if (self.duration() > 0 && self.duration() != undefined)
                return secondsToHms(self.duration());
            return " - ";
        });
        self.plannedDurationInTime = ko.computed(function () {
            if (self.plannedDuration() > 0 && self.plannedDuration() != undefined)
                return secondsToHms(self.plannedDuration());
            return " - ";
        });
    }

    function favorite(data) {
        var self = this;
        data = data || {};
        //{"id":5,"date":"2011-03-31T00:00:00","sportTypeName":"Schwimmen","zoneName":"Endurance","zoneId":2,
        //"duration":60,"distance":null,"trainingTypeName":"Training","description":null,"discipline":"Swimming"}
        self.isFavorite = ko.observable(true);
        self.sessionId = ko.observable(data.id);
        self.sportTypeName = ko.observable(data.sportTypeName);
        self.sportTypeId = ko.observable(data.sportTypeId);

        self.description = ko.observable(data.description);
        self.duration = ko.observable(data.duration);
        self.plannedDuration = ko.observable(data.plannedDuration);
        self.plannedDurationWithUnit = ko.computed(function () {
            if (self.plannedDuration())
                return self.plannedDuration() + " '";
            return "";
        });
        self.distance = ko.observable(data.distance);
        self.distanceWithUnit = ko.computed(function () {
            if (self.distance())
                return self.distance() + " km";
            return "";
        });
        self.heartrate = ko.observable(data.heartrate);
        self.heartrateWithUnit = ko.computed(function () {
            if (self.heartrate())
                return self.heartrate() + " bpm";
            return "";
        });
        self.plannedDistance = ko.observable(data.plannedDistance);
        self.zoneId = ko.observable(data.zoneId);
        self.zoneName = ko.observable(data.zoneName);
        self.discipline = ko.observable(data.discipline);

        self.showMenu = ko.observable(false);
        self.showItemMenu = function () {
            self.showMenu(true);
        };
        self.hideItemMenu = function () {
            self.showMenu(false);
        };


        self.toJson = function () { return ko.toJSON(self) };
    }

    function week(data) {
        var self = this;
        data = data || {};

        self.number = ko.observable(data.number);
        self.firstDayInWeek = ko.observable(data.firstDayInWeek);
        self.phase = ko.observable(data.phase);
        self.weekNumberToNextGoal = ko.observable(data.weekNumberToNextGoal);
        self.weekSummary = ko.observableArray(new importSummary(data.weekSummary, self.number()));

        self.weekSummary.removeSession = function (session) {
            removeSessionFromWeekSummary(session);
        };

        self.weekSummary.addSession = function (session) {
            addSessionToWeekSummary(session);
        };

        self.afterDrag = function (arg, event, ui) {
            //clone current fav
            var sessionClone = new newSession(ko.toJS(arg));
            return sessionClone;
        };
        self.updateSession = function (arg, event, ui) {
            //session or favorite was moved
            var oldWeekNumber;
            var session;

            if (!arg.item.isFavorite()) {
                oldWeekNumber = arg.sourceParent.weekNumber;
                session = arg.item;
                var newWeekNumber = arg.targetParent.weekNumber;
                var dayId = arg.targetParent.parentId;
                var sessionId = arg.item.sessionId();
                session.isFavorite(false);
                datacontext.updateSessionWeek(session, dayId, sessionId, oldWeekNumber, newWeekNumber);
            }
            else {
                //is fav clone session
                //erst der summary hinzufügen (wegen der plannedduration)
                addSessionToWeekSummary(arg.item);
                arg.item.isFavorite(false);
                arg.item.duration(arg.item.plannedDuration());
                ////add session to summary
                ////addSession(sessionClone);
                ////self.sessions.push(sessionClone);
                datacontext.updateSessionDate(arg.targetParent.parentId, arg.item.sessionId(), true, function (newId) {
                    arg.item.sessionId(newId);
                    //Meldung ausgeben?
                });
            }

        };

        self.days = ko.observableArray(new importDay(data.days, self.number(), self.weekSummary.removeSession, self.weekSummary.addSession));

        self.allDurationInTime = ko.computed(function () {
            //gesamt zeit
            var allDuration = 0;
            for (var i = 0; i < self.weekSummary().length; i++) {
                allDuration += self.weekSummary()[i].duration();
            }
            if (allDuration <= 0)
                return " - ";
            return secondsToHms(allDuration);
        });
        self.allPlannedDurationInTime = ko.computed(function () {
            //gesamt zeit
            var allPlannedDuration = 0;
            for (var i = 0; i < self.weekSummary().length; i++) {
                allPlannedDuration += self.weekSummary()[i].plannedDuration();
            }
            if (allPlannedDuration <= 0)
                return " - ";
            return secondsToHms(allPlannedDuration);
        });
        self.toJson = function () { return ko.toJSON(self) };

        function removeSessionFromWeekSummary(session) {
            for (var i = 0; i < self.weekSummary().length; i++) {
                if (session.sportTypeId() === self.weekSummary()[i].sportTypeId()) {
                    var dur = self.weekSummary()[i].duration() - session.duration();
                    if (dur == NaN || dur < 0)
                        dur = 0;
                    self.weekSummary()[i].duration(dur);
                    var pdur = self.weekSummary()[i].plannedDuration() - session.plannedDuration();
                    if (pdur == NaN || pdur < 0)
                        pdur = 0;
                    self.weekSummary()[i].plannedDuration(pdur);

                    if (self.weekSummary()[i].duration() == 0 && self.weekSummary()[i].plannedDuration() == 0)
                    {
                        self.weekSummary.remove(self.weekSummary()[i]);
                    }
                }
            }
        }

        function addSessionToWeekSummary(session) {
            //falls sportTypeId schon existiert, dann hinzufügen
            var sportTypeIdExits = false;
            for (var i = 0; i < self.weekSummary().length; i++) {
                if (session.sportTypeId() === self.weekSummary()[i].sportTypeId()) {
                    sportTypeIdExits = true;
                    if (session.duration()) {
                        self.weekSummary()[i].duration(self.weekSummary()[i].duration() + session.duration());
                    }
                    if (session.plannedDuration()) {
                        self.weekSummary()[i].plannedDuration(self.weekSummary()[i].plannedDuration() + session.plannedDuration());
                    }
                }
            }
            if (!sportTypeIdExits)
            {
                //sporttype noch nicht vorhanden
                var data = {
                    discipline: session.discipline(),
                    sportTypeId: session.sportTypeId(),
                    duration: session.duration(),
                    plannedDuration: session.plannedDuration(),
                    distance: 0,
                    plannedDistance: 0
                };

                var newSportSummary = new newSummary(data, self.number());
                self.weekSummary.push(newSportSummary);
                ////sportTypeId hinzufügen
                //if (session.duration()) {
                //    self.weekSummary.push(newSportSummary);
                //}
                //if (session.plannedDuration()) {
                //    self.weekSummary()[i].plannedDuration(self.weekSummary()[i].plannedDuration() + session.plannedDuration());
                //}
            }
        }
    }
    function importSummary(summary, weekNumber) {
        var mappedSummary = $.map(summary || [],
                function (summaryData) {
                    return new newSummary(summaryData, weekNumber);
                });

        return mappedSummary;
    }

    function importDay(days, weekNumber, removeSession, addSession) {
        var mappedDays = $.map(days || [],
                function (dayData) {
                    return new newDay(dayData, weekNumber, removeSession, addSession);
                });

        return mappedDays;
    }
    function importSession(session) {

        return $.map(session || [],
                function (sessionData) {
                    return new newSession(sessionData);
                });
    }
})(ko, entitiesApp.datacontext);

