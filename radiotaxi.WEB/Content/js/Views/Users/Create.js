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
            errorEmail: false,
            errorPassword: false,
            states: [],
            cities: [],
            message: 'New Padron Admin',
            data: null,
            reports: {},
            render: false,
            CurrentPage: null,
            NumberOfPages: null,
            SearchModel: {
                SearchpartnerReference: '',
                SearchfirstName: '',
                SearchlastNameF: '',
                SearchlastNameM: '',
                sm: '',
                mz: '',
                lt: '',
                zip: '',
                state: '',
                city: '',
                agemin: '',
                agemax: '',
                page: 1,
                pagesize: 20
            }
        },
        filters: {
            capitalize: function (value) {
                if (!value) return ''
                value = value.toString()
                return value.charAt(0).toUpperCase() + value.slice(1)
            },
            DateFormat: function (value) {
                var date = value.toString();
                date = date.replace("/Date(", "");
                date = date.replace(")/", "");
                // Create a new JavaScript Date object based on the timestamp
                // multiplied by 1000 so that the argument is in milliseconds, not seconds.
                date = new Date(parseInt(date));
                var months = ['En', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dec'];
                var year = date.getFullYear();
                var month = months[date.getMonth()];
                var DateS = date.getDate();
                var hour = date.getHours();
                var min = date.getMinutes();
                var sec = date.getSeconds();
                var time = DateS + ' ' + month + ' ' + year;
                return time;
            },
            HourFormat: function (value) {
                var date = value.toString();
                date = date.replace("/Date(", "");
                date = date.replace(")/", "");
                date = new Date(parseInt(date));
                var months = ['En', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dec'];
                var year = date.getFullYear();
                var month = months[date.getMonth()];
                var DateS = date.getDate();
                var hour = date.getHours();
                var min = date.getMinutes();
                var sec = date.getSeconds();
                var time = hour + ':' + min + ':' + sec;
                var dateSplit = date.toString().split(" ");
                return dateSplit[4];
            }
        },
        methods: {
            clickCallback: function (pageNum) {
                var that = this;
                $.ajaxSetup({
                    headers: { 'RequestVerificationToken': csrfToken }
                });
                $.ajax({
                    type: "POST",
                    url: "/Reports/index/",
                    data: { page: pageNum },
                    context: this,
                    cache: false,
                    success: function (data) {
                        that.reports = data.reports;
                        that.NumberOfPages = data.paginModel.TotalPages;
                        that.CurrentPage = data.paginModel.CurrentPage;
                    },
                    complete: function (response) {
                    }
                });
            },
            confirmEmail: function (target) {
                // Validamos si los correos son iguales.
                var Email = $("#Email").val();
                var Emailconfirm = $("#Emailconfirm").val();
                if (Email != Emailconfirm) {
                    $(".Emailconfirm.has-error").html("El correo no coincide.");
                    errorEmail = true;
                } else {
                    $(".Emailconfirm.has-error").html("");
                    errorEmail = false;
                }
            },
            confirmPassword: function (target) {
                // Validamos si los correos son iguales.
                var Password = $("#Password").val();
                var Passwordconfirm = $("#Passwordconfirm").val();
                if (Password != Passwordconfirm) {
                    $(".confirmPassword.has-error").html("La contraseña no coincide.");
                    errorPassword = true;
                } else {
                    $(".confirmPassword.has-error").html("");
                    errorPassword = false;
                }
            },
            submit: function (e){
                this.$validator.validateAll().then(function (response) {
                    if (response === true) {
                        //console.log("Todo bien Enviar");
                    } else {
                        e.preventDefault();
                        //console.log("La hemos cagado");
                    }
                }).catch(function (e) { });
            }
        },
        mounted: function () {
            var that = this;
            that.clickCallback(1);
            $('.input-ex-1').generatePassword({
                passCharachterSet: '0-9',
                passSize: "8"
            });
        }
    })
});