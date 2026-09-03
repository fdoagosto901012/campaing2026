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
            // Estos son los elementos para hacer la busqueda.
            cajera: 'Cajera 1',
            name: '',
            lastname: '',
            reference: '',
            pago: '100',
            Cuppons: [],
            CupponsPrinted: [],
            CupponsPrePrint: [],
            CupponsTemp: [],
            total: 0,
            user: null,
            orders: []
        },
        methods: {
            load: function () {
                var that = this;
                $.ajaxSetup({
                    headers: { 'RequestVerificationToken': csrfToken }
                });
                $.ajax({
                    type: "POST",
                    url: "/cupones/all/",
                    data: {
                    },
                    context: this,
                    cache: false,
                    success: function (data) {
                        // Renderisamos la información
                        that.data = $.parseJSON(data);
                        console.log(that.data);
                        that.Cuppons = that.data.NOcuponsGroupPrinteds;
                        that.CupponsPrinted = that.data.cuponsGroupPrinteds;
                        that.orders = that.data.orders;
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
            preprint: function (event, group) {
                // Se obtiene el objeto de
                event.preventDefault()
                console.log(group);
                var that = this;
                // Eliminamos el grupo de la lista de cupones creados.
                that.CupponsTemp = that.Cuppons.filter(function (g) {
                    return g.Id !== group.Id;
                });
                that.Cuppons = that.CupponsTemp;
                that.CupponsPrePrint.push(group);
            },
            cancel: function (event, group) {
                // Se obtiene el objeto de
                event.preventDefault();
                console.log(group);
                var that = this;
                // Eliminamos el grupo de la lista de cupones creados.
                var temp = that.CupponsPrePrint.filter(function (g) {
                    return g.Id !== group.Id;
                });
                that.CupponsPrePrint = temp;
                that.Cuppons.push(group);
            },
            print: function (event) {
                var that = this;
                event.preventDefault();
                // Enviamos toda la seleccion para impresion...
                var groupsCopy = JSON.parse(JSON.stringify(that.CupponsPrePrint));
                console.log("print send groupsCopy : ", groupsCopy);
                $.each(groupsCopy, function (index, item) {
                    item.Cupons = [];
                });
                $.ajaxSetup({
                    headers: { 'RequestVerificationToken': csrfToken }
                });
                $.ajax({
                    type: "POST",
                    url: "/cupones/printedaction/",
                    data: {
                        group: groupsCopy
                    }, // Enviamos el array para poder registrar la Impresion...
                    context: this,
                    cache: false,
                    success: function (data) {
                        // Renderisamos la información
                        var data = $.parseJSON(data);
                        console.log("Response de print: ", data);
                        if (data.status == 200) {

                            $.each(that.CupponsPrePrint, function (index, item) {
                                that.CupponsPrinted.push(item);
                            });

                            that.CupponsPrePrint = [];

                            Swal.fire({
                                icon: 'success',
                                title: '¡Impresión enviada!',
                                text: 'Los cupones fueron enviados correctamente a la cola de impresión.',
                                confirmButtonText: 'Aceptar',
                                confirmButtonColor: '#34c38f'
                            });

                        } else {

                            Swal.fire({
                                icon: 'warning',
                                title: 'No fue posible imprimir',
                                text: data.message,
                                confirmButtonText: 'Aceptar',
                                confirmButtonColor: '#556ee6'
                            });

                        }
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
            deliver: function (event, group){
                groupT = group;
                groupT.Cupons = [];
                event.preventDefault();
                // Enviamos toda la seleccion para impresion...
                var that = this;
                $.ajaxSetup({
                    headers: { 'RequestVerificationToken': csrfToken }
                });
                $.ajax({
                    type: "POST",
                    url: "/cupones/deliveraction/",
                    data: {
                        group: groupT
                    }, // Enviamos el array para poder registrar la Impresion...
                    context: this,
                    cache: false,
                    success: function (data) {
                        // Renderisamos la información
                        var data = $.parseJSON(data);
                        // recorremos todos los elementos regresados para retirarlos de la lista de preimpresion.  
                        var temp = that.CupponsPrinted.filter(function (g) {
                            return g.Id !== data.Id;
                        });
                        that.CupponsPrinted = temp;
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
            disableorder: function (event, key) {
                event.preventDefault();
                var that = this;
                $.ajaxSetup({
                    headers: { 'RequestVerificationToken': csrfToken }
                });
                $.ajax({
                    type: "GET",
                    url: "/cupones/order/finish/" + key,
                    data: {
                    },
                    context: this,
                    cache: false,
                    success: function (data){
                        var data = $.parseJSON(data);
                        Vue.delete(that.orders, key);
                        console.log(that.orders);
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
            formatDate: function (value) {
                if (!value) return '';
                const date = new Date(value);
                const day = String(date.getDate()).padStart(2, '0');
                const month = String(date.getMonth() + 1).padStart(2, '0');
                const year = date.getFullYear();
                const hours = String(date.getHours()).padStart(2, '0');
                const minutes = String(date.getMinutes()).padStart(2, '0');
                return `${day}-${month}-${year} ${hours}:${minutes}`;
            },
            truncate: function (data, num) {
                const reqdString =
                    data.split("").slice(0, num).join("");
                return reqdString+"...";
            }
        },
        mounted: function () {
            var that = this;
            that.load();
        }
    })
});