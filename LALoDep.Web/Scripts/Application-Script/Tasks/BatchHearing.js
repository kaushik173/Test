
var oTable = $('#searchBatchHearing').dataTable({
    //"lengthMenu": [20],
    //"lengthChange": false,
    "searching": false,
    "bSort": false,
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,

    "columns": [

        { "data": "Agency" },
        { "data": "JcatsNumber" },
         {
             "render": function (data, type, full, meta) {
                 return ('<input type="checkbox"  class="hearingChk" data-petitionID="' + full.PetitionID + '" data-caseID="' + full.CaseID + '" name="" value=" " id="chkAll" tabindex="14" />')
             }
         },
        { "data": "CaseNumber" },
        { "data": "PetitionType" },
        { "data": "Department" },
        { "data": "Client" },
        { "data": "ChildName" },
        { "data": "NextHearing" },
    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    "deferRender": true
});

if (device.mobile() || device.tablet()) {
    
    var middleColumn = oTable.DataTable().column(2);
    middleColumn.visible(false);
}

$('#searchBatchHearing').on('draw.dt', function () {
    if (device.mobile() || device.tablet())//if mobile or tablet
    {
        $('#searchBatchHearing_info').parent().addClass('col-xs-4').removeClass('col-xs-6');
        $('#searchBatchHearing_paginate').parent().addClass('col-xs-8').removeClass('col-xs-6');
    }

});

$('.all').on("change", function () {
    $(".hearingChk").prop("checked", $(this).is(":checked"));
});

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

$(window).bind('resize', function () {
    $('#searchBatchHearing').css('width', '100%');
});

function loadData() {


    var formInvalid = false;

    if ($('#Case1').val() == ""
        && $('#Case2').val() == ""
        && $('#Case3').val() == ""
        && $('#Case4').val() == ""
        && $('#Case5').val() == ""
        && $('#Case6').val() == "") {
        formInvalid = true;
    }

    if (formInvalid) {
        Notify('At least one Case # is required.', 'bottom-right', '5000', 'danger', 'fa-warning', true);
        return false;
    }
    IPadKeyboardFix();

    var data = $('#batchHearing-form').serialize();
    $.ajax({
        type: "POST",
        url: '/Task/BatchHearingSearch',
        data: data,
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
        $('#btnAddHearing').removeClass('hidden');
        fitCalculatedHeightForSearchDataTable();
    } else {
        $('#btnAddHearing').addClass('hidden');
        Notify('No results found.', 'bottom-right', '5000', 'blue', 'fa-frown-o', true);
    }

}

function getData() {
    var StartHrr = $('#Hours').val(), StartMin = $('#Minutes').val(), StartAmPm = $('#TimeAmPm').val();
    if (StartHrr == "")
        StartHrr = "00";
    if (StartMin == "")
        StartMin = "00";
    if (StartAmPm == "")
        StartAmPm = "am";
    var hearingTime = StartHrr + ':' + StartMin + ' ' + StartAmPm;
    var data = {
        'HearingTypeID': $('#HearingTypeID').val(),
        'HearingDate': $('#HearingDate').val(),
        'HearingTime': hearingTime,
        'HearingOfficerID': $('#HearingOfficerID').val(),
        'DepartmentID': $('#DepartmentID').val(),
        'BatchHearingList': []
    };

    var hearingTr = $("#searchBatchHearing > tbody > tr");
    for (var index = 0; index < hearingTr.length; index++) {
        var tr = hearingTr.eq(index);
        var chk = tr.find('.hearingChk');
        if (chk.is(":checked")) {
            var hearingData = {
                'CaseID': chk.attr('data-caseID'),
                'PetitionID': chk.attr('data-petitionID')
            };
            data.BatchHearingList.push(hearingData);
        }
    }
    return data;
}

function saveData() {

    var data = JSON.stringify(getData());
    $.ajax({
        type: "POST", dataType: 'json', url: '/Task/BatchHearingSave', data: data, contentType: "application/json",
        success: function (result) {
            if (result.isSuccess) {
                Notify('Hearings has been saved successfully.', 'bottom-right', '4000', 'success', 'fa-check', true);
            }
            else {
                if (result.URL) {
                    window.location.href = result.URL;
                } else {
                    Notify("Something worng while proccessing data.", 'bottom-right', '4000', 'danger', 'fa-warning', true);
                }
            }
        }
    });
}

function validate() {
    var isValid = true;
    if ($("#HearingTypeID").val() == "") {
        $("#HearingTypeID").focus();
        Notify('Hearing Type is required.', 'bottom-right', '4000', 'danger', 'fa-warning', true);
        isValid = false;
    }
    else if ($("#HearingDate").val() == "") {
        $("#HearingDate").focus();
        Notify('Hearing Date is required.', 'bottom-right', '4000', 'danger', 'fa-warning', true);
        isValid = false;
    }
    else if ($("#Hours").val() == "") {
        $("#Hours").focus();
        Notify('Hearing Time is required.', 'bottom-right', '4000', 'danger', 'fa-warning', true);
        isValid = false;
    }
    else if ($("#Minutes").val() == "") {
        $("#Minutes").focus();
        Notify('Hearing Time is required.', 'bottom-right', '4000', 'danger', 'fa-warning', true);
        isValid = false;
    }
    else if ($("#TimeAmPm").val() == "") {
        $("#TimeAmPm").focus();
        Notify('Hearing Time is required.', 'bottom-right', '4000', 'danger', 'fa-warning', true);
        isValid = false;
    }
    else if ($("#HearingOfficerID").val() == "") {
        $("#HearingOfficerID").focus();
        Notify('Department is required.', 'bottom-right', '4000', 'danger', 'fa-warning', true);
        isValid = false;
    }
    else if ($("#DepartmentID").val() == "") {
        $("#DepartmentID").focus();
        Notify('Hearing Officer is required.', 'bottom-right', '4000', 'danger', 'fa-warning', true);
        isValid = false;
    }
    return isValid;
}

//$('#reset').on('click', function (e) {
//    e.preventDefault();
//    var $formErrorContainer = $('#search-validation-error');
//    $formErrorContainer.addClass('hidden');
//    $('#invoiceQueue-search-form *').val('');
//    $('#invoiceQueue-search-form input:text:first').focus();
//    oTable.fnClearTable();
//});
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
                    //console.log(calc_height + " - " + $(this).outerHeight(true));
                    calc_height = calc_height - $(this).outerHeight(true);
                }
            });
            _offset = _offset + $(this).outerHeight(true) - $(this).height();
        });

        calc_height = calc_height - _offset;
        // console.log("total: " + calc_height);
        $('#divSearchResult .dataTables_scrollBody').css('max-height', calc_height + 'px');
        oTable.fnAdjustColumnSizing();
    }
    return calc_height;
}
$(window).bind('resize', function () {
    fitCalculatedHeightForSearchDataTable();
});

$('#search').on('click', function () {
    loadData();
});

$('#btnAddHearing').on('click', function () {
    if (validate())
        saveData();
    else
        return false;
});

$(document).ready(function () {

});
