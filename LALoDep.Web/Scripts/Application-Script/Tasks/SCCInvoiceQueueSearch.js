
var currentAttorney = 0;
var page = (function () {
    return {
        $form: $('#invoiceQueue-search-form'),
        $table: $('#searchInvoiceQueue'),
        mainKey: GetWindowID() + 'invoiceQueue-search-form',
        rowSelectedKey: GetWindowID() + 'case-search-form-row#',
        ttl: 600000 /*In Milli seconds*/
    };
})();

var oTable = $('#searchInvoiceQueue').dataTable({
    //"lengthMenu": [20],
    //"lengthChange": false,
    "searching": false,
    "bSort": false,
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,

    "columns": [

        { "data": "Attorney" },
        { "data": "Client" },
        { "data": "Source" },
        { "data": "CaseNumber" },
        { "data": "NextDate" },
        { "data": "JcatsNumber" },
        { "data": "Type" },
        { "data": "Amount" },
        { "data": "Status" },
        { "data": "DateToBePaid" },
        { "data": "InvoiceDt" },
        {
            "data": "InvoiceNumber",
            "render": function (data, type, full, meta) {
                if ($canEditAccess) {
                    return ('<a class="lnkInvoiceNo" data-href="/SCCInvoiceQueue/SCCInvoiceAddEdit/' + full.EncryptedInvoiceID + '?caseID=' + full.EncryptedCaseID + '" style="cursor:pointer">' + full.InvoiceNumber + '</a>')
                }
                else {
                    return data;
                }
            }
        },
    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    "deferRender": true,
    "fnDrawCallback": function (oSettings) {
 //       $("a.lnkInvoiceNo").on('click', onCaseInvoiceClick);
    }
});

if (device.mobile() || device.tablet()) {
    var middleColumn = oTable.DataTable().column(2);
    middleColumn.visible(false);
}

$('#searchInvoiceQueue').on('draw.dt', function () {
    if (device.mobile() || device.tablet())//if mobile or tablet
    {
        $('#searchCase_info').parent().addClass('col-xs-4').removeClass('col-xs-6');
        $('#searchCase_paginate').parent().addClass('col-xs-8').removeClass('col-xs-6');
    }

});


function ResetPageState() {
    simpleStorage.deleteKey(page.mainKey);
    //simpleStorage.deleteKey(page.resultPageIdKey);
    simpleStorage.deleteKey(page.rowSelectedKey);
};

function SavePageState(results) {
    var formData = {};
    page.$form.serializeArray().map(function (item) {
        formData[item.name] = item.value;
    });
    var pageState = { formData: formData, results: results };
    simpleStorage.set(page.mainKey, pageState, { TTL: page.ttl });
};

function LoadPreviousPageState() {
    var pageState = simpleStorage.get(page.mainKey);
    if (pageState && pageState.formData && pageState.results) {

        /*Load Results*/
        setData(pageState.results);

        /*Load Page #*/
        //var pageNumber = simpleStorage.get(page.resultPageIdKey);
        //if (pageNumber) page.$table.dataTable().fnPageChange(pageNumber);

        /*Load Form inputs*/
        var formData = pageState.formData;
        $('#invoiceQueue-search-form *').filter(':input').each(function () {
            var $this = $(this);
            if (formData[$this.attr('id')])
                $this.val(formData[$this.attr('id')]);
        });

        /*Highlight selected Row*/
        var rowIndex = simpleStorage.get(page.rowSelectedKey);
        if (rowIndex >= 0) {
            page.$table.find("tr:nth-child(" + (rowIndex + 1) + ")").css("background", "#f3f7b4");
            page.$table.find("tr:nth-child(" + (rowIndex + 1) + ") td").css("background", "#f3f7b4");
        }

    }
}


function loadData() {
    var $form = $('#invoiceQueue-search-form');

    var formInvalid = false;

    if ($('#AttorneyId').val() == "" &&
        $('#SourceId').val() == ""
        && $('#InvoiceStatusId').val() == ""
        && $('#ClientLastName').val() == ""
        && $('#ClientFirstName').val() == ""
        && $('#CaseID').val() == ""
        && $('#CourtNumber').val() == ""
        && $('#InvoiceNumber').val() == ""
        && $('#SCCInvoicePaidDateStart').val() == ""
        && $('#SCCInvoicePaidDateEnd').val() == "" && $onViewLoad != 'True') {
        formInvalid = true;
    }

    if (formInvalid) {
        Notify('At least one search parameter is required.', 'bottom-right', '4000', 'danger', 'fa-warning', true);
        return false;
    }
    IPadKeyboardFix();

    //if ($onViewLoad != 'True') {
    var data = getData();
    $.ajax({
        type: "POST",
        url: '/SCCInvoiceQueue/Search',
        data: data,
        success: function (data) {
            setData(data);
            SavePageState(data);
        },
        dataType: 'json'
    });
    //}$onViewLoad = 'False';
}

function setData(data) {
    oTable.fnClearTable();
    if (data.data.length > 0) {
        oTable.fnAddData(data.data);
        fitCalculatedHeightForSearchDataTable();
        $('#btnPrint').removeClass('hidden');
    } else {
        $('#btnPrint').addClass('hidden');
        Notify('No results found.', 'bottom-right', '3000', 'blue', 'fa-frown-o', true);
    }
}

function getData() {
    var fData = $('#invoiceQueue-search-form').serialize();
    return fData;
}

function onCaseInvoiceClick(e) {
    e.preventDefault();
    var $this = $(this);
    simpleStorage.set(page.rowSelectedKey, $this.closest('tr').index(), { TTL: page.ttl });
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
                    //console.log(calc_height + " - " + $(this).outerHeight(true));
                    calc_height = calc_height - $(this).outerHeight(true);
                }
            });
            _offset = _offset + $(this).outerHeight(true) - $(this).height();
        });

        calc_height = calc_height - _offset;
        // console.log("total: " + calc_height);        

        calc_height = calc_height < 250 ? 250 : calc_height;
        $('#divSearchResult .dataTables_scrollBody').css('max-height', calc_height + 'px');
        oTable.fnAdjustColumnSizing();
    }
    return calc_height;
}

