/* bindings: extends KO with custom data bindings */

// Controls whether or not the text in a textbox is selected based on a model property
ko.bindingHandlers.selected = {
    init: function (elem, valueAccessor) {
        $(elem).blur(function () {
            var boundProperty = valueAccessor();
            if (ko.isWriteableObservable(boundProperty)) {
                boundProperty(false);
            }
        });
    },
    update: function (elem, valueAccessor) {
        var shouldBeSelected = ko.utils.unwrapObservable(valueAccessor());
        if (shouldBeSelected) {
            $(elem).select();
        }
    }
};

// Makes a textbox lose focus if you press "enter"
ko.bindingHandlers.blurOnEnter = {
    init: function (elem, valueAccessor) {
        $(elem).keypress(function (evt) {
            if (evt.keyCode === 13 /* enter */) {
                evt.preventDefault();
                $(elem).triggerHandler("change");
                $(elem).blur();
            }
        });
    }
};

// Simulates HTML5-style placeholders on older browsers
ko.bindingHandlers.placeholder = {
    init: function (elem, valueAccessor) {
        var placeholderText = ko.utils.unwrapObservable(valueAccessor()),
            input = $(elem);

        input.attr('placeholder', placeholderText);

        // For older browsers, manually implement placeholder behaviors
        if (!Modernizr.input.placeholder) {
            input.focus(function () {
                if (input.val() === placeholderText) {
                    input.val('');
                    input.removeClass('placeholder');
                }
            }).blur(function () {
                setTimeout(function () {
                    if (input.val() === '' || input.val() === placeholderText) {
                        input.addClass('placeholder');
                        input.val(placeholderText);
                    }
                }, 0);
            }).blur();

            input.parents('form').submit(function () {
                if (input.val() === placeholderText) {
                    input.val('');
                }
            });
        }
    }
};


// extends observable objects intelligently
ko.utils.extendObservable = function (target, source) {
    var prop, srcVal, tgtProp, srcProp,
        isObservable = false;

    for (prop in source) {

        if (!source.hasOwnProperty(prop)) {
            continue;
        }

        if (ko.isWriteableObservable(source[prop])) {
            isObservable = true;
            srcVal = source[prop]();
        } else if (typeof (source[prop]) !== 'function') {
            srcVal = source[prop];
        }

        if (ko.isWriteableObservable(target[prop])) {
            target[prop](srcVal);
        } else if (target[prop] === null || target[prop] === undefined) {

            target[prop] = isObservable ? ko.observable(srcVal) : srcVal;

        } else if (typeof (target[prop]) !== 'function') {
            target[prop] = srcVal;
        }

        isObservable = false;
    }
};

// then finally the clone function
ko.utils.clone = function (obj, emptyObj) {
    var json = ko.toJSON(obj);
    var js = JSON.parse(json);

    return ko.utils.extendObservable(emptyObj, js);
};

//var _dragged;
//ko.bindingHandlers.drag = {
//    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
//        var dragElement = $(element);
//        var dragOptions = {
//            connectToSortable: "#dayContent", //"#ko_container",
//            helper: 'clone',
//            revert: true,
//            revertDuration: 0,
//            start: function () {
//                _dragged = ko.utils.unwrapObservable(valueAccessor().value);
//            },
//            cursor: 'default'
//        };
//        dragElement.draggable(dragOptions).disableSelection();
//    }
//};

//ko.bindingHandlers.drop = {
//    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
//        console.log("drop");
//        console.log(element);
//        console.log(valueAccessor);
//        var dropElement = $(element);
//        var dropOptions = {
//            drop: function (event, ui) {
//                valueAccessor().value(_dragged);
//            }
//        };
//        dropElement.droppable(dropOptions);
//    }
//};
//connect items with observableArrays
//ko.bindingHandlers.sortableList = {
//    init: function (element, valueAccessor) {
//        var list = valueAccessor();
//        $(element).sortable({
//            update: function (event, ui) {
//                //retrieve our actual data item
//                //console.log(ui.item.data());
//                //console.log(ui.item[0]);
//                var item = ui.item.data();
//                //figure out its new position
//                var position = ko.utils.arrayIndexOf(ui.item.parent().children(), ui.item[0]);
//                console.log(position);
//                //remove the item and add it back in the right spot
//                if (position >= 0) {
//                    list.remove(item.currentItem);
//                    console.log(item.currentItem);
//                    list.push(item.currentItem);//.splice(position, 0, item);
//                }
//                ui.item.remove();
//            }
//        });
//    }
//};