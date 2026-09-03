$(document).ready(function () {

    function updateActiveText(isActive) {
        var activeText = document.getElementById("activeText");

        if (!activeText) {
            return;
        }

        activeText.textContent = isActive ? "Activo" : "Inactivo";

        activeText.classList.remove("text-success", "text-danger");
        activeText.classList.add(
            isActive ? "text-success" : "text-danger"
        );
    }

    $(document).on("change", "#active", function (event) {
        updateActiveText(event.target.checked);
    });

    var activeCheckbox = document.getElementById("active");

    if (activeCheckbox) {
        updateActiveText(activeCheckbox.checked);
    }

    const dictionary = {
        es: {
            messages: {
                confirmed: function () {
                    return "La contraseña no está confirmada.";
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

    VeeValidate.Validator.localize("en", dictionary.es);
    Vue.use(VeeValidate);

    var app = new Vue({
        el: "#content",

        data: {
            client: null,
            rates: [],
            loadingRates: false
        },

        methods: {

            getRates: function (event) {
                if (event) {
                    event.preventDefault();
                }

                var that = this;
                var clienteId = $("#cliente").val();
                var tarifaSelect = $("#tarifa");

                if (!clienteId || clienteId === "0") {
                    that.client = null;
                    that.rates = [];

                    tarifaSelect.html(
                        '<option value="">Seleccione un cliente primero</option>'
                    );

                    return;
                }

                that.loadingRates = true;

                tarifaSelect
                    .prop("disabled", true)
                    .html('<option value="">Cargando tarifas...</option>');

                $.ajaxSetup({
                    headers: {
                        "RequestVerificationToken": csrfToken
                    }
                });

                $.ajax({
                    type: "GET",
                    url: "/cupones/tarifas/" + clienteId,
                    cache: false,

                    success: function (data) {
                        try {
                            var responseData =
                                typeof data === "string"
                                    ? JSON.parse(data)
                                    : data;

                            that.client = responseData;
                            that.rates = responseData.Couponrates || [];

                            tarifaSelect.empty();

                            if (that.rates.length === 0) {
                                tarifaSelect.append(
                                    '<option value="">El cliente no tiene tarifas disponibles</option>'
                                );

                                Swal.fire({
                                    icon: "info",
                                    title: "Sin tarifas disponibles",
                                    text: "El cliente seleccionado no tiene tarifas de cupones registradas.",
                                    confirmButtonText: "Aceptar",
                                    confirmButtonColor: "#556ee6"
                                });

                                return;
                            }

                            tarifaSelect.append(
                                '<option value="">Seleccione una tarifa</option>'
                            );

                            $.each(that.rates, function (index, item) {
                                var valorFormateado = Number(
                                    item.Valor || 0
                                ).toLocaleString("es-MX", {
                                    style: "currency",
                                    currency: "MXN",
                                    minimumFractionDigits: 2,
                                    maximumFractionDigits: 2
                                });

                                var tipoCupon =
                                    item.Cupontype &&
                                        item.Cupontype.Name
                                        ? item.Cupontype.Name
                                        : "Sin tipo";

                                tarifaSelect.append(
                                    $("<option>", {
                                        value: item.CouponrateID,
                                        text:
                                            tipoCupon +
                                            " - " +
                                            valorFormateado +
                                            " MXN"
                                    })
                                );
                            });

                            console.log("Cliente seleccionado:", that.client);
                            console.log("Tarifas disponibles:", that.rates);

                        } catch (error) {
                            console.error(
                                "Error al interpretar las tarifas:",
                                error
                            );

                            tarifaSelect.html(
                                '<option value="">No fue posible cargar las tarifas</option>'
                            );

                            Swal.fire({
                                icon: "error",
                                title: "Respuesta no válida",
                                text: "No fue posible interpretar la información de las tarifas.",
                                confirmButtonText: "Aceptar",
                                confirmButtonColor: "#f46a6a"
                            });
                        }
                    },

                    error: function (response) {
                        console.error(
                            "Error al consultar las tarifas:",
                            response
                        );

                        tarifaSelect.html(
                            '<option value="">No fue posible cargar las tarifas</option>'
                        );

                        Swal.fire({
                            icon: "error",
                            title: "No fue posible cargar las tarifas",
                            text: "Ocurrió un error al consultar las tarifas del cliente.",
                            confirmButtonText: "Aceptar",
                            confirmButtonColor: "#f46a6a"
                        });
                    },

                    complete: function () {
                        that.loadingRates = false;
                        tarifaSelect.prop("disabled", false);
                    }
                });
            }
        }
    });

});