$('#reset').on('click', function (e) {
    e.preventDefault();
    var $formErrorContainer = $('#search-validation-error');
    $formErrorContainer.addClass('hidden');
    $('#invoiceQueue-search-form *').val('');
    $('#invoiceQueue-search-form input:text:first').focus();
    oTable.fnClearTable();
});

$(window).bind('resize', function () {
    $('#searchInvoiceQueue').css('width', '100%');
    fitCalculatedHeightForSearchDataTable();
});

$('#search').on('click', function () {
    loadData();
});

$('#btnPrint').on('click', function () {
    var _target = $("body").data("print-document-on") == "NewWindow" ? 'target="_blank"' : '';
    var data = {
        id: 1
    };
    $.download($('#hdnCurrentSessionGuidPath').val() + '/SCCInvoiceQueue/PrintSSCInvoiceQueue', data, "POST", _target);


});

$('#add').on('click', function () {
    OpenPopup("/SCCInvoiceQueue/SCCInvoiceAddEdit", 'View Invoice');
});

$("#btnReset").on("click", function (e) {
    e.preventDefault();
    $("#invoiceQueue-search-form")[0].reset()
    oTable.fnClearTable();
    fitCalculatedHeightForSearchDataTable();
});

$(document).ready(function () {
    if ($onViewLoad == "True") {
        ResetPageState();
        loadData();
    } else {
        if ($loadFromCache == "True")
            LoadPreviousPageState();
        else
            ResetPageState();
    }

    if ($IsAdd != '') {
        $('#add').removeClass('hidden');
    }




    $('body').on('click', '.lnkInvoiceNo', function (e) {
        e.preventDefault();
       

        OpenPopup($(this).attr('data-href'), 'View Invoice');
        return false;
    })
});
function RefreshAndClosePopup() {
    loadData();
    ClosePopup();
}