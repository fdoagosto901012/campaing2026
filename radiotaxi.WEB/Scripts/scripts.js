


jQuery(document).ready(function () {
    alert("aalll");
});



$(document).ready(function () {


    //////////////////////

    

    /////////////////////


    // Obtenemos lo valores de de cada evento.
    var calendarfull = $('#calendar').fullCalendar({
        eventDrop: function(event,dayDelta,minuteDelta,allDay,revertFunc){
            alert(
                event.id + " - "+event.title + " was moved " +
                dayDelta + " days and " +
                minuteDelta + " minutes."
            );
            if (allDay) {
                alert("Event is now all-day");
            }else{
                alert("Event has a time-of-day");
            }
            if (!confirm("Are you sure about this change?")) {
                revertFunc();
            }
        },
        eventMouseover: function( event, jsEvent, view ) {
            var layer = "";
            $(this).append(layer);
        },
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,basicWeek,basicDay'

        },
        editable: true,
        droppable: true, // this allows things to be dropped onto the calendar !!!
        drop: function(date, allDay) { // this function is called when something is dropped

            // retrieve the dropped element's stored Event Object
            var originalEventObject = $(this).data('eventObject');

            // we need to copy it, so that multiple events don't have a reference to the same object
            var copiedEventObject = $.extend({}, originalEventObject);

            // assign it the date that was reported
            copiedEventObject.start = date;
            copiedEventObject.allDay = allDay;

            // render the event on the calendar
            // the last `true` argument determines if the event "sticks" (http://arshaw.com/fullcalendar/docs/event_rendering/renderEvent/)
            $('#calendar').fullCalendar('renderEvent', copiedEventObject, true);

            // is the "remove after drop" checkbox checked?
            if ($('#drop-remove').is(':checked')) {
                // if so, remove the element from the "Draggable Events" list
                $(this).remove();
            }
        }
    });

    
});