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
                integer: (field) => `El campo  debe ser un entero`,
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

    Date.prototype.parseDate = function (separator) {
        let year = new Intl.DateTimeFormat('en', { year: 'numeric' }).format(this);
        let month = new Intl.DateTimeFormat('en', { month: 'numeric' }).format(this);
        let day = new Intl.DateTimeFormat('en', { day: '2-digit' }).format(this);
        return (`${year}-${month}${separator}${day}`);
    }

    VeeValidate.Validator.localize('en', dictionary.es);
    r = VeeValidate.Validator;
    Vue.component('validation-provider', VeeValidate.ValidationProvider);

    Vue.use(VeeValidate);
    var app = new Vue({
        el: '#content',
        data: {
            loading: false,
            bloqueos: null,
            desbloqueos: null,
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
            removeMessage: function (folio, reference) {
                $.ajaxSetup({
                    headers: { 'RequestVerificationToken': csrfToken }
                });

                $.ajax({
                    type: "POST",
                    url: "/Partner/desbloquear/" + reference + "/" + folio,
                    data: {
                        id: this.value,
                    },
                    context: this,
                    cache: false,
                    success: function (data) {
                        // Renderisamos la información
                        console.table(data);
                        if (data.response == true) {
                            Swal.fire({
                                position: "top-end",
                                icon: "success",
                                title: "Se elimino el bloqueo de manera correcta.",
                                showConfirmButton: false,
                                timer: 2000
                            });
                            location.reload();
                        } else {
                            Swal.fire({
                                position: "top-end",
                                icon: "error",
                                title: "No puede eliminar el bloqueo, no cuenta con los permisos necesarios.",
                                showConfirmButton: false,
                                timer: 3000
                            });
                        }

                        /*$('.ciudad').empty();
                        // Iteramos sobre los datos recibidos y creamos opciones para el select
                        $.each(JSON.parse(data), function (index, city) {
                            $('.ciudad').append($('<option>', {
                                value: city.id,
                                text: city.name
                            }));
                        });*/
                    },
                    complete: function (response) {
                    },
                    error: function (response) {
                        console.log("Error : ");
                        console.log(response);
                    }
                });

            }
        },
        mounted: function () {
            var that = this;

            that.bloqueos = new DataTable("#tbloqueos", {
                layout: {
                    topStart: {
                        buttons: ['excelHtml5']
                    }
                }
            });
            that.desbloqueos = new DataTable("#tdesbloqueos", {
                layout: {
                    topStart: {
                        buttons: ['excelHtml5']
                    }
                }
            });
        },
    })
});