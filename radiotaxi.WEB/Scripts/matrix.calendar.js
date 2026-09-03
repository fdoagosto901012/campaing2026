var dateService;


window.onload = function () {
    MenuSelector();
};



function MenuSelector() {

}

var Service = new Array();
var ServiceAux = new Array();
var date = new Date();
var d = date.getDate();
var m = date.getMonth();
var y = date.getFullYear();
var id = 0;

$(document).ready(function () {

    //loadLastTen();
    Date.prototype.addHours = function (h) {
        this.setHours(this.getHours() + h);
        return this;
    }

    Date.prototype.addMinutes = function (m) {
        this.setMinutes(this.getMinutes() + m);
        return this;
    }

    if (!Array.prototype.remove) {
        Array.prototype.remove = function (val) {
            var i = this.indexOf(val);
            return i > -1 ? this.splice(i, 1) : [];
        };
    }


    maruti.init();

    $('#add-event-submit').click(function () {
        maruti.add_event();
    });

    $('#event-name').keypress(function (e) {
        if (e.which == 13) {
            maruti.add_event();
        }
    });

    // MOdal

    $(document).on('click', '#sendDate', function (event) {
        event.preventDefault();
        /* Act on the event */
        var timepicker = $("#timepicker-24-hr").val().replace(" ", "").split(":");
        //console.log(timepicker[0]);
        //console.log(timepicker[1]);
        //console.log(dateService);
        dateService.setHours(timepicker[0]);
        dateService.setMinutes(timepicker[1]);
        $('#fullcalendar').fullCalendar('renderEvent', {
            id: id,
            title: dateService.getHours().toString() + ":" + dateService.getMinutes().toString() + " Servicio",
            start: dateService
        });

        Service.push({
            id: id,
            title: dateService.getHours().toString() + ":" + dateService.getMinutes().toString() + " Servicio",
            start: dateService
        });

        //console.log(Service);
        //console.log(Service.length);
        if (Service.length <= 1) {
            //console.log("Un servicio");

            $("#datePickUp").val(Service[Service.length - 1].start.toString());
        } else {
            //console.log("Multiples servicios");
        }
        id = id + 1;
        $('#calendar-form').modal('toggle');
    });


    /////////////////////////////////////////////////////////////////


    // Datepicker para el calendario
    
    $.validator.addMethod("regx", function (value, element, regexpr) {
        return regexpr.test(value);
    }, "Solo se aceptan valores numéricos");



    $('#createClient').validate({ // initialize the plugin
        rules: {
            phone: {
                required: true,
                regx: /^[0-9]*$/,
                minlength: 10,
                maxlength: 10,
            },
            name: {
                required:true
            },
            lastname1: {
                required:true
            },
            lastname2: {
                required: true
            },
            address: {
                required: true
            }

        },
        messages: {
            name : 'Nombre requerido',
            lastname1: "Campo requerido",
            lastname2: "Campo requerido",
            address: "Campo requerido",
            phone: {
                required: "Campo requerido, debe de contener de 1 a 8 caracteres n&uacute;mericos",
                minlength: "No puede ser menor de 10 car&aacute;cter",
                maxlength: "No puede ser mayor a 10 car&aacute;cter",
                regx: "Solo se aceptan valores num&eacute;ricos"
            },
        }
    });



    $('#createService').validate({ // initialize the plugin
        rules: {
            startPoint: {
                required : true,
            },
            endPoint: {
                required : true,
            },
            datePickUp: {
                required : true
            },
            phoneId: {
                required: true,
                regx: /^[0-9]*$/,
                minlength: 10,
                maxlength: 10,
            }
        },
        messages: {
            startPoint: "Campo requerido",
            endPoint: "Campo requerido",
            datePickUp: "Campo requerido",
            phoneId: {
                required: "Campo requerido, debe de contener de 1 a 8 caracteres n&uacute;mericos",
                minlength: "No puede ser menor de 10 car&aacute;cter",
                maxlength: "No puede ser mayor a 10 car&aacute;cter",
                regx: "Solo se aceptan valores num&eacute;ricos"
            },
        },
        submitHandler: function (form) {
            //console.log("Submitted!");
            var url = $("#createService").attr("action");
            //console.log(url);
            var string = "";
            $.each(Service, function (key, value) {
                //console.log(key);
                if (key > 0) {
                    string = string + "=" + value.start;
                } else {
                    string = string + value.start;
                }
            });
            string = $.param({ datesServices: string });
            //console.log(string);
            $("#createService").attr("action", url + "/?" + string);
            form.submit();
        }/*,
        submitHandler: function (form) { // for demo
            var self = this;
            e.preventDefault();
            alert("UN");
            var url = $("#createService").attr("action");
            var string = "";
            $.each(Service, function (key, value) {
                if (key > 0) {
                    string = string + "=" + value.start;
                } else {
                    string = string + value.start;
                }
            });
            string = $.param({ datesServices: string });
            $("#createService").attr("action", url + "/?" + string);
            //console.log($("#createService").attr("action"));
            return true;
        }*/
    });

    /*
    jQuery("#createService").submit(function (e) {
        //console.log("Te la comes");
        var self = this;
        e.preventDefault();
        var url = $("#createService").attr("action");
        var string = "";
        $.each(Service, function (key, value) {
            if (key > 0) {
                string = string + "=" + value.start;
            } else {
                string = string + value.start;
            }
        });
        string = $.param({ datesServices: string });
        $("#createService").attr("action", url + "/?" + string);
        //console.log($("#createService").attr("action"));
        this.submit();
        //return false; //is superfluous, but I put it here as a fallback
    });*/


    $("#timepicker-24-hr").wickedpicker({ twentyFour: true });
    ////////////////////////////////////////////////////////////////



    
    $(document).on('click', "#searchb2", function () {
        var phoneNumber = $("#phoneNumber2").val();

        
    });


});



