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
    //////////////////////////////////////////////////////////////////////////////////////
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
            taxi: '',
            data: null,
            selected: 3,
            renderiframe : null,
        },
        methods: {
            searchCar: function (){
                var that = this;
                $.ajaxSetup({
                    headers: { 'RequestVerificationToken': csrfToken }
                });
                $.ajax({
                    type: "POST",
                    url: "/Operator/SearchCarCreate/",
                    data: {
                        gafet: that.gafet,
                        taxi: that.taxi
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
                    url: "/Operator/SaveCard/",
                    data: {
                        taxi: that.taxi,
                        turn: that.selected,
                        op: _op,
                    },
                    context: this,
                    cache: false,
                    success: function (data){
                        that.renderiframe = true;
                        // Renderisamos la información
                        var src = "/card/generatepdf/" + data.id;
                        $("#iframe1").attr("src", src);
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
                        that.renderiframe = null;
                    }
                });
            }
        },
        filters: {

        },
        mounted: function () {
            var that = this;
            that.renderiframe = null;
        }
    })
});