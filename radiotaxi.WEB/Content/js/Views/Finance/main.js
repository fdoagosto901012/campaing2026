$(document).ready(function () {
    var t1 = new DataTable("#tsocios", {
        pageLength: 100,
        layout: {
            topStart: {
                buttons: ['excelHtml5']
            }
        },
        columnDefs: [
            { width: '10%', targets: 0, className: 'text-center' },
            { width: '40%', targets: 1, className: 'text-center' },
            { width: '20%', targets: 2, className: 'text-center' },
            { width: '20%', targets: 3, className: 'text-center' }
        ],
        columns: [
            { data: "socio", name: "socio", sortable: true },
            { data: "nombre", name: "nombre", sortable: true },
            {
                data: "IMPORTE_MES",
                render: $.fn.dataTable.render.number(',', '.', 2),
                name: "IMPORTE_MES", sortable: false
            },
            {
                data: "IMPORTE", name: "IMPORTE",
                render: $.fn.dataTable.render.number(',', '.', 2),
                sortable: false
            }
        ]
    });

    var t2 = new DataTable("#toperadores", {
        layout: {
            topStart: {
                buttons: ['excelHtml5']
            }
        }
    });

    $("#formsoc").on("submit", function (event) {
        event.preventDefault();
        $inputs = $('#formsoc').serializeArray();
        $.ajaxSetup({
            headers: { 'RequestVerificationToken': csrfToken }
        });
        $.ajax({
            type: "POST",
            url: "/finance/fdi_soc/",
            data: $inputs,
            context: this,
            cache: false,
            success: function (data) {
                console.log(data);
                /*
                var html = "<tr>" +
                    " <td> " + data.socio + " </td> " +
                    " <td> " + data.nombre + "</td> " +
                    " <td> " + data.IMPORTE_MES + "</td> " +
                    " <td> " + data.IMPORTE+ " </td> " +
                    "</tr> ";

                $("#t-content").append(html);
                */
                data.forEach(item => {
                    var html = "<tr>" +
                        " <td> " + item.socio + " </td> " +
                        " <td> " + item.nombre + "</td> " +
                        " <td> " + item.IMPORTE_MES.toLocaleString('es-MX', { style: 'currency', currency: 'MXN' }) + "</td> " +
                        " <td> " + item.IMPORTE.toLocaleString('es-MX', { style: 'currency', currency: 'MXN' }) + " </td> " +
                        "</tr>";

                    $("#t-content").append(html);
                });
                //t1.rows.remove();
                //t1.rows.add(data).draw();
            },
            complete: function (response) {
                console.log("Complete : ");
                //console.log(response);
            },
            error: function (response) {
                console.log("Error : ");
                console.log(response);
            }
        });
    });
});

