$BaseURL = '/';
var origin_wrapper_height = 0, origin_content_height = 0;
var page = (function () {
    return {        
        rowSelectedKey: GetWindowID()+'ARSummary-search-form-row#',
        ttl: 600000 /*In Milli seconds*/
    };
})();

function clearSelectedRow() {
    simpleStorage.deleteKey(page.rowSelectedKey);
};
function markSelectedRow() {
    var rowId = simpleStorage.get(page.rowSelectedKey);
    if (rowId != '') {
        $('#ARSummaryList table').find("tr[data-id=" + rowId + "]").addClass("selectedrow");        
    }
}

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
    calc_height = calc_height < 250 ? 250 : calc_height;

    $('#divMyCaseLoadReult .table-responsive').css('max-height', calc_height + 'px');

    return calc_height;
}

$('#search').on('click', function (e) {
    e.preventDefault();
      IPadKeyboardFix();
    $("#htn_BackButtonButtonValue").val('');
    $('#ARSummary-search-form').submit();

});

$('#ARSummary-search-form').on('submit', function (e) {
    e.preventDefault();

    var $form = $('#ARSummary-search-form');

    $.ajax({
        type: "POST", url: '/ARSummary/Search', data: $form.serialize(), success: function (data) {
            $('#ARSummaryList').html(data);
            fitCalculatedHeightForSearchDataTable();            
            if ($("#htn_BackButtonButtonValue").val() == 'true') {
                markSelectedRow();
            }
            else {
                clearSelectedRow();
            }
        }
    });
});

//Set selected Row
$("#ARSummaryList").on('click', 'a.person-name', function (e) {
    e.preventDefault();
    $this = $(this);    
    simpleStorage.set(page.rowSelectedKey, $this.closest('tr').data("id"), { TTL: page.ttl });
    $("#htn_BackButtonButtonValue").val("true");
    document.location = $this.attr('href');
});

$(window).bind('resize', function () {
    fitCalculatedHeightForSearchDataTable();
});

$(document).ready(function () {
    $('#ARSummary-search-form').submit();    
});