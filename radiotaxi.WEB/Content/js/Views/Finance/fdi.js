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

    function formatDate(date) {
        if (!date) return "";

        const parts = date.split("-");

        if (parts.length !== 3)
            return date;

        return `${parts[2]}/${parts[1]}/${parts[0]}`;
    }

    DataTable.ext.type.order['producto-pre'] = function (data) {
        if (!data) return '';

        return data
            .toString()
            .split('.')
            .map(x => x.padStart(6, '0'))
            .join('');
    };

    VeeValidate.Validator.localize('en', dictionary.es);
    r = VeeValidate.Validator;
    Vue.component('validation-provider', VeeValidate.ValidationProvider);

    Vue.use(VeeValidate);
    var app = new Vue({
        el: '#content',
        data: {
            initDateSoc: null,
            endDateSoc: null,
            test: "Hola mundo",
            loading: false,
            table: null,
            t2: null,
            socs: [],
            socMesTotal: 0,
            socTotal: 0,

            ops: [],
            opMesTotal: 0,
            opTotal: 0,
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
                date = new Date(parseInt(date));
                var months = ['En', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dec'];
                var year = date.getFullYear();
                var month = months[date.getMonth()];
                var DateS = date.getDate();
                var time = DateS + ' ' + month + ' ' + year;
                return time;
            },
            HourFormat: function (value) {
                var date = value.toString();
                date = date.replace("/Date(", "");
                date = date.replace(")/", "");
                date = new Date(parseInt(date));
                var dateSplit = date.toString().split(" ");
                return dateSplit[4];
            },
        },
        methods: {
            onSubmitSOC(e) {
                var that = this;
                e.preventDefault();
                that.initDateSoc = $("#initDateSoc").val();
                that.endDateSoc = $("#endDateSoc").val();

                that.$validator.validateAll().then(function (response) {
                    if (response === true && that.loading === false) {
                        that.loading = true;
                        console.log("Todo bien Enviar");
                        $.ajaxSetup({
                            headers: { 'RequestVerificationToken': csrfToken }
                        });
                        $.ajax({
                            type: "POST",
                            url: "/finance/fdi_soc/",
                            data: {
                                start: that.initDateSoc,
                                end: that.endDateSoc
                            },
                            context: this,
                            cache: false,
                            success: function (data) {
                                console.log("Sucess");
                                console.table(data);

                                data.map((item) => {
                                    that.socMesTotal += item.IMPORTE_MES;
                                    that.socTotal += item.IMPORTE;
                                });

                                let counter = 0;
                                console.table(data);

                                data.map((soc) => {
                                    that.table.row
                                        .add([
                                            soc.Id_Producto,
                                            soc.socio,
                                            soc.nombre,
                                            soc.IMPORTE_MES.toLocaleString('es-MX', { style: 'currency', currency: 'MXN' }),
                                            soc.IMPORTE.toLocaleString('es-MX', { style: 'currency', currency: 'MXN' })
                                        ])
                                        .draw(true);
                                    counter++;
                                });

                                console.log(that.socMesTotal);
                                console.log(that.socTotal);
                            },
                            complete: function (response) {
                                console.log("Complete");
                                that.loading = false;
                            },
                            error: function (response) {
                                console.log("Error");
                                console.log(response);
                            }
                        });
                    } else {
                        console.log("La hemos cagado");
                        Swal.fire({
                            icon: "error",
                            title: "Datos incorrectos",
                            text: "¡Compruebe que todos los campos son correctos!",
                            footer: '<span>rellene los campos en color rojo</span>'
                        });
                    }
                }).catch(function (e) {
                    console.log(e);
                });
            },
            onSubmitOP(e) {
                var that = this;
                e.preventDefault();
                that.initDateOp = $("#initDateOp").val();
                that.endDateOp = $("#endDateOp").val();

                that.$validator.validateAll().then(function (response) {
                    if (response === true && that.loading === false) {
                        that.loading = true;
                        console.log("Todo bien Enviar");
                        $.ajaxSetup({
                            headers: { 'RequestVerificationToken': csrfToken }
                        });
                        $.ajax({
                            type: "POST",
                            url: "/finance/fdi_chof/",
                            data: {
                                start: that.initDateOp,
                                end: that.endDateOp
                            },
                            context: this,
                            cache: false,
                            success: function (data) {
                                console.log("Sucess OP");
                                console.table(data);

                                data.map((item) => {
                                    that.opMesTotal += item.IMPORTE_MES;
                                    that.opTotal += item.IMPORTE;
                                });

                                that.ops = data;
                                let counter2 = 0;
                                console.table(data);

                                data.map((ops) => {
                                    that.t2.row
                                        .add([
                                            ops.Id_Producto,
                                            ops.socio,
                                            ops.nombre,
                                            ops.IMPORTE_MES.toLocaleString('es-MX', { style: 'currency', currency: 'MXN' }),
                                            ops.IMPORTE.toLocaleString('es-MX', { style: 'currency', currency: 'MXN' })
                                        ])
                                        .draw(true);
                                    counter2++;
                                });
                            },
                            complete: function (response) {
                                console.log("Complete");
                                that.loading = false;
                            },
                            error: function (response) {
                                console.log("Error");
                                console.log(response);
                            }
                        });
                    } else {
                        console.log("La hemos cagado");
                        Swal.fire({
                            icon: "error",
                            title: "Datos incorrectos",
                            text: "¡Compruebe que todos los campos son correctos!",
                            footer: '<span>rellene los campos en color rojo</span>'
                        });
                    }
                }).catch(function (e) {
                    console.log(e);
                });
            },
        },
        mounted: function () {
            var that = this;
            that.initDateSoc = new Intl.DateTimeFormat("az").format((new Date()).addDays(-7));
            that.endDateSoc = new Intl.DateTimeFormat("az").format((new Date()));
            $("#initDateSoc").val(that.initDateSoc);
            $("#endDateSoc").val(that.endDateSoc);
            $("#initDateOp").val(that.initDateSoc);
            $("#endDateOp").val(that.endDateSoc);

            that.table = new DataTable("#tsocios", {
                layout: {
                    topStart: {
                        buttons: [
                            {
                                extend: 'excelHtml5',
                                title: function () {

                                    let inicio = formatDate($('#initDateSoc').val());
                                    let fin = formatDate($('#endDateSoc').val());

                                    return `Fondo de Defunción e Invalidez de Socios | ${inicio} al ${fin} | Practicontrol Web`;
                                },
                                filename: function () {

                                    let inicio = formatDate($('#initDateSoc').val()).replace(/\//g, "-");
                                    let fin = formatDate($('#endDateSoc').val()).replace(/\//g, "-");

                                    return `FDI_Socios_${inicio}_al_${fin}`;
                                }
                            }
                        ]
                    }
                },
                columnDefs: [
                    { width: '10%', targets: 0, className: 'text-center', type: 'producto' },
                    { width: '10%', targets: 1, className: 'text-center' },
                    { width: '30%', targets: 2, className: 'text-center' },
                    { width: '20%', targets: 3, className: 'text-center' },
                    { width: '20%', targets: 4, className: 'text-center' }
                ],
                order: [[0, 'asc']]
            });

            that.t2 = new DataTable("#toperadores", {
                layout: {
                    topStart: {
                        buttons: [
                            {
                                extend: 'excelHtml5',
                                title: function () {

                                    let inicio = formatDate($('#initDateOp').val());
                                    let fin = formatDate($('#endDateOp').val());

                                    return `Fondo de Defunción e Invalidez de Operadores | ${inicio} al ${fin} | Practicontrol Web`;
                                },
                                filename: function () {

                                    let inicio = formatDate($('#initDateOp').val()).replace(/\//g, "-");
                                    let fin = formatDate($('#endDateOp').val()).replace(/\//g, "-");

                                    return `FDI_Operadores_${inicio}_al_${fin}`;
                                }
                            }
                        ]
                    }
                },
                columnDefs: [
                    { width: '10%', targets: 0, className: 'text-center', type: 'producto' },
                    { width: '10%', targets: 1, className: 'text-center' },
                    { width: '30%', targets: 2, className: 'text-center' },
                    { width: '20%', targets: 3, className: 'text-center' },
                    { width: '20%', targets: 4, className: 'text-center' }
                ],
                order: [[0, 'asc']]
            });
        },
    })
});