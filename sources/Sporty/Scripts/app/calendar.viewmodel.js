/* Defines the Todo application ViewModel */
function viewModel(ko, datacontext) {
    var self = this;

    //var //calendar = ko.observable(),
    self.weeks = ko.observableArray();
    self.monthId = ko.observable();
    self.monthName = ko.observable();
    self.monthNames = ko.observableArray();
    self.year = ko.observable();
    self.years = ko.observableArray();
    self.error = ko.observable();
    self.favorites = ko.observableArray();

    self.previousMonth = function () {
        var currentMonth = self.monthId();
        if (currentMonth == 1) {
            self.monthId(12);
            self.monthName(mymonthNames[11]);
            var newYear = self.year() - 1;
            self.year(newYear);
        }
        else {
            var newId = self.monthId() - 1;
            self.monthId(newId);
            self.monthName(mymonthNames[newId - 1]);
        }
        datacontext.getExercises(self.monthId(), self.year(), self.weeks, self.monthName, self.monthId, self.year, self.favorites, self.error);
    };
    self.nextMonth = function () {
        var currentMonth = self.monthId();
        if (currentMonth == 12) {
            self.monthId(1);
            self.monthName(mymonthNames[0]);
            var newYear = self.year() + 1;
            self.year(newYear);
        }
        else {
            var newId = self.monthId() + 1;
            self.monthId(newId);
            self.monthName(mymonthNames[newId - 1]);
        }
        datacontext.getExercises(self.monthId(), self.year(), self.weeks, self.monthName, self.monthId, self.year, self.favorites, self.error);
    };
    self.selectedMonth = function (month) {
        self.monthName(month.name());
        self.monthId(month.monthId());

        return datacontext.getExercises(self.monthId(), self.year(), self.weeks, self.monthName, self.monthId, self.year, self.favorites, self.error)
         .done(function () {

         //    console.log("success sm");
         })
        .fail(function () {
            console.log("failure");
        });
    };
    self.selectedYear = function (year) {
        self.year(year.year());

        return datacontext.getExercises(self.monthId(), self.year(), self.weeks, self.monthName, self.monthId, self.year, self.favorites, self.error)
         .done(function () {
         //    console.log("success sm");
         })
        .fail(function () {
            console.log("failure");
        });
    };
    self.updateFavorite = function (fav, event) {
        //current session was deleted
        var msg = $(event.target).attr("title");
        bootbox.confirm(msg, function (boxResult) {
            if (boxResult) {
                //remove from summary
                updateFavorite(fav);
                //delete session
                self.favorites.remove(fav);
                datacontext.updateFavorite(fav.favoriteId());
            }
        });
    };

    //self.createNewPlanFromFavorite = function (arg, event, ui) {
    //    console.log("fav moved");
    //    //session was moved
    //    //var newWeekNumber = arg.targetParent.weekNumber;
    //    //var oldWeekNumber = arg.sourceParent.weekNumber;
    //    //var dayId = arg.targetParent.parentId;
    //    //var sessionId = arg.item.sessionId();
    //    //var session = arg.item;
    //    //datacontext.updateSessionWeek(session, dayId, sessionId, oldWeekNumber, newWeekNumber);
    //};

    datacontext.getExercises(self.monthId(), self.year(), self.weeks, self.monthName, self.monthId, self.year, self.favorites, self.error);

    var mappedMonthNames = $.map(mymonthNames,
        function (monthitem, index) {
            return new createNewMonth(monthitem, index);
        });
    var mappedYears = $.map(myyears,
        function (yearitem) {
            return new createNewYear(yearitem);
        });

    self.years(mappedYears);
    self.monthNames(mappedMonthNames);
};

// Initiate the Knockout bindings
ko.applyBindings(new viewModel(ko, entitiesApp.datacontext));

function createNewMonth(monthitem, index) {
    var self = this;
    self.name = ko.observable(monthitem);
    self.monthId = ko.observable(index + 1);
    self.toJson = function () { return ko.toJSON(self) };
};

function createNewYear(yearitem) {
    var self = this;
    self.year = ko.observable(yearitem);
    self.toJson = function () { return ko.toJSON(self) };
};