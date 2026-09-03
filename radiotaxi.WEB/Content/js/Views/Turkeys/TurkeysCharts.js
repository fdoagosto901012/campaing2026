
// A $( document ).ready() block.
$(document).ready(function () {
    var entregaValues = [];
    var entregasLabel = [];

    var entregaValuesop = [];
    var entregasLabelop = [];


    // Función para obtener los colores desde el atributo data-colors
    function getChartColors(elementId) {
        const element = document.getElementById(elementId);
        const colorData = element.getAttribute('data-colors');
        return JSON.parse(colorData); // Convierte el string JSON a un array
    }




    // CARGAR CON AJAX 
    var url = "/Turkey/getbydatedelivered/";
    // BUSCAMOS LAS DEUDAS DEL SOCIO QUE VA A HACER EL PAGO.
    $.ajaxSetup({
        headers: { 'RequestVerificationToken': csrfToken }
    });
    $.ajax({
        type: "GET",
        url: url,
        data: {},
        context: this,
        cache: false,
        success: function (data) {
            $.each(JSON.parse(data), function (index, value) {
                entregaValues.push(value.amount);
                var formattedDate = new Date(value.date);
                var d = formattedDate.getDate();
                var m = formattedDate.getMonth();
                m += 1;  // JavaScript months are 0-11
                var y = formattedDate.getFullYear();
                entregasLabel.push(d + "-" + m + "-" + y);
            });
            // Gráfico de líneas
            // Gráfico de líneas
            var optionsLine = {
                chart: {
                    type: 'bar',
                    height: 350,
                    toolbar: {
                        show: true, // Mostrar la barra de herramientas
                        tools: {
                            download: true, // Mostrar solo el botón de descarga
                            zoomin: false, // Desactivar botón de zoom in
                            zoomout: false, // Desactivar botón de zoom out
                            pan: false, // Desactivar pan
                            reset: false, // Desactivar el botón de reset
                            zoom: false, // Desactivar zoom
                        }
                    }
                },
                series: [{
                    name: 'Entrega de pavos socios',
                    data: entregaValues
                }],
                dataLabels: {
                    enabled: true
                },
                stroke: {
                    curve: 'smooth'
                },
                xaxis: {
                    categories: entregasLabel,
                },
                colors: getChartColors('Linechart') // Colores extraídos del HTML
            };

            var chartLine = new ApexCharts(document.querySelector("#Linechart"), optionsLine);
            chartLine.render();

        },
        error: function (response) {

        },
        complete: function (response) {
            //console.log("Complete");
        }
    });





    // CARGAR CON AJAX 
    var urlop = "/Turkey/getbydatedeliveredop/";
    // BUSCAMOS LAS DEUDAS DEL SOCIO QUE VA A HACER EL PAGO.
    $.ajaxSetup({
        headers: { 'RequestVerificationToken': csrfToken }
    });
    $.ajax({
        type: "GET",
        url: urlop,
        data: {},
        context: this,
        cache: false,
        success: function (data) {
            $.each(JSON.parse(data), function (index, value) {

                //console.log(data);
                entregaValuesop.push(value.amount);

                var formattedDate = new Date(value.date);
                var d = formattedDate.getDate();
                var m = formattedDate.getMonth();
                m += 1;  // JavaScript months are 0-11
                var y = formattedDate.getFullYear();
                entregasLabelop.push(d + "-" + m + "-" + y);

            });

            //console.log(entregaValuesop);
            //console.log(entregasLabelop);

            // Gráfico de líneas
            var optionsLine = {
                chart: {
                    type: 'bar',
                    height: 350,
                    toolbar: {
                        show: true, // Mostrar la barra de herramientas
                        tools: {
                            download: true, // Mostrar solo el botón de descarga
                            zoomin: false, // Desactivar botón de zoom in
                            zoomout: false, // Desactivar botón de zoom out
                            pan: false, // Desactivar pan
                            reset: false, // Desactivar el botón de reset
                            zoom: false, // Desactivar zoom
                        }
                    }
                },
                series: [{
                    name: 'pavos pendientes/entregados socios',
                    data: entregaValuesop
                }],
                dataLabels: {
                    enabled: true
                },
                stroke: {
                    curve: 'smooth'
                },
                xaxis: {
                    categories: entregasLabelop,
                },
                colors: getChartColors('Linechartop') // Colores extraídos del HTML
            };

            var chartLineop = new ApexCharts(document.querySelector("#Linechartop"), optionsLine);
            chartLineop.render();

        },
        error: function (response) {

        },
        complete: function (response) {
            //console.log("Complete");
        }
    });








    var valuesvs = [];
    var labesvs = [];

    // CARGAR CON AJAX 
    var urvs = "/Turkey/getTicketsVsDeliver/";
    // BUSCAMOS LAS DEUDAS DEL SOCIO QUE VA A HACER EL PAGO.
    $.ajaxSetup({
        headers: { 'RequestVerificationToken': csrfToken }
    });
    $.ajax({
        type: "GET",
        url: urvs,
        data: {},
        context: this,
        cache: false,
        success: function (data) {
            $.each(JSON.parse(data), function (index, _array) {
                $.each(_array, function (index, item) {
                    //console.log(item.type + " " + item.amount);
                    valuesvs.push(item.amount);
                    labesvs.push(item.type);
                });
            });

            // Gráfico de líneas
            var optionsLine = {
                chart: {
                    type: 'bar',
                    height: 350,
                    toolbar: {
                        show: true, // Mostrar la barra de herramientas
                        tools: {
                            download: true, // Mostrar solo el botón de descarga
                            zoomin: false, // Desactivar botón de zoom in
                            zoomout: false, // Desactivar botón de zoom out
                            pan: false, // Desactivar pan
                            reset: false, // Desactivar el botón de reset
                            zoom: false, // Desactivar zoom
                        }
                    }
                },
                series: [{
                    name: 'Entrega',
                    data: valuesvs
                }],
                dataLabels: {
                    enabled: true
                },
                stroke: {
                    curve: 'smooth'
                },
                xaxis: {
                    categories: labesvs,
                },
                colors: getChartColors('ticketvsdeliver') // Colores extraídos del HTML
            };

            var chartLineop = new ApexCharts(document.querySelector("#ticketvsdeliver"), optionsLine);
            chartLineop.render();

        },
        error: function (response) {

        },
        complete: function (response) {
            //console.log("Complete");
        }
    });





    var valuesvsop = [];
    var labesvsop = [];

    // CARGAR CON AJAX 
    var urvsop = "/Turkey/getTicketsVsDeliverop/";
    // BUSCAMOS LAS DEUDAS DEL SOCIO QUE VA A HACER EL PAGO.
    $.ajaxSetup({
        headers: { 'RequestVerificationToken': csrfToken }
    });
    $.ajax({
        type: "GET",
        url: urvsop,
        data: {},
        context: this,
        cache: false,
        success: function (data) {
            $.each(JSON.parse(data), function (index, _array) {
                $.each(_array, function (index, item) {
                    //console.log(item.type + " " + item.amount);
                    valuesvsop.push(item.amount);
                    labesvsop.push(item.type);
                });
            });

            // Gráfico de líneas
            var optionsLine = {
                chart: {
                    type: 'bar',
                    height: 350,
                    toolbar: {
                        show: true, // Mostrar la barra de herramientas
                        tools: {
                            download: true, // Mostrar solo el botón de descarga
                            zoomin: false, // Desactivar botón de zoom in
                            zoomout: false, // Desactivar botón de zoom out
                            pan: false, // Desactivar pan
                            reset: false, // Desactivar el botón de reset
                            zoom: false, // Desactivar zoom
                        }
                    }
                },
                series: [{
                    name: 'pavos pendientes/entregados operadores',
                    data: valuesvsop
                }],
                dataLabels: {
                    enabled: true
                },
                stroke: {
                    curve: 'smooth'
                },
                xaxis: {
                    categories: labesvsop,
                },
                colors: getChartColors('ticketvsdeliverop') // Colores extraídos del HTML
            };

            var chartLineop = new ApexCharts(document.querySelector("#ticketvsdeliverop"), optionsLine);
            chartLineop.render();

        },
        error: function (response) {

        },
        complete: function (response) {
            //console.log("Complete");
        }
    });






    var valuesvscash = [];
    var labesvscash = [];

    // CARGAR CON AJAX 
    var urvscash = "/Turkey/getbycashier/";
    // BUSCAMOS LAS DEUDAS DEL SOCIO QUE VA A HACER EL PAGO.
    $.ajaxSetup({
        headers: { 'RequestVerificationToken': csrfToken }
    });
    $.ajax({
        type: "GET",
        url: urvscash,
        data: {},
        context: this,
        cache: false,
        success: function (data) {
            $.each(JSON.parse(data), function (index, item) {
                valuesvscash.push(item.impresiones);
                labesvscash.push(item.nombre);
            });


            var options = {
                series: [{
                    data: valuesvscash
                }],
                chart: {
                    type: 'bar',
                    height: 350
                },
                plotOptions: {
                    bar: {
                        borderRadius: 4,
                        borderRadiusApplication: 'end',
                        horizontal: true,
                    }
                },
                dataLabels: {
                    enabled: false
                },
                xaxis: {
                    categories: labesvscash,
                }
            };

            var chart = new ApexCharts(document.querySelector("#cashier"), options);
            chart.render();

        },
        error: function (response) {

        },
        complete: function (response) {
            //console.log("Complete");
        }
    });

    var valuestotal = [];
    var labestotal = [];

    // CARGAR CON AJAX 
    var urltotal = "/Turkey/gettotal/";
    // BUSCAMOS LAS DEUDAS DEL SOCIO QUE VA A HACER EL PAGO.
    $.ajaxSetup({
        headers: { 'RequestVerificationToken': csrfToken }
    });
    $.ajax({
        type: "GET",
        url: urltotal,
        data: {},
        context: this,
        cache: false,
        success: function (data) {
            var obj = JSON.parse(data);

            var entregados = obj[0].total;
            var faltantes = 15000 - entregados;

            $("#totalamount").html(entregados);

            console.log();
            var options = {
                series: [entregados, faltantes],
                chart: {
                    width: 380,
                    type: 'pie',
                },
                labels: ['Pavos entregados', 'Pavos por entregar'],
                responsive: [{
                    breakpoint: 480,
                    options: {
                        chart: {
                            width: 200
                        },
                        legend: {
                            position: 'bottom'
                        }
                    }
                }]
            };

            var chart = new ApexCharts(document.querySelector("#total"), options);
            chart.render();

        },
        error: function (response) {

        },
        complete: function (response) {
            //console.log("Complete");
        }
    });





});
