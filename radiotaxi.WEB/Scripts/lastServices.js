$(document).ready(function () {
    function LastServices() {
        var phoneNumber = $("#phoneId").val();;
        var tempUrl = '/TaxiService/GetLastServices';
        $.ajax({
            type: "GET",
            url: tempUrl,
            data: { phoneNumber: phoneNumber },
            success: function (partialView) {
                $('#dvResult').html(partialView);
                $('#dvResult').show();
            }
        });
    };

    LastServices();
});