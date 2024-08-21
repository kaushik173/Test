
var oTable = $('#invoiceList').dataTable({
    "searching": false,
    "bSort": false,
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,
    "columns": [
        {
            "data": "WorkID",
            "render": function (data, type, full, meta) {
                return ('<input data-agencyid=' + full.AgencyID + ' data-id="' + data + '" data-batchguid="' + full.BatchGUID + '" type="checkbox" name="name" value="" class="chk-invoice"/>');
            }
        },
        { "data": "HourlyInvoiceDate" },
        { "data": "CaseName" },
        { "data": "Description" },
        {
            "data": "Amount",
            "render": function (data, type, full, meta) {
                return '$' + data;
            }
        },
        { "data": "Hours" },
        {
            "data": "Rate",
            "render": function (data, type, full, meta) {
                return '$' + data;
            }
        }
    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    "deferRender": true
});


function fitCalculatedHeightForSearchDataTable() {
    var calc_height = 0;
    if (oTable != null) {
        calc_height = $(window).height();
        var _offset = 25;

        $("#divSearchResult .dataTables_scrollBody").children().first().parentsUntil("body").each(function () {
            $(this).siblings().each(function () {
                if (calc_height > $(this).outerHeight(true) && $(this).css('display') != 'none') {
                    if ($(this).attr("id") == 'loading')
                        return;
                    calc_height = calc_height - $(this).outerHeight(true);
                }
            });
            _offset = _offset + $(this).outerHeight(true) - $(this).height();
        });

        calc_height = calc_height - _offset;
        calc_height = calc_height < 250 ? 250 : calc_height;

        $('#divSearchResult .dataTables_scrollBody').css('max-height', calc_height + 'px');
        oTable.fnAdjustColumnSizing();
    }
    return calc_height;
}


function Search() {
    IPadKeyboardFix();
    var data = { id: $("#AttorneyID").val() };
    $.ajax({
        type: "POST", url: '/HourlyInvoiceList/SearchInvoiceToAdd', data: data,
        success: function (result) {
            setData(result);
        },
        dataType: 'json'
    });

}

function setData(data) {
    oTable.fnClearTable();
    if (data.data.length > 0) {
        oTable.fnAddData(data.data);
        fitCalculatedHeightForSearchDataTable();
    } else
        Notify('No results found.', 'bottom-right', '5000', 'blue', 'fa-frown-o', true);


}

function getData() {
    var data = {
        id: $("#AttorneyID").val(),
        agencyId: $("#invoiceList .chk-invoice:checked:first").data("agencyid"),
        batchGuid: $("#invoiceList .chk-invoice:checked:first").data("batchguid"),
        workIds: ''
    };

    var workids = [];
    $("#invoiceList .chk-invoice:checked").each(function () {
        workids.push($(this).data("id"));
    });

    data.workIds = workids.join(',');
    return data;
}

function saveData() {
    if ($("#invoiceList .chk-invoice:checked").length == 0) {
        notifyDanger("At least one invoice is required");
        $("#invoiceList .chk-invoice:first").focus();
        return false;
    }

    var data = getData();
 
    $.ajax({
        type: "POST", url: '/HourlyInvoiceList/Add', data: data,
        success: function (result) {
            RequestSubmitted();
            if (result.URL != undefined && result.URL != '') {
                window.location.href = result.URL;
            }
        },
        dataType: 'json'
    });
}

$('#AttorneyID').on('change', function () {
    if (!IsValidFormRequest()) {
        return false;
    }
    else {
        Search();
    }
});

$("#chkAll").on("click", function () {
    var select = $(this).is(":checked");
    $("#invoiceList .chk-invoice").each(function () {
        $(this).prop("checked", select);
    });
});

$("#invoiceList").on("click", ".chk-invoice", function () {
    var allSelected = $("#invoiceList .chk-invoice:checked").length == $("#invoiceList .chk-invoice").length;
    $("#chkAll").prop("checked", allSelected);
});

$("#btnAdd").on("click", function () {
    if (!IsValidFormRequest()) {
        return false;
    }
    saveData();
});


$(window).bind('resize', function () {    
    fitCalculatedHeightForSearchDataTable();
});

$(document).ready(function () {
    if ($("#AttorneyID").val() != '') {
        Search();
    }
});