$(document).ready(function () {

    const dictionary = {
        es: {
            messages: {
                confirmed: function () {
                    return "La confirmación no coincide.";
                },
                email: function () {
                    return "Correo electrónico no válido.";
                },
                alpha: function () {
                    return "No se aceptan números ni caracteres especiales.";
                },
                required: function () {
                    return "Campo requerido.";
                },
                numeric: function () {
                    return "Debe ser numérico.";
                },
                min: function (target, value) {
                    return "El mínimo de caracteres debe ser de " + value + ".";
                },
                max: function (target, value) {
                    return "El máximo de caracteres debe ser de " + value + ".";
                },
                tel: function () {
                    return "Teléfono no válido.";
                }
            }
        }
    };

    VeeValidate.Validator.localize("es", dictionary.es);
    Vue.use(VeeValidate, {
        locale: "es"
    });

    var app = new Vue({
        el: "#content",

        data: {
            data: {
                hotels: [],
                Cupontypes: []
            },

            hotel: null,
            Cupontype: null,
            value: 0,

            grouscupons: [],

            loadingData: false,
            savingRate: false
        },

        methods: {

            saveRate: function (e) {
                e.preventDefault();
                var that = this;


                if (!that.hotel || !that.hotel.HotelID) {
                    Swal.fire({
                        icon: "warning",
                        title: "Hotel o hotel requerido",
                        text: "Selecciona un hotel o hotel para continuar."
                    });

                    return;
                }

                if (!that.Cupontype || !that.Cupontype.CupontypeID) {
                    Swal.fire({
                        icon: "warning",
                        title: "Cupontype de tarifa requerido",
                        text: "Selecciona un Cupontype de tarifa para continuar."
                    });

                    return;
                }

                var valueNumerico = Number(that.value);

                if (
                    that.value === null ||
                    that.value === "" ||
                    isNaN(valueNumerico)
                ) {
                    Swal.fire({
                        icon: "warning",
                        title: "value no válido",
                        text: "Ingresa un value numérico para la tarifa."
                    });

                    return;
                }

                if (valueNumerico < 0) {
                    Swal.fire({
                        icon: "warning",
                        title: "value no válido",
                        text: "El value de la tarifa no puede ser negativo."
                    });

                    return;
                }

                var Data = {
                    Valor: that.value,
                    HotelID: that.hotel.HotelID,
                    CupontypeID: that.Cupontype.CupontypeID
                };

                console.log("Hotel : ", that.hotel);
                console.log("Cupontype : ", that.Cupontype);

                var url = "/cupones/tarifa/crear/json/";
                $.ajaxSetup({
                    headers: { 'RequestVerificationToken': csrfToken }
                });
                $.ajax({
                    type: "post",
                    url: url,
                    data: Data,
                    context: this,
                    cache: false,
                    success: function (data) {
                        // Renderisamos la información
                        console.log("Response Success : ", data);
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

                var tarifa = {
                    id: that.generarIdTemporal(),
                    hotel: that.hotel,
                    Cupontype: that.Cupontype,
                    value: valueNumerico,
                    fechaCreacion: that.obtenerFechaActual()
                };


                that.limpiarFormulario();

                that.savingRate = false;

                Swal.fire({
                    icon: "success",
                    title: "Tarifa agregada",
                    text: "La tarifa de cupón se agregó correctamente.",
                    confirmButtonText: "Continuar"
                });
            },



            loaddata: function () {
                var that = this;
                var url = "/cupones/loaddata/";

                that.loadingData = true;

                if (typeof csrfToken !== "undefined") {
                    $.ajaxSetup({
                        headers: {
                            RequestVerificationToken: csrfToken
                        }
                    });
                }

                $.ajax({
                    type: "GET",
                    url: url,
                    cache: false,
                    success: function (response) {
                        try {
                            var parsedData;

                            if (typeof response === "string") {
                                parsedData = $.parseJSON(response);
                            } else {
                                parsedData = response;
                            }

                            that.data = parsedData || {
                                hotels: [],
                                Cupontypes: []
                            };

                            if (!Array.isArray(that.data.hotels)) {
                                that.data.hotels = [];
                            }

                            if (!Array.isArray(that.data.Cupontypes)) {
                                that.data.Cupontypes = [];
                            }

                            console.log("Información cargada:", that.data);

                        } catch (error) {
                            console.error(
                                "No fue posible interpretar la información:",
                                error
                            );

                            Swal.fire({
                                icon: "error",
                                title: "Error",
                                text: "No fue posible procesar la información de hoteles y Cupontypes de tarifa."
                            });
                        }
                    },

                    error: function (response) {
                        console.error("Error al cargar la información:", response);

                        Swal.fire({
                            icon: "error",
                            title: "Error",
                            text: "No fue posible cargar los hoteles y Cupontypes de tarifa."
                        });
                    },

                    complete: function () {
                        that.loadingData = false;
                    }
                });
            },

            
            remove: function (e, item) {
                if (e) {
                    e.preventDefault();
                }

                var that = this;

                Swal.fire({
                    icon: "warning",
                    title: "¿Eliminar tarifa?",
                    text: "La tarifa será eliminada del listado.",
                    showCancelButton: true,
                    confirmButtonText: "Sí, eliminar",
                    cancelButtonText: "Cancelar",
                    reverseButtons: true
                }).then(function (result) {

                    if (!result.isConfirmed) {
                        return;
                    }

                    that.grouscupons = that.grouscupons.filter(function (obj) {

                        if (item.id && obj.id) {
                            return item.id !== obj.id;
                        }

                        return !(
                            item.hotel &&
                            obj.hotel &&
                            item.Cupontype &&
                            obj.Cupontype &&
                            Number(item.hotel.HotelID) === Number(obj.hotel.HotelID) &&
                            Number(item.Cupontype.CupontypeID) === Number(obj.Cupontype.CupontypeID) &&
                            Number(item.value) === Number(obj.value)
                        );
                    });

                    Swal.fire({
                        icon: "success",
                        title: "Tarifa eliminada",
                        text: "La tarifa fue eliminada correctamente.",
                        timer: 1600,
                        showConfirmButton: false
                    });
                });
            },
            limpiarFormulario: function () {
                this.hotel = null;
                this.Cupontype = null;
                this.value = 0;

                if (this.$validator) {
                    this.$validator.reset();
                }
            },
            generarIdTemporal: function () {
                return (
                    Date.now().toString() +
                    Math.random().toString(16).substring(2)
                );
            },
            obtenerFechaActual: function () {
                var fecha = new Date();

                var dia = String(fecha.getDate()).padStart(2, "0");
                var mes = String(fecha.getMonth() + 1).padStart(2, "0");
                var anio = fecha.getFullYear();

                var horas = String(fecha.getHours()).padStart(2, "0");
                var minutos = String(fecha.getMinutes()).padStart(2, "0");

                return dia + "/" + mes + "/" + anio + " " + horas + ":" + minutos;
            },
            formatoMoneda: function (value) {
                var numero = Number(value);

                if (isNaN(numero)) {
                    numero = 0;
                }

                return numero.toLocaleString("es-MX", {
                    style: "currency",
                    currency: "MXN",
                    minimumFractionDigits: 2
                });
            }
        },

        mounted: function () {
            this.loaddata();
        }
    });

});