maruti = {

    // === Initialize the fullCalendar and external draggable events === //
    init: function () {
        // Prepare the dates
        var date = new Date();
        var d = date.getDate();
        var m = date.getMonth();
        var y = date.getFullYear();

        $('#fullcalendar').fullCalendar({
            lang: 'es',
            header: {
                left: 'prev,next',
                center: 'title',
                right: 'month,basicWeek,basicDay'
            },
            dayClick: function (date, jsEvent, view) {
                $('#calendar-form').modal();
                dateService = new Date(date);
                //console.log(dateService);
            },
            eventClick: function (event, element) {
                console.log("--------Service----------");
                console.log(Service);
                console.log("-------------------------");
                //Service[event.id.toString()] = null;
                ServiceAux = new Array();

                $.each(Service, function (key, value) {
                    
                    if (value.id != event.id) {
                        ServiceAux.push({
                            id: value.id,
                            title: value.title,
                            start: value.start
                        });
                        console.log(key);
                    }
                });
                Service = new Array();
                Service = ServiceAux;
                //Service.remove(event.id.toString());
                $('#fullcalendar').fullCalendar('removeEvents', event._id);

                
                console.log("------Service Aux--------");
                console.log(Service);
                console.log("-------------------------");


            },
            editable: true,
            droppable: true, // this allows things to be dropped onto the calendar !!!
            drop: function (date, allDay) { // this function is called when something is dropped

                // retrieve the dropped element's stored Event Object
                var originalEventObject = $(this).data('eventObject');

                // we need to copy it, so that multiple events don't have a reference to the same object
                var copiedEventObject = $.extend({}, originalEventObject);

                // assign it the date that was reported
                copiedEventObject.start = date;
                copiedEventObject.allDay = allDay;

                // render the event on the calendar
                // the last `true` argument determines if the event "sticks" (http://arshaw.com/fullcalendar/docs/event_rendering/renderEvent/)
                $('#fullcalendar').fullCalendar('renderEvent', copiedEventObject, true);

                // is the "remove after drop" checkbox checked?

                // if so, remove the element from the "Draggable Events" list
                $(this).remove();

            }
        });
        this.external_events();
    },

    // === Adds an event if name is provided === //
    add_event: function () {
        if ($('#event-name').val() != '') {
            var event_name = $('#event-name').val();
            $('#external-events .panel-content').append('<div class="external-event ui-draggable label label-inverse">' + event_name + '</div>');
            this.external_events();
            $('#modal-add-event').modal('hide');
            $('#event-name').val('');
        } else {
            this.show_error();
        }
    },

    // === Initialize the draggable external events === //
    external_events: function () {
        /* initialize the external events
		-----------------------------------------------------------------*/
        $('#external-events div.external-event').each(function () {
            // create an Event Object (http://arshaw.com/fullcalendar/docs/event_data/Event_Object/)
            // it doesn't need to have a start or end
            var eventObject = {
                title: $.trim($(this).text()) // use the element's text as the event title
            };

            // store the Event Object in the DOM element so we can get to it later
            $(this).data('eventObject', eventObject);

            // make the event draggable using jQuery UI
            $(this).draggable({
                zIndex: 999,
                revert: true,      // will cause the event to go back to its
                revertDuration: 0  //  original position after the drag
            });
        });
    },

    // === Show error if no event name is provided === //
    show_error: function () {
        $('#modal-error').remove();
        $('<div style="border-radius: 5px; top: 70px; font-size:14px; left: 50%; margin-left: -70px; position: absolute;width: 140px; background-color: #f00; text-align: center; padding: 5px; color: #ffffff;" id="modal-error">Enter event name!</div>').appendTo('#modal-add-event .modal-body');
        $('#modal-error').delay('1500').fadeOut(700, function () {
            $(this).remove();
        });
    }


};



