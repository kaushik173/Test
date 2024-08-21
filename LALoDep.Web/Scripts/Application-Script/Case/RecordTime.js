function fitCalculatedHeightForSearchDataTable() {
    var calc_height = 0;
    calc_height = $(window).height();
    var _offset = 25;
    origin_wrapper_height = $('body>div.container-fluid').height();
    origin_content_height = $('#divSearchResult .table-responsive').height();
    var extra = 0;
    if ($('#slideout').length > 0) {
        extra = 100;
    }
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
    $('#divSearchResult .table-responsive').css('max-height', calc_height + extra + 'px');

    return calc_height;
}

$('#search').on('click', function (e) {
    e.preventDefault();
    IPadKeyboardFix();
    var $form = $('#recordTime-form');

    $.ajax({
        type: "POST", url: '/Case/RecordTimeList', data: $form.serialize(), success: function (data) {
            $('#recordTimeData').html(data);
            fitCalculatedHeightForSearchDataTable();
        }
    });
});

$(window).bind('resize', function () {
    fitCalculatedHeightForSearchDataTable();
});

$('#addNew').on('click', function (e) {
    if (Contains(window.location.href, "arId=")) {
        window.location.href = "/Case/RecordTimeAdd?arId=" + getParameterByName("arId");
    } else {
        window.location.href = "/Case/RecordTimeAdd";
    }

});

$("#btnPrint").on("click", function () {
    var printWorks = $(".chkPrint:checked");
    if (printWorks.length <= 0) {
        notifyDanger('No reccord time has been selected.');
        return;
    }


    var workids = [];
    for (var indx = 0; indx < printWorks.length; indx++) {
        var chk = printWorks.eq(indx);
        workids.push(chk.attr("id"));
    }


    var data = { id: workids.join(',') };
   $.download($('#hdnCurrentSessionGuidPath').val()+'/Case/RecordTimeListPrint', data, "POST", "target='_blank'");

});

$(document).on("click", ".chkPrint", function () {
    $("#chkAllPrint").prop("checked", $(".chkPrint").length == $(".chkPrint:checked").length);
});

$(document).on("click", "#chkAllPrint", function () {
    var chkPrint = $(".chkPrint");
    for (var indx = 0; indx < chkPrint.length; indx++) {
        var chk = chkPrint.eq(indx);
        chk.prop("checked", $(this).is(":checked"));
    }
});

$(document).ready(function () {
    $('#search').trigger('click');
});
$('body').on('click', '.deleteRecord', function () {
    var id = $(this).attr('data-id');
    var tr = $(this).parent().parent();
    confirmBox("Are you sure you want to remove selected records?", function (result) {
        if (result) {
            $.ajax({
                type: "POST", url: '/Case/RecordTimeDelete/' + id,
                dataType: "json",
                success: function (data) {
                    tr.remove();
                    Notify('Selected record delete successfully.', 'bottom-right', '5000', 'success', 'fa-check', true);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                }
            });
        }
    });
});