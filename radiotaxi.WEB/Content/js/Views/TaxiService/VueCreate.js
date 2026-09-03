$(document).ready(function () {
    var app = new Vue({
        el: '#content',
        data: {
            message : 'Hello from Vue!',
            services: {
                id: 0,
                date:null,
            },
            date : '',
            day : null, 
            month : null,
            year : null,
            id: 0,
            dateService: null,
            events: [],
            time:''
        },
        methods: {
            save: function(){
                $("[data-bs-dismiss=modal]").trigger({ type: "click" });
                this.time = $("#time").val();
                var date = new Date(this.date + " " + this.time);
                $("#datePickUp").val(this.date + " " + this.time);
            },
            gettime: function () {
                var that = this;
                var today = new Date();
                var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
                var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
                var dateTime = date + ' ' + time;
                return time;
            }
        },
        filters: {

        },
        mounted: function () {
            var that = this;
            var calendarEl = document.getElementById('calendar');
            var calendar = new FullCalendar.Calendar(calendarEl, {
                initialView: 'dayGridMonth',
                locale: 'es',
                dateClick: function (info) {
                    // change the day's background color just for fun
                    info.dayEl.style.backgroundColor = 'red';
                    // Obtenemos la fecha del evento.
                    that.services.date = info.date;
                    // abrir el modal para colocar la hora.
                    that.date = info.dateStr;
                    var myModal = new bootstrap.Modal(document.getElementById("staticBackdrop"), {});
                    $("#time").val(that.gettime(that.time));
                    myModal.show();
                }
            });
            calendar.render();
        }
    })
});