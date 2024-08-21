

var oTable = $('#searchMyARs').dataTable({
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,
    "searching": false,
    "bSort": false,
    "columns": [
        {
            "data": "RequestedByName",
            "render": function (data, type, full, meta) {
                if (data != '') {
                    var chkbox = '<input id="" class="ar-transfer" type="checkbox" name="" value="" ' +
                        ' data-ReportFilingDueID="' + full.ReportFilingDueID + '"' +
                        ' data-AgencyID="' + full.AgencyID + '"' +
                        ' data-HearingID="' + full.HearingID + '"' +
                        ' data-HearingReportFilingDueTypeCodeID="' + full.HearingReportFilingDueTypeCodeID + '"' +
                        ' data-RequestDate="' + full.RequestDate + '"' +
                        ' data-DueDate="' + full.DueDate + '"' +
                        ' data-CompleteDate="' + full.CompleteDate + '"' +
                        ' data-RequestedByPersonID="' + full.RequestedByPersonID + '"' +
                        ' data-HearingReportFilingDueLegalResearchTypeCodeID="' + full.HearingReportFilingDueLegalResearchTypeCodeID + '"' +
                        ' data-RecordStateID="' + full.RecordStateID + '"' +
                        '/>';

                    return chkbox;
                }
                else {
                    return '';
                }
            }
        },
        { "data": "RequestedByName" },
        { "data": "ReportFilingDueType" },
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
        { "data": "HearingDate" },
        { "data": "HearingType" },
        { "data": "PetitionNumber" },
        { "data": "Client" }
    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    "deferRender": true
});

function loadData() {
    if ($('#StartDate').val().length == 0 && $('#EndDate').val().length == 0 && $('#DateRangeType').val() == '') {
        Notify('At least one search parameter is required.', 'bottom-right', '4000', 'danger', 'fa-warning', true);
        return false;
    }

    var data = data = {
        'PersonID': $('#PersonID').val(),
        'DateRangeType': $('#DateRangeType').val(),
        'StartDate': $('#StartDate').val(),
        'EndDate': $('#EndDate').val(),
    };

    $.ajax({
        type: "POST", url: '/Task/ARTransferList', data: data,
        success: function (data) {
            setData(data);
        },
        dataType: 'json'
    });
}

function setData(data) {
    oTable.fnClearTable();
    if (data.data.length > 0) {
        oTable.fnAddData(data.data);
        fitCalculatedHeightForSearchDataTable();
    } else {
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
    return calc_height;
}
function getData() {
    var data = {
        TransferToPersonID: $("#TransferToPersonID").val(),
        ARsToTransfer: []
    }

    $(".ar-transfer:checked").each(function (e) {
        var chk = $(this);
        data.ARsToTransfer.push({
            ReportFilingDueID: chk.attr("data-ReportFilingDueID"),
            AgencyID: chk.attr("data-AgencyID"),
            HearingID: chk.attr("data-HearingID"),
            HearingReportFilingDueTypeCodeID: chk.attr("data-HearingReportFilingDueTypeCodeID"),
            RequestDate: chk.attr("data-RequestDate"),
            DueDate: chk.attr("data-DueDate"),
            CompleteDate: chk.attr("data-CompleteDate"),
            RequestedByPersonID: chk.attr("data-RequestedByPersonID"),
            HearingReportFilingDueLegalResearchTypeCodeID: chk.attr("data-HearingReportFilingDueLegalResearchTypeCodeID"),
            RecordStateID: chk.attr("data-RecordStateID")
        });
    });

    return data;
}

function transferAR() {
    if ($("#TransferToPersonID").val() == '') {
        Notify('Transfer To is required.', 'bottom-right', '4000', 'danger', 'fa-warning', true);
        $("#TransferToPersonID").focus();
        return false;
    }
    if ($(".ar-transfer:checked").length == 0) {
        Notify('At least one AR must be selected.', 'bottom-right', '4000', 'danger', 'fa-warning', true);
        return false;
    }

    var data = getData()
    $.ajax({
        type: "POST", url: '/Task/ARTransfer', data: data,
        success: function (data) {
            RequestSubmitted();
            if (data.isSuccess) {
                window.location.href = window.location.href;
            }
        },
        dataType: 'json'
    });

    return false;
}

$(window).bind('resize', function () {
    $('#searchMyARs').css('width', '100%');
    fitCalculatedHeightForSearchDataTable();
});

$('#btnSearch').on('click', function () {
    IPadKeyboardFix();
    if (!IsValidFormRequest()) {
        return false;
    }
    loadData();

});

$("#chkSelectAll").on("click", function () {
    var checkAll = $(this).is(":checked");
    $(".ar-transfer").each(function (e) {
        $(this).prop("checked", checkAll);
    });
});

$("#searchMyARs").on("click", ".ar-transfer", function () {
    $("#chkSelectAll").prop("checked", $(".ar-transfer").length == $(".ar-transfer:checked").length);
});

$("#btnTransfer").on("click", function () {
    if (!IsValidFormRequest()) {
        return false;
    }
    transferAR();
});

$("#btnCancel").on("click", function () {
    window.location.href = "/Task/MyARQueue/" + $("#hdn_EncryptedPersonID").val();
});

$(document).ready(function () {
    loadData();
});