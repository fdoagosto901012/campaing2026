$(document).on('click', ".addRow", function () {

    var rowCount = $('.rtNumberServicesRow').length;
    if (rowCount == 0) {
    }
    $("#rtNumberServices").append('<div class="rtNumberServicesRow"><input type="hidden" name="rtNumberServices[' + rowCount + '].serviceId" value="' + $('#id').val() + '" class="serviceId"><label for="Número RT">Número RT&nbsp</label><input type="text" name="rtNumberServices[' + rowCount + '].rtNumber" class="rtNumber" /><label for="Nombre económico">&nbspNombre económico&nbsp</label><input type="text" name="rtNumberServices[' + rowCount + '].taxiNumber" class="taxiNumber" />&nbsp<a href="javascript:void(0);" class="remRow"><i class="icon-remove-sign fcancel-remove"></i></a></div>');
});

$(document).on('click', ".addRowR", function () {

    var rowCount = $('.taxiReportsRow').length;
    if (rowCount == 0) {
    }
    $("#taxiReports").append('<div class="taxiReportsRow"><input type="hidden" name="taxiReports[' + rowCount + '].reportId" value="' + $('#id').val() + '" class="reportId"><label for="Número RT">Número RT&nbsp</label><input type="text" name="taxiReports[' + rowCount + '].taxiNumber" class="taxiNumber" />&nbsp<a href="javascript:void(0);" class="remRowR"><i class="icon-remove-sign fcancel-remove"></i></a></div>');
});

$("#rtNumberServices").on('click', '.remRow', function () {

    $(this).closest('.rtNumberServicesRow').remove();

    $('.serviceId').each(function (index) {
        $(this).attr('name', 'rtNumberServices[' + index + '].serviceId');
    });

    $('.id').each(function (index) {
        $(this).attr('name', 'rtNumberServices[' + index + '].id');
    });

    $('.rtNumber').each(function (index) {
        $(this).attr('name', 'rtNumberServices[' + index + '].rtNumber');
    });

    $('.taxiNumber').each(function (index) {
        $(this).attr('name', 'rtNumberServices[' + index + '].taxiNumber');
    });
});

$("#taxiReports").on('click', '.remRowR', function () {

    $(this).closest('.taxiReportsRow').remove();

    $('.reportId').each(function (index) {
        $(this).attr('name', 'taxiReports[' + index + '].reportId');
    });

    $('.id').each(function (index) {
        $(this).attr('name', 'taxiReports[' + index + '].id');
    });

    $('.taxiNumber').each(function (index) {
        $(this).attr('name', 'taxiReports[' + index + '].taxiNumber');
    });
});


//$(document).on('click', "#searchb", function () {
/*
$(document).on('submit', '.search-form', function (e) {
    e.preventDefault();
    var phoneNumber = $("#phoneNumber").val();
    var name = $("#name").val();
    var lastName1 = $("#lastName1").val();
    var lastName2 = $("#lastName2").val();

    $.ajax({
        type: "GET",
        url: '/Search/GetClients/',
        data: { phoneNumber: phoneNumber, name: name, lastName1: lastName1, lastName2: lastName2 },
        success: function (partialView) {
            $('#dvSearchResult').html(partialView);
            $('#dvSearchResult').show();
            document.getElementById('dvSearchResult').scrollIntoView();
        }
    });
});*/

$(document).on('submit', '.search-forms', function (e) {
    e.preventDefault();
    var phoneNumber = $("#phoneNumber").val();
    var rtNumber = $("#rtNumber").val();
    var datePickUp = $("#datePickUp").val();
    $("#datePickUp").val("");
    $.ajax({
        type: "GET",
        url: '/Search/GetServices/',
        data: { phoneNumber: phoneNumber, rtNumber: rtNumber, datePickUp: datePickUp },
        success: function (partialView) {
            $('#dvSearchServicesResult').html(partialView);
            $('#dvSearchServicesResult').show();
            document.getElementById('dvSearchServicesResult').scrollIntoView();
        }
    });
});


$(document).on('submit', '#historyForm', function (e) {
    document.getElementById('dvSearchServicesResult').scrollIntoView();
});

$(document).on('submit', '#reportForm', function (e) {
    document.getElementById('dvSearchReportsResult').scrollIntoView();
});


/*
$(document).on('click', "#searchb2", function () {
    var phoneNumber = $("#phoneNumber2").val();

    $.ajax({
        type: "GET",
        url: '/Search/GetClients/',
        data: { phoneNumber: phoneNumber },
        success: function (partialView) {
            $('#dvSearchResult').html(partialView);
            $('#dvSearchResult').show();
        }
    });
});*/


$(document).on('click', "td.addressSource.address", function () {
    $("#startPoint").val($(this).data("origin"));
    console($(this).data("origin"));
});
