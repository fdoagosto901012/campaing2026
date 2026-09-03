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
            data: [],
            selected: 3,


            // Estos son los elementos para hacer la busqueda.
            cajera: 'Cajera 1',
            name: '',
            lastname: '',
            reference: '',
            pago: '100',
            Cuppons: [],
            total: 0,
            user : null,
        },
        methods: {
            IdentityReturn: function () {
                var that = this;
                Swal.fire({
                    title: "¡Regresar identidad!",
                    text: "",
                    icon: "warning",
                    html: ``
                    ,
                    showCancelButton: true,
                    confirmButtonColor: "#3085d6",
                    cancelButtonColor: "#d33",
                    confirmButtonText: "Continuar"
                });
            },
            addCoupons: function (e) {
                e.preventDefault();
                // Obtenemos los valores de sector
                var sector = $("#sector").val();
                // Obtenemos los valores de perforacion
                var perforacion = $("#perforacion").val();

                this.Cuppons.push(
                    {
                        reference: "8624",
                        sector: sector,
                        numero: "13500",
                        perforacion: perforacion,
                        pago: 1400,
                        cajera: "Cristel Herrera"

                    }
                );
                this.calculate();
            },
            calculate: function () {
                var that = this;
                that.total = 0;
                that.Cuppons.forEach(function (Cuppon, index) {
                    that.total = that.total + Cuppon.pago;
                });
            },
            search: function () {
                var that = this;
                $.ajaxSetup({
                    headers: { 'RequestVerificationToken': csrfToken }
                });
                $.ajax({
                    type: "POST",
                    url: "/Operator/search/",
                    data: {
                        reference: that.reference,
                        name: that.name,
                        lastname: that.lastname
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
            add: function (index) {
                console.log("INDEX : " + index);
                var that = this;
                that.user = that.data.Data[index];
                console.log(that.user);
            },
            searchTicket: function () {
                // Buscar tickets 
                var sector = $("#sector").val();
                var perforacion = $("#perforacion").val();

                console.log("Sector : " + sector);
                console.log("perforacion : " + perforacion);

                var that = this;
                $.ajaxSetup({
                    headers: { 'RequestVerificationToken': csrfToken }
                });
                $.ajax({
                    type: "POST",
                    url: "/Coupons/search/",
                    data: {
                        sector: sector,
                        perforacion: perforacion
                    },
                    context: this,
                    cache: false,
                    success: function (data) {
                        if (data.length <= 0) {
                            Swal.fire({
                                title: 'Mensaje',
                                text: 'No se encontro el cupon',
                                icon: 'info',
                                confirmButtonText: 'Ok'
                            });
                        } else {
                            Swal.fire({
                                title: 'Mensaje',
                                text: message,
                                icon: 'info',
                                confirmButtonText: 'Ok'
                            });
                        }

                        console.log(data);
                    },
                    complete: function (response) {
                        console.log(response);

                    },
                    error: function (response) {
                        console.log(response);
                    }
                });

            }
        },
        filters: {

        },
        mounted: function () {
            var that = this;
        }
    })
});