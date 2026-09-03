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
            user: null,
            client: null,
            rates: [],
            grouscupons: [],
            tarifas : null,
        },
        methods: {
            getRates: function (e) {
                e.preventDefault();
                // Obtenemos los valores de sector
                var cliente = $("#cliente").val();
                var that = this;
                var url = "/cupones/tarifas/" + cliente;
                $.ajaxSetup({
                    headers: { 'RequestVerificationToken': csrfToken }
                });
                $.ajax({
                    type: "get",
                    url: url,
                    context: this,
                    cache: false,
                    success: function (data) {
                        // Renderisamos la información
                        that.data = data;
                        that.client = JSON.parse(data);
                        let html = "";
                        $.each(that.client.Couponrates, function (index, item) {
                            html += "<option value='" + item.CouponrateID + "'>" + item.Cupontype.Name + "- $" + item.Valor + "</option>"
                        });
                        $("#tarifa").html(html);
                        // Marcar con un alert que no se encontro datos y limpiar
                        console.log(that.client);
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
            addCuponsGroup: function (e) {
                console.log("Agregar cupones");
                var that = this;
                let rate = null;
                // Obtener el objeto de la tarifa.
                $.each(that.client.Couponrates, function (index, item) {
                    if (item.CouponrateID == $("#tarifa").val()) {
                        rate = item;
                    }
                });
                let objeto = {
                    cant: $("#cant").val(),
                    clienteid: that.client.HotelID,
                    clientename: that.client.Name,
                    tarifa: rate.CouponrateID,
                    tarifaObj: rate
                };
                that.grouscupons.push(objeto);
                console.log(that.grouscupons);
            },
            removeOfGroup: function (e, index) {
                let that = this;
                that.grouscupons.splice(index,1);
            },
            createGroups: function (e) {
                e.preventDefault();
                console.log("createGroups");
                // Obtenemos los valores de sector
                var cliente = $("#cliente").val();
                var that = this;
                console.log(that.grouscupons);
                var url = "/cupones/crear/";
                $.ajaxSetup({
                    headers: { 'RequestVerificationToken': csrfToken }
                });
                $.ajax({
                    type: "post",
                    url: url,
                    data: {
                        objects: that.grouscupons
                    },
                    context: this,
                    cache: false,
                    success: function (data){
                        // Renderisamos la información
                        that.data = data;
                        that.client = JSON.parse(data);
                        let html = "";
                        $.each(that.client.Couponrates, function (index, item) {
                            html += "<option value='" + item.CouponrateID + "'>" + item.Cupontype.Name + "- $" + item.Valor + "</option>"
                        });
                        $("#tarifa").html(html);
                        // Marcar con un alert que no se encontro datos y limpiar
                        console.log(that.client);
                        // similar behavior as clicking on a link
                        window.location.href = "/cupones/impresion";
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
            }
        },
        filters: {

        },
        mounted: function () {
            var that = this;
        }
    })
});