$(document).ready(function () {

    var app = new Vue({

        el: '#content',

        data: {
            loading: false,
            table: null
        },

        methods: {

            initTable: function () {

                var that = this;
                var tableSelector = '#cuponesTable';

                /*
                 * Verificamos que la tabla exista.
                 */
                if (!document.querySelector(tableSelector)) {
                    console.warn('No se encontró la tabla #cuponesTable');
                    return;
                }

                /*
                 * Verificamos que DataTables esté disponible.
                 */
                if (typeof DataTable === 'undefined') {
                    console.error('DataTables no se encuentra cargado.');
                    return;
                }

                /*
                 * Evita inicializar DataTables más de una vez.
                 *
                 * Esto nos permitirá posteriormente refrescar la tabla,
                 * trabajar con AJAX, modales, filtros, etc. sin provocar
                 * una segunda inicialización accidental.
                 */
                if (
                    $.fn.DataTable &&
                    $.fn.DataTable.isDataTable(tableSelector)
                ) {
                    that.table = $(tableSelector).DataTable();
                    return;
                }

                /*
                 * Inicialización de la tabla de cupones.
                 */
                that.table = new DataTable(tableSelector, {

                    pageLength: 25,

                    lengthMenu: [
                        [10, 25, 50, 100, -1],
                        [10, 25, 50, 100, 'Todos']
                    ],

                    /*
                     * Folio de mayor a menor.
                     */
                    order: [
                        [0, 'desc']
                    ],

                    autoWidth: false,

                    /*
                     * Botones de exportación.
                     */
                    layout: {

                        topStart: {

                            buttons: [

                                /*
                                 * EXCEL
                                 */
                                {
                                    extend: 'excelHtml5',

                                    text:
                                        '<i class="bx bx-spreadsheet me-1"></i> Excel',

                                    title:
                                        'Listado de Cupones | Practicontrol Web',

                                    filename: function () {

                                        var fecha = new Date()
                                            .toLocaleDateString('es-MX')
                                            .replace(/\//g, '-');

                                        return 'Cupones_' + fecha;
                                    },

                                    /*
                                     * No exportar la última columna:
                                     * Acciones.
                                     */
                                    exportOptions: {
                                        columns: ':not(:last-child)'
                                    }
                                },

                                /*
                                 * PDF
                                 */
                                {
                                    extend: 'pdfHtml5',

                                    text:
                                        '<i class="bx bxs-file-pdf me-1"></i> PDF',

                                    title:
                                        'Listado de Cupones | Practicontrol Web',

                                    filename: function () {

                                        var fecha = new Date()
                                            .toLocaleDateString('es-MX')
                                            .replace(/\//g, '-');

                                        return 'Cupones_' + fecha;
                                    },

                                    orientation: 'landscape',

                                    pageSize: 'LETTER',

                                    /*
                                     * No exportar la última columna:
                                     * Acciones.
                                     */
                                    exportOptions: {
                                        columns: ':not(:last-child)'
                                    }
                                }

                            ]

                        }

                    },

                    /*
                     * Configuración de columnas.
                     */
                    columnDefs: [

                        /*
                         * Folio
                         */
                        {
                            targets: 0,
                            width: '7%',
                            className: 'text-center text-wrap'
                        },

                        /*
                         * Fecha de pago
                         */
                        {
                            targets: 1,
                            width: '11%',
                            className: 'text-center text-wrap'
                        },

                        /*
                         * Socio / Operador
                         */
                        {
                            targets: 2,
                            width: '18%',
                            className: 'text-wrap'
                        },

                        /*
                         * Proveedor / Hotel
                         */
                        {
                            targets: 3,
                            width: '17%',
                            className: 'text-wrap'
                        },

                        /*
                         * Tarifa / Perforación
                         */
                        {
                            targets: 4,
                            width: '12%',
                            className: 'text-center text-wrap'
                        },

                        /*
                         * Valor
                         */
                        {
                            targets: 5,
                            width: '10%',
                            className: 'text-center text-wrap'
                        },

                        /*
                         * Bloqueado
                         */
                        {
                            targets: 6,
                            width: '9%',
                            className: 'text-center text-wrap'
                        },

                        /*
                         * Activo
                         */
                        {
                            targets: 7,
                            width: '7%',
                            className: 'text-center text-wrap'
                        },

                        /*
                         * Acciones
                         */
                        {
                            targets: 8,
                            width: '9%',
                            className: 'text-center text-wrap',
                            orderable: false,
                            searchable: false
                        }

                    ],

                    /*
                     * Traducciones.
                     */
                    language: {

                        decimal: '',

                        emptyTable:
                            'No hay cupones disponibles',

                        info:
                            'Mostrando _START_ a _END_ de _TOTAL_ cupones',

                        infoEmpty:
                            'Mostrando 0 a 0 de 0 cupones',

                        infoFiltered:
                            '(filtrado de _MAX_ cupones totales)',

                        lengthMenu:
                            'Mostrar _MENU_ registros',

                        loadingRecords:
                            'Cargando...',

                        processing:
                            'Procesando...',

                        search:
                            'Buscar:',

                        searchPlaceholder:
                            'Buscar en la tabla...',

                        zeroRecords:
                            'No se encontraron resultados',

                        paginate: {
                            first: 'Primero',
                            last: 'Último',
                            next: 'Siguiente',
                            previous: 'Anterior'
                        }

                    }

                });
            }

        },

        mounted: function () {

            var that = this;

            that.initTable();

            console.log('Cupones Index cargado');

        }

    });

});