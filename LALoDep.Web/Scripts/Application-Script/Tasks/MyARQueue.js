

var oTable = $('#searchMyARQueue').dataTable({
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,
    "searching": false,
    "bSort": false,
    "bInfo" : false,
    "columns": [
     { "data": "RequestedByName" },
     { "data": "RequestedForName" },

     {
         "data": "ReportFilingDueType",
         "render": function (data, type, full, meta) {
             return '<a href="/Task/EditRFD/' + full.EncryptedReportFilingDueID + '?caseId=' + full.EncryptedCaseID + '" >' + data + '</a>';
         }
     },
    { "data": "RequestDate" },
    { "data": "DueDate" },

    {
        "data": "CompleteDate",
        "render": function (data, type, full, meta) {
            if (data == "Closed Client")
                return ('<span class="red">' + ((data != null) ? data : '') + '</span>')
            else
                return ('<span>' + ((data != null) ? data : '') + '</span>')
        }
    },

    { "data": "HearingType" },
    { "data": "HearingDate" },
    { "data": "PetitionNumber" },
    { "data": "Client" },
     {
         "data": "DisplayQuickAR",
         "render": function (data, type, full, meta) {
             return (data && full.RequestedByName != '') ? '<a href="/Case/QuickAR/' + full.EncryptedReportFilingDueID + '?caseId=' + full.EncryptedCaseID + '&page=/Task/MyARQueue" data-secure-id="247">Quick AR</a>' : "";
         }
     },
    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    "deferRender": true
});



function loadData() {
    var formInvalid = $('#StartDate').val().length == 0
                && $('#EndDate').val().length == 0
                && $('#DateRangeType').val() == ''
                && $('#IncludeCompletedFlag').val() == '';

    if (formInvalid) {
        Notify('At least one search parameter is required.', 'bottom-right', '5000', 'danger', 'fa-warning', true);
        return false;
    }

    var data = getData()
    $.ajax({
        type: "POST", url: '/Task/MyARQueueList', data: data,
        success: function (data) {
            setData(data);
        },
        dataType: 'json'
    });
}



function getData() {
    var data = {
        'PersonID': $('#PersonID').val(),
        'DateRangeType': $('#DateRangeType').val(),
        'IncludeCompletedFlag': parseInt($('#IncludeCompletedFlag').val()),
        'StartDate': $('#StartDate').val(),
        'EndDate': $('#EndDate').val(),
    };
    return data;
}
function setData(data) {
    oTable.fnClearTable();
    if (data.data.length > 0) {
        $("#totalArs").text(" (" + data.recordsTotal + ")");
        oTable.fnAddData(data.data);
        fitCalculatedHeightForSearchDataTable();
    } else {
        $("#totalArs").text("");
        Notify('No results found.', 'bottom-right', '5000', 'blue', 'fa-frown-o', true);
    }
}

function fitCalculatedHeightForSearchDataTable() {
    var calc_height = 0;
    calc_height = $(window).height();
    var _offset = 25;
    origin_wrapper_height = $('body>div.container-fluid').height();
    origin_content_height = $('#divSearchResult .dataTables_scrollBody').height();

    $("#divSearchResult .dataTables_scrollBody").children().first().parentsUntil("body").each(function () {
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
    $('#divSearchResult .dataTables_scrollBody').css('max-height', calc_height + 'px');
    oTable.fnAdjustColumnSizing();
    //new $.fn.dataTable.FixedHeader(oTable);
    return calc_height;
}

$(window).bind('resize', function () {
    $('#searchMyARQueue').css('width', '100%');
    fitCalculatedHeightForSearchDataTable();
});

$('#btnSearch').on('click', function () {
    IPadKeyboardFix();
    loadData();
});


$("#btnTransfer").on("click", function () {
    window.location.href = "/Task/ARTransfer/" + $("#EncryptedPersonID").val();
});


$(document).ready(function () {
    loadData();
}); 

$("#btnPrint").on("click", function () {
    var data = getData();
    console.log(data)
   $.download($('#hdnCurrentSessionGuidPath').val()+'/Task/MyARQueuePrint', data, "POST", 'target="_blank"');

});


 