
var oTable = $('#searchHourlyInvoiceList').dataTable({
    //"lengthMenu": [20],
    //"lengthChange": false,
    "searching": false,
    "bSort": false,
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,

    "columns": [
        { "data": "InvoiceNumber" },
        {
            "data": "InvoiceDate",
            "render": function (data, type, full, meta) {
                return ('<a href="/HourlyInvoiceList/Edit/' + full.EncryptedHourlyInvoiceID + '">' + data + '</a>')
            }
        },
        { "data": "InvoiceAmount" },
        { "data": "Attorney" },
        { "data": "ApprovalDate" },
        { "data": "ApprovalAmount" },
        {
            "render": function (data, type, full, meta) {
                return (' <a class="btn btn-info btn-xs print auto-download" href="/HourlyInvoiceList/PrintHourlyInvoiceList/' + full.EncryptedHourlyInvoiceID + '""})"><i class="fa fa-print"></i> Print</a>')
            }
        }
    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    "deferRender": true


});
if (device.mobile() || device.tablet()) {
    var middleColumn = oTable.DataTable().column(2);
    middleColumn.visible(false);
}

$('#searchHourlyInvoiceList').on('draw.dt', function () {
    if (device.mobile() || device.tablet())//if mobile or tablet
    {
        $('#searchHourlyInvoiceList_info').parent().addClass('col-xs-4').removeClass('col-xs-6');
        $('#searchHourlyInvoiceList_paginate').parent().addClass('col-xs-8').removeClass('col-xs-6');
    }
});

$('#search').on('click', function () {
    Search();
});

function Search() {

    var $form = $('#HourlyInvoiceList-search-form');
    var formInvalid = $('#AttorneyID').val().length == 0
        && $('#InvoiceNumber').val().length == 0

    if (formInvalid) {
        Notify('Attorney or Invoice # is required.', 'bottom-right', '5000', 'danger', 'fa-warning', true);
        return false;
    }
    IPadKeyboardFix();

    var data = getData();
    $.ajax({
        type: "POST", url: '/HourlyInvoiceList/Search', data: data,
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
    } else
        Notify('No results found.', 'bottom-right', '5000', 'blue', 'fa-frown-o', true);


}

function getData() {
    var fData = $('#HourlyInvoiceList-search-form').serialize();
    return fData;
}

$("#btnAdd").on("click", function () {
    window.location.href = "/HourlyInvoiceList/Add/" + $("#AttorneyID").val();
});

$('#reset').on('click', function (e) {
    e.preventDefault();
    var $formErrorContainer = $('#search-validation-error');
    $formErrorContainer.addClass('hidden');
    $('#HourlyInvoiceList-search-form *').val('');
    $('#HourlyInvoiceList-search-form input:text:first').focus();

    oTable.fnClearTable();
});
$(window).bind('resize', function () {
    $('#searchHourlyInvoiceList').css('width', '100%');
    fitCalculatedHeightForSearchDataTable();
});
function onCaseNumberClick(e) {
    var $this = $(this);
    e.preventDefault();
    document.location = $this.attr('href');
}

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

$(document).ready(function () {
    if ($("#AttorneyID").val() != '') {
        Search();
    }
});