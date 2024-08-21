

var origin_wrapper_height = 0, origin_content_height = 0;
var currentAttorney = 0;






$(window).on("keydown", handleHotkey);

function handleHotkey(e) {
    if (!e.ctrlKey) return;
    switch (String.fromCharCode(e.keyCode).toLowerCase()) {
        case 'r':
            $('#reset').trigger('click');
            e.preventDefault();
            break;
        default:
            break;
    }
}

$('#filter').on('click', function () {
    loadData();
});


//href="/Case/Main/' + full.ClientCaseId + '"
$(window).bind('resize', function () {
    $('#trainingSummary').css('width', '100%');
});

function loadData() {
    
    var $form = $('#trainingSummary-search-form');
    IPadKeyboardFix();

    var data = getData();
    
    var $form = $('#myCaseLoad-form');

    $.ajax({
        type: "POST", url: '/TrainingSummary/Filter', data: data, success: function (data) {
            $('#trainingSummaryDiv').html(data);
            fitCalculatedHeightForSearchDataTable();
}
        });
}

function getData() {
    var fData = $('#trainingSummary-search-form').serialize();
    return fData;
}

$(window).bind('resize', function () {
    fitCalculatedHeightForSearchDataTable();
});

function fitCalculatedHeightForSearchDataTable() {
    var calc_height = 0;
        calc_height = $(window).height();
        var _offset = 25;
    origin_wrapper_height = $('body>div.container-fluid').height();
    origin_content_height = $('#divSearchResult .table-responsive').height();

    $("#divSearchResult .table-responsive").children().first().parentsUntil("body").each(function () {

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
    $('#divSearchResult .table-responsive').css('max-height', calc_height + 'px');

    return calc_height;
}

$(document).ready(function () {
    loadData();
});