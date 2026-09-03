$(document).ready(function () {

    const dictionary = {
        es: {
            messages: {
                confirmed: function () {
                    return "Your password is not confirmed."
                },
                email: function () {
                    return "Email no valido."
                },
                alpha: function () {
                    return "No se aceptan números, ni caracteres especiales."
                },
                required: function () {
                    return "Campo requerido."
                },
                numeric: function () {
                    return "Debe ser númerico.";
                },
                min: function (target, value) {
                    return "El minímo de carácteres debe ser de " + value + ".";
                },
                max: function (target, value) {
                    return "El máximo de carácteres debe ser de " + value + "."
                },
                tel: function () {
                    return "Teléfono invalido.";
                }
            }
        }
    };
    VeeValidate.Validator.localize('en', dictionary.es);

    Vue.use(VeeValidate);

    var app = new Vue({
        el: '#content',
        data: {
            message: 'Hello from Vue!',
            services: {
                id: 0,
                date: null,
            },
            date: '',
            day: null,
            month: null,
            year: null,
            id: 0,
            dateService: null,
            events: [],
            time: '',
            gafet: '',
            data: null,
            selected: 3,
        },
        methods: {
            searchCar: function () {
                console.log("SearchCar");
                var that = this;
                $.ajaxSetup({
                    headers: { 'RequestVerificationToken': csrfToken }
                });

                $.ajax({
                    type: "POST",
                    url: "/Partner/SearchCar/",
                    data: {
                        gafet: that.gafet
                    },
                    context: this,
                    cache: false,
                    success: function (data) {
                        // Renderisamos la información
                        that.data = data;
                        console.log(data);
                        // Marcar con un alert que no se encontro datos y limpiar
                    },
                    complete: function (response) {
                        console.log("Complete : ");
                        console.log(response);
                    },
                    error: function (response) {
                        console.log("Error : ");
                        console.log(response);
                        // Marcar con un alert que no se encontro datos y limpiar
                    }
                });
            },
            saveCard: function () {
                var that = this;
                var _op = $("#gafet").val();
                console.log("Taxi : " + that.gafet);
                console.log("Turno  : " + that.selected);
                console.log("Operador  : " + _op);
                console.log("Guardar tarjeton.");
                $.ajaxSetup({
                    headers: { 'RequestVerificationToken': csrfToken }
                });

                $.ajax({
                    type: "POST",
                    url: "/Partner/SaveCard/",
                    data: {
                        taxi: that.gafet,
                        turn: that.selected,
                        op: _op,
                    },
                    context: this,
                    cache: false,
                    success: function (data) {
                        // Renderisamos la información
                        //that.data = data;
                        //console.log(data);
                        var fechaAsigned = new Date(Date.parse(data.asignedDate));
                        var dia = fechaAsigned.getDate().toString().padStart(2, '0');
                        var mes = (fechaAsigned.getMonth() + 1).toString().padStart(2, '0');
                        var año = fechaAsigned.getFullYear().toString();
                        var fechaFormateadaas = dia + "/" + mes + "/" + año;

                        var fechaExpiration = new Date(Date.parse(data.expiration_date));
                        var dia = fechaExpiration.getDate().toString().padStart(2, '0');
                        var mes = (fechaExpiration.getMonth() + 1).toString().padStart(2, '0');
                        var año = fechaExpiration.getFullYear().toString();
                        var fechaFormateadaex = dia + "/" + mes + "/" + año;

                        var html = "<tr>" +
                            " <td  data-org-colspan=\"1\" data-priority=\"1\" data-columns=\"tech-companies-1-col-0\"> " + data.taxi + " </td> " +
                            " <td data-org-colspan=\"1\" data-priority=\"3\" data-columns=\"tech-companies-1-col-1\"> " + fechaFormateadaas /*(new Date(Date.parse(data.asignedDate)).toString("dd/MM/yyyy")) */ + "</td> " +
                            " <td data-org-colspan=\"1\" data-priority=\"1\" data-columns=\"tech-companies-1-col-2\"> " + fechaFormateadaex  /*(new Date(data.expiration_date)).Tostring()*/ + "</td> " +
                            " <td data-org-colspan=\"1\" data-priority=\"3\" data-columns=\"tech-companies-1-col-3\"> " + (data.turn == 3 ? "Ambos" : data.turn == 2 ? "Vespertino" : data.turn == 1 ? "Matutino" : "") + " </td> " +
                            " <td data-org-colspan=\"1\" data-priority=\"3\" data-columns=\"tech-companies-1-col-4\"> " +
                            " <a href=\"/card/edit/" + data.taxi + "\" class=\"btn btn-success waves-effect btn-label waves-light\"> <i class=\"bx bx-edit-alt label-icon\"></i> Editar</a> " +
                            " <a href=\"/card/details/" + data.taxi + "\" class=\"btn btn-success waves-effect btn-label waves-light\"> <i class=\"bx bx-detail label-icon\"></i> Detalles </a> " +
                            " </td> " +
                            "</tr> ";

                        $("#t-content").append(html);
                        Swal.fire({
                            title: "Tarjetón generado!",
                            text: "Exitosamente!",
                            icon: "success"
                        });
                    },
                    complete: function (response) {
                        console.log("Complete : ");
                        console.log(response);
                    },
                    error: function (response) {
                        console.log("Error : ");
                        console.log(response);
                    }
                });
            },
            showAlert: function (message) {
                alert(message);
                Swal.fire({
                    title: 'Mensaje',
                    text: message,
                    icon: 'info',
                    confirmButtonText: 'Ok'
                });
            },
            generateTicket: function (type, reference){
                console.log("Turkey");
                var that = this;
                var note = $("#note").val();
                $.ajaxSetup({
                    headers: { 'RequestVerificationToken': csrfToken }
                });
                $.ajax({
                    type: "POST",
                    url: "/Turkey/printWEB/" + reference + "/" + type,
                    data:{
                        note:note      
                    },
                    context: this,
                    cache: false,
                    success: function (data) {
                        console.log(data);
                        var newWin = window.open('', 'Print-Window');
                        newWin.document.open();
                        newWin.document.write(data);
                        newWin.document.close();
                        setTimeout(function () {
                            newWin.close();
                            location.reload();
                        }, 10);
                    },
                    complete: function (response) {
                        console.log("Complete : ");
                        console.log(response);
                    },
                    error: function (response) {
                        console.log("Error : ");
                        console.log(response);
                    }
                });
            }
        },
        filters: {

        },
        mounted: function () {
            var that = this;
        },



    })
});