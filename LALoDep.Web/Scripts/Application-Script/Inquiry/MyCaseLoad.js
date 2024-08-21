$BaseURL = '/';
var origin_wrapper_height = 0, origin_content_height = 0;

$('#btnSearch').on('click', function (e) {
    e.preventDefault();
    IPadKeyboardFix();
    $('#myCaseLoad-form').submit();

});

$('#myCaseLoad-form').on('submit', function (e) {
    e.preventDefault();

    var $form = $('#myCaseLoad-form');

    $.ajax({
        type: "POST", url: '/Inquiry/MyCaseLoad', data: $form.serialize(), success: function (data) {
            $('#myCaseLoadData').html(data);
            fitCalculatedHeightForSearchDataTable();
        }
    });
});

$('#btnPrint').on('click', function () {

    var data = {
        'StartDate': $('#StartDate').val(),
        'EndDate': $('#EndDate').val(),
        'LastName': $('#LastName').val(),
        'FirstName': $('#FirstName').val(),
        'CaseNumber': $('#CaseNumber').val(),
        'ClientType': $('#ClientType').val(),
        'CaseStatus': $('#CaseStatus').val(),
        'CountyID': $('#CountyID').val(),
        'PrintSort': $('#PrintSort').val(),
        'AgencyID': $('#AgencyID').val(),
        'PersonID': $('#PersonID').val(),
    }
    var _target = $("body").data("print-document-on") == "NewWindow" ? 'target="_blank"' : '';

   $.download($('#hdnCurrentSessionGuidPath').val()+'/Inquiry/PrintMyCaseload', data, "POST",_target);
});

$(window).bind('resize', function () {
    fitCalculatedHeightForSearchDataTable();
});

function fitCalculatedHeightForSearchDataTable() {
    var calc_height = 0;
    calc_height = $(window).height();
    var _offset = 25;
    origin_wrapper_height = $('body>div.container-fluid').height();
    origin_content_height = $('#divMyCaseLoadReult .table-responsive').height();

    $("#divMyCaseLoadReult .table-responsive").children().first().parentsUntil("body").each(function () {

        $(this).siblings().each(function () {
            if (calc_height > $(this).outerHeight(true) && $(this).css('display') != 'none') {
                //console.log(calc_height + " - " + $(this).outerHeight(true));
                if ($(this).attr("id") == 'loading')
                    return;
                calc_height = calc_height - $(this).outerHeight(true);
            }
        });
        _offset = _offset + $(this).outerHeight(true) - $(this).height();
    });

    //console.log("calc :" + calc_height + " offset: " + _offset);
    calc_height = calc_height - _offset;
    //console.log("total: " + calc_height);
    $('#divMyCaseLoadReult .table-responsive').css('max-height', calc_height + 'px');

    return calc_height;
}

$(document).ready(function () {
    if ($('#PersonID').val() == "0" && $onViewLoad != "True") {
        return false;
    }
    else {        
        $('#myCaseLoad-form').submit();
    }
});

