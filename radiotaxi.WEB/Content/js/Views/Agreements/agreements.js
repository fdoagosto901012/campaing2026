$(document).ready(function () {

    var app = new Vue({
        el: '#content',
        data: {
            convenioBuscar: '',
            mostrarBuscadorComparacion: false,
            mostrarComparacion: false,
            result : null,
        },
        methods: {
            abrirPanelComparacion: function () {
                this.mostrarBuscadorComparacion = true;

                this.$nextTick(function () {
                    if (this.$refs.inputConvenioComparar) {
                        this.$refs.inputConvenioComparar.focus();
                    }
                });
            },
            buscarConvenio: function () {
                this.mostrarComparacion = true;

                this.$nextTick(function () {
                    if (this.$refs.inputConvenioComparar) {
                        this.$refs.inputConvenioComparar.focus();
                        this.$refs.inputConvenioComparar.select();
                    }
                });
            },
            limpiarComparacion: function () {
                this.mostrarComparacion = false;
                this.convenioBuscar = '';
                this.$nextTick(function () {
                    if (this.$refs.inputConvenioComparar) {
                        this.$refs.inputConvenioComparar.focus();
                    }
                });
            },
            cerrarPanelComparacion: function () {
                this.mostrarBuscadorComparacion = false;
            },
            cancelconvenio: function (id_op) {
                $.ajaxSetup({
                    headers: { 'RequestVerificationToken': csrfToken }
                });
                $.ajax({
                    type: 'POST',
                    url: '/json/cancelar/convenio',
                    data: {
                        id_op: id_op
                    },
                    cache: false,
                    success: function (data) {
                        if (data.httpcode == 400) {

                            Swal.fire({
                                icon: 'error',
                                title: 'Error',
                                text: data.message,
                                confirmButtonText: 'Aceptar'
                            });

                        } else {

                            Swal.fire({
                                icon: 'success',
                                title: 'Convenio cancelado',
                                text: data.message,
                                confirmButtonText: 'Aceptar'
                            });

                        }
                    },
                    error: function (response) {
                        console.log('Error:', response);

                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: 'Ocurrió un error al comunicarse con el servidor.',
                            confirmButtonText: 'Aceptar'
                        });
                    }
                });
            },

            searchagreement: function (id_op, reference) {
                var that = this;
                $.ajaxSetup({
                    headers: { 'RequestVerificationToken': csrfToken }
                });
                $.ajax({
                    type: 'POST',
                    url: '/json/convenio/detalles',
                    data: {
                        id_op: id_op,
                        reference: reference
                    },
                    cache: false,
                    success: function (data) {
                        if (data.httpcode == 400) {

                            Swal.fire({
                                icon: 'warning',
                                title: 'Convenio no encontrado',
                                text: data.message,
                                confirmButtonText: 'Aceptar'
                            }).then(() => {

                                that.result = null;
                                that.mostrarComparacion = false;

                                that.$nextTick(function () {
                                    if (that.$refs.inputConvenioComparar) {
                                        that.$refs.inputConvenioComparar.focus();
                                        that.$refs.inputConvenioComparar.select();
                                    }
                                });

                            });

                        } else {

                            that.result = data.result;
                            that.mostrarComparacion = true;

                            console.log('Detalles del convenio:', data);

                        }
                    },
                    error: function (response) {
                        console.log('Error:', response);

                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: 'Ocurrió un error al comunicarse con el servidor.',
                            confirmButtonText: 'Aceptar'
                        });
                    }
                });
            },
            cancelpartida: function (id_op, folio) {
                console.log('Cancelar partida');
                console.log('Convenio:', id_op);
                console.log('Folio:', folio);
            },
            money: function (value) {
                if (value === null || value === undefined || value === '') return '';

                return Number(value).toLocaleString('es-MX', {
                    style: 'currency',
                    currency: 'MXN'
                });
            },

            numberClean: function (value) {
                if (value === null || value === undefined || value === '') return '';
                return Number(value).toLocaleString('es-MX', {
                    maximumFractionDigits: 0
                });
            },

            dateMx: function (value) {
                if (!value) return '-';

                if (typeof value === 'string' && value.indexOf('/Date(') === 0) {
                    var timestamp = parseInt(value.replace('/Date(', '').replace(')/', ''), 10);
                    var date = new Date(timestamp);
                    return date.toLocaleDateString('es-MX');
                }

                if (typeof value === 'string' && value.includes('T')) {
                    var dateIso = new Date(value);
                    return dateIso.toLocaleDateString('es-MX');
                }

                if (typeof value === 'string' && value.length >= 10) {
                    return value.substring(0, 10).split('-').reverse().join('/');
                }

                return value;
            },

            timeMx: function (value) {
                if (!value) return '';

                if (typeof value === 'string' && value.indexOf('/Date(') === 0) {
                    var timestamp = parseInt(value.replace('/Date(', '').replace(')/', ''), 10);
                    var date = new Date(timestamp);

                    return date.toLocaleTimeString('es-MX', {
                        hour: '2-digit',
                        minute: '2-digit'
                    });
                }

                if (typeof value === 'string' && value.includes('T')) {
                    var dateIso = new Date(value);

                    return dateIso.toLocaleTimeString('es-MX', {
                        hour: '2-digit',
                        minute: '2-digit'
                    });
                }

                return value;
            }
        }
    });
});