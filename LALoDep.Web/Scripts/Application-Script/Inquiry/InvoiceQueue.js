var page = (function () {
    return {
        $form: $('#InvoiceQueue-search-form'),
        $table: $('#searchInvoiceQueue'),
        mainKey: 'InvoiceQueue-search-form' + GetWindowID(),
        resultPageIdKey: GetWindowID() + 'InvoiceQueue-search-form-page#',
        rowSelectedKey: GetWindowID() + 'InvoiceQueue-search-form-row#',
        ttl: 600000 /*In Milli seconds*/
    };
})();

$(document).ready(function () {

    /*Load Default state of the form*/
    if ($loadFromCache && simpleStorage.get(page.mainKey)) {
        LoadPreviousPageState();
    }
    else {
        ResetPageState();
        loadData();
    }
});

var oTable = $('#searchInvoiceQueue').dataTable({
    //"lengthMenu": [20],
    //"lengthChange": false,
    "searching": false,
    "bSort": false,
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,

    "columns": [
        { "data": "InvoiceDate" },
        {
            "render": function (data, type, full, meta) {
                return ('<input type="checkbox" id="' + "full" + '">');
            }
        },
        {
            "data": "Status",
            "render": function (data, type, full, meta) {
                return '<a href="javascript:void(0);" class="statusLink" data-id="' + full.InvoiceID + '">' + data + '</a>';
            }
        },
        {
            "data": "Client",
            "render": function (data, type, full, meta) {
                return '<a href="/Case/Main/' + full.CaseID + '">' + data + '</a>';
            }
        },
        { "data": "PetitionNumber" },
        { "data": "Hearing" },
        { "data": "Division" },
        { "data": "Branch" }
    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    "fnDrawCallback": function (oSettings) {
        $("a.lnkCaseNumber").on('click', onCaseNumberClick);
    },
    "deferRender": true
}); /*.on('page.dt', function () {
                var table = $('#searchCase').dataTable();
                var pageNumber = table.fnSettings()._iDisplayStart / table.fnSettings()._iDisplayLength
                simpleStorage.set('case-search-form-page#', pageNumber, { TTL: 600000 });
            });*/

if (device.mobile() || device.tablet()) {
    var middleColumn = oTable.DataTable().column(2);
    middleColumn.visible(false);
}

$('#searchInvoiceQueue').on('draw.dt', function () {
    if (device.mobile() || device.tablet())//if mobile or tablet
    {
        $('#searchInvoiceQueue_info').parent().addClass('col-xs-4').removeClass('col-xs-6');
        $('#searchInvoiceQueue_paginate').parent().addClass('col-xs-8').removeClass('col-xs-6');
    }

});

$("#searchInvoiceQueue").on("click", ".statusLink", function () {
    var id = $(this).data("id");
    OpenPopup('/InvoiceQueue/InvoiceVerifyPopup/' + id, 'Invoice');
    //setTimeout(200, function () {  });
});

//Mousetrap.bind(['command+alt+shift+r', 'alt+shift+r'], function () { $('#reset').trigger('click'); });

//var _oldStopCallback = Mousetrap.stopCallback;
//Mousetrap.stopCallback = function (e, element, combo) {
//    if (combo == 'alt+shift+r' || combo == 'command+alt+shift+r') return false;
//    return _oldStopCallback(e, element, combo);
//}
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

$('#search').on('click', function () {
    var $form = $('#InvoiceQueue-search-form');
    $form.submit();
});

$('#print').on('click', function () {
    //$.ajax({
    //    type: "POST", url: '/SCCInvoiceQueue/PrintSSCInvoiceQueue',
    //    success: function (returnValue) {
    //        if (returnValue.FileName != '') {
    //            window.location = '/Inquiry/Download?file=' + returnValue.FileName;
    //        }
    //    },
    //    dataType: 'json'
    //});
    var data = {
        id: 1
    };
    var _target = $("body").data("print-document-on") == "NewWindow" ? 'target="_blank"' : '';

   $.download($('#hdnCurrentSessionGuidPath').val()+'/InvoiceQueue/PrintInvoiceQueue', data, "POST", _target);


});
//href="/Case/Main/' + full.ClientCaseId + '"
$(window).bind('resize', function () {
    $('#searchInvoiceQueue').css('width', '100%');
});

$('#InvoiceQueue-search-form').on('submit', function (e) {
    e.preventDefault();
    IPadKeyboardFix();
    var $form = $('#InvoiceQueue-search-form');
    //var formInvalid = $('#AgencyID').val().length == 0
    //    && $('#AttorneyID').val().length == 0
    //    && $('#InvoiceNumber').val().length == 0
    //    && $('#StatusCodeID').val().length == 0


    //if (formInvalid) {
    //    Notify('At least one search parameter is required.', 'bottom-right', '5000', 'danger', 'fa-warning', true);
    //    return false;
    //}
    IPadKeyboardFix();

    loadData();
});

function loadData() {
    data = $('#InvoiceQueue-search-form').serialize();
    IPadKeyboardFix();
    $.ajax({
        type: "POST", url: '/InvoiceQueue/Search', data: data,
        success: function (data) {
            setData(data);
            SavePageState(data);
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
    var fData = $('#InvoiceQueue-search-form').serialize();
    return fData;
}


$('#reset').on('click', function (e) {
    e.preventDefault();
    var $formErrorContainer = $('#search-validation-error');
    $formErrorContainer.addClass('hidden');
    $('#InvoiceQueue-search-form *').val('');
    $('#InvoiceQueue-search-form input:text:first').focus();
    ResetPageState();
    oTable.fnClearTable();
});
$(window).bind('resize', function () {
    fitCalculatedHeightForSearchDataTable();
});
function onCaseNumberClick(e) {
    var $this = $(this);
    e.preventDefault();
    simpleStorage.set(page.rowSelectedKey, $this.closest('tr').index(), { TTL: page.ttl });
    document.location = $this.attr('href');
}


function ResetPageState() {
    simpleStorage.deleteKey(page.mainKey);
    simpleStorage.deleteKey(page.resultPageIdKey);
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

page.$table.on('page.dt', function () {
    var table = page.$table.dataTable();
    var pageInfo = table.fnPagingInfo();
    var pageNumber = pageInfo.iPage;
    simpleStorage.set(page.resultPageIdKey, pageNumber, { TTL: page.ttl });
});

function LoadPreviousPageState() {
    var pageState = simpleStorage.get(page.mainKey);
    if (pageState && pageState.formData && pageState.results) {

        /*Load Results*/
        setData(pageState.results);

        /*Load Page #*/
        var pageNumber = simpleStorage.get(page.resultPageIdKey);
        if (pageNumber) page.$table.dataTable().fnPageChange(pageNumber);

        /*Load Form inputs*/
        var formData = pageState.formData;
        $('#InvoiceQueue-search-form *').filter(':input').each(function () {
            var $this = $(this);
            if (formData[$this.attr('id')])
                $this.val(formData[$this.attr('id')]);
        });

        /*Highlight selected Row*/
        var rowIndex = simpleStorage.get(page.rowSelectedKey);
        if (rowIndex) {
            page.$table.find("tr:nth-child(" + (rowIndex + 1) + ")").css("background", "#f3f7b4");
        }

    }
};

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
        $('#divSearchResult .dataTables_scrollBody').css('max-height', calc_height + 'px');
        oTable.fnAdjustColumnSizing();
    }
    return calc_height;
}