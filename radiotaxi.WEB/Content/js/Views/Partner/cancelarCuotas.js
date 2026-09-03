$(document).ready(function () {
    const dictionary = {
        es: {
            messages: {
                _default: (field) => `El campo  no es válido`,
                after: (field, [target, inclusion]) => `El campo  debe ser posterior ${inclusion ? 'o igual ' : ''}a ${target}`,
                alpha: (field) => `El campo  solo debe contener letras`,
                alpha_dash: (field) => `El campo  solo debe contener letras, números y guiones`,
                alpha_num: (field) => `El campo  solo debe contener letras y números`,
                alpha_spaces: (field) => `El campo  solo debe contener letras y espacios`,
                before: (field, [target, inclusion]) => `El campo  debe ser anterior ${inclusion ? 'o igual ' : ''}a ${target}`,
                between: (field, [min, max]) => `El campo  debe estar entre ${min} y ${max}`,
                confirmed: (field) => `El campo  no coincide`,
                credit_card: (field) => `El campo  es inválido`,
                date_between: (field, [min, max]) => `El campo  debe estar entre ${min} y ${max}`,
                date_format: (field, [format]) => `El campo  debe tener un formato ${format}`,
                decimal: (field, [decimals = '*'] = []) => `El campo  debe ser numérico y contener${!decimals || decimals === '*' ? '' : ' ' + decimals} puntos decimales`,
                digits: (field, [length]) => `El campo  debe ser numérico y contener exactamente ${length} dígitos`,
                dimensions: (field, [width, height]) => `El campo  debe ser de ${width} píxeles por ${height} píxeles`,
                email: (field) => `El campo  debe ser un correo electrónico válido`,
                excluded: (field) => `El campo  debe ser un valor válido`,
                ext: (field) => `El campo  debe ser un archivo válido`,
                image: (field) => `El campo  debe ser una imagen`,
                included: (field) => `El campo  debe ser un valor válido`,
                integer: (field) => `El campo debe ser un entero`,
                ip: (field) => `El campo  debe ser una dirección ip válida`,
                length: (field, [length, max]) => {
                    if (max) {
                        return `El largo del campo  debe estar entre ${length} y ${max}`;
                    }

                    return `El largo del campo  debe ser ${length}`;
                },
                max: (field, [length]) => `El campo  no debe ser mayor a ${length} caracteres`,
                max_value: (field, [max]) => `El campo  debe de ser ${max} o menor`,
                mimes: (field) => `El campo  debe ser un tipo de archivo válido`,
                min: (field, [length]) => `El campo  debe tener al menos ${length} caracteres`,
                min_value: (field, [min]) => `El campo  debe ser ${min} o superior`,
                numeric: (field) => `El campo  debe contener solo caracteres numéricos`,
                regex: (field) => `El formato del campo  no es válido`,
                required: (field) => `El campo  es obligatorio`,
                size: (field, [size]) => `El campo  debe ser menor a ${formatFileSize(size)}`,
                url: (field) => `El campo  no es una URL válida`
            }
        }
    };

    VeeValidate.Validator.localize('en', dictionary.es);
    r = VeeValidate.Validator;
    Vue.component('validation-provider', VeeValidate.ValidationProvider);
    Vue.use(VeeValidate, {
        useConstraintAttrs: false
    });
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
            },
        },
        methods: {
            onSubmit(e) {
                var that = this;
                this.$validator.validateAll().then(function (response) {
                    if (response === true) {
                        console.log("Todo bien Enviar");
                    } else {
                        e.preventDefault();
                        console.log("La hemos cagado");
                        Swal.fire({
                            icon: "error",
                            title: "Datos incorrectos",
                            text: "¡Compruebe que todos los campos son correctos!",
                            footer: '<span>rellene los campos en color rojo</span>'
                        });
                    }
                }).catch(function (e) { });
            },
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
            sendform: function () {
                $("#content").on("submit", "form", function (e) {
                    e.preventDefault();
                });

                $("#SendForm").on("click", function (e) {
                    var that = this;

                });
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
            // Metodos del formulario mamon de german.
            next: async function (target) {
                // Eliminar todas las tabs activas
                $(".tab-pane.active").removeClass("active");
                $("#" + target).addClass("active");
                this.processbar();
            },
            prev: function (target) {
                // Eliminar todas las tabs activas
                $(".tab-pane.active").removeClass("active");
                $("#" + target).addClass("active");
                this.processbar();
            },
            processbar: function () {
                var s = $("#content").find(".tab-pane.active");
                var x = $("#content").find(".tab-pane");
                var i = x.index(s);
                x = x.length;
                i = ((i + 1) / x) * 100;
                $("#progrss-wizard")
                    .find(".progress-bar")
                    .css({ width: i + "%" });
            },
            setAndFilter: function (i) {
                var r = i.target.value;
                i.target.value = r.replace(/[^0-9]/g, "").replace(/(\d{3})(\d{3})(\d{4,4})/, "($1) $2 $3").substring(0, 14);
            },
        }
    })
});