//var page = (function () {
//    return {
//        $form: $('#MonthlyInvoiceList-search-form'),
//        $table: $('#searchMonthlyInvoiceList'),
//        mainKey: 'MonthlyInvoiceList-search-form',
//        resultPageIdKey: 'MonthlyInvoiceList-search-form-page#',
//        rowSelectedKey: 'MonthlyInvoiceList-search-form-row#',
//        ttl: 600000 /*In Milli seconds*/
//    };
//})();



var oTable = $('#searchMonthlyInvoiceList').dataTable({
    //"lengthMenu": [20],
    //"lengthChange": false,
    "searching": false,
    "bSort": false,
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,

    "columns": [
        { "data": "Attorney" },
          { "data": "County" },
        { "data": "YearMonth" },
         {
             "data": "InvoiceNumber",
             "render": function (data, type, full, meta) {
                 return '<a href="/MonthlyInvoiceList/EditInvoice/' + full.EncryptedID + '" data-secure-link-id="371">' + data + '</a>';
             }
         },
        { "data": "AmountNumber" },
        { "data": "SubmitDate" },
        { "data": "Status" },
        { "data": "StatusDate" },
          {
              "data": "InvoiceNumber",
              "render": function (data, type, full, meta) {
                  return '<a data-id="' + full.InvoiceNumber + '" class="btnPrint btn btn-info btn-sm" >Print</a>';
              }
          },
    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    
    "deferRender": true
}); 

if (device.mobile() || device.tablet()) {
    var middleColumn = oTable.DataTable().column(2);
    middleColumn.visible(false);
}

$('#searchMonthlyInvoiceList').on('draw.dt', function () {
    if (device.mobile() || device.tablet())//if mobile or tablet
    {
        $('#searchMonthlyInvoiceList_info').parent().addClass('col-xs-4').removeClass('col-xs-6');
        $('#searchMonthlyInvoiceList_paginate').parent().addClass('col-xs-8').removeClass('col-xs-6');
    }

});



//if (simpleStorage.get(page.mainKey)) {
    
//    LoadPreviousPageState();
//}

function search() {
    var $form = $('#MonthlyInvoiceList-search-form');
    var formInvalid = $('#AgencyID').val().length == 0
        && $('#AttorneyID').val().length == 0
        && $('#InvoiceNumber').val().length == 0
        && $('#StatusCodeID').val().length == 0;


    //if (formInvalid) {
    //    Notify('At least one search parameter is required.', 'bottom-right', '5000', 'danger', 'fa-warning', true);
    //    return false;
    //}
    IPadKeyboardFix();

    var data = getData();
    $.ajax({
        type: "POST", url: '/MonthlyInvoiceList/Search', data: data,
        success: function (data) {
            setData(data);
            //SavePageState(data);
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
    var fData = $('#MonthlyInvoiceList-search-form').serialize();
    return fData;
}

//function LoadPreviousPageState() {
//    var pageState = simpleStorage.get(page.mainKey);
//    if (pageState && pageState.formData && pageState.results) {

//        /*Load Results*/
//        setData(pageState.results);

//        /*Load Page #*/
//        var pageNumber = simpleStorage.get(page.resultPageIdKey);
//        if (pageNumber) page.$table.dataTable().fnPageChange(pageNumber);

//        /*Load Form inputs*/
//        var formData = pageState.formData;
//        $('#MonthlyInvoiceList-search-form *').filter(':input').each(function () {
//            var $this = $(this);
//            if (formData[$this.attr('id')])
//                $this.val(formData[$this.attr('id')]);
//        });

//        /*Highlight selected Row*/
//        var rowIndex = simpleStorage.get(page.rowSelectedKey);
//        if (rowIndex) {
//            page.$table.find("tr:nth-child(" + (rowIndex + 1) + ")").css("background", "#f3f7b4");
//        }

//    }
//};

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

//function ResetPageState() {
//    simpleStorage.deleteKey(page.mainKey);
//    simpleStorage.deleteKey(page.resultPageIdKey);
//    simpleStorage.deleteKey(page.rowSelectedKey);
//};

//function SavePageState(results) {
//    var formData = {};
//    page.$form.serializeArray().map(function (item) {
//        formData[item.name] = item.value;
//    });
//    var pageState = { formData: formData, results: results };

//    simpleStorage.set(page.mainKey, pageState, { TTL: page.ttl });
//};

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
    search();
});
$('body').on('click', '.btnPrint', function (e) {
    e.preventDefault();
    var ids = $(this).data('id');

    var data = {
        id: ids

    }

   $.download($('#hdnCurrentSessionGuidPath').val()+'/MonthlyInvoiceList/PrintInvoice/' + ids, data, "POST", 'target="_blank"');

});

$('#btnAdd').on('click', function () {
    if ($('#AttorneyPersonID').val() !== '') {
        if (moment($('#AsOfDate').val()) > moment()) {
          
            notifyDanger('As Of Date cannot be a future date');
            $('#AsOfDate').focus();
            return false;
        }
        window.location.href = "/MonthlyInvoiceList/AddInvoice?addInvoiceFor=" + $('#AttorneyPersonID').val() + '&AsOfDate=' + $('#AsOfDate').val();
    } else {
        window.location.href = "/MonthlyInvoiceList/AddInvoice";
    }
   
});

$('#reset').on('click', function (e) {
    e.preventDefault();
    var $formErrorContainer = $('#search-validation-error');
    $formErrorContainer.addClass('hidden');
    $('#MonthlyInvoiceList-search-form *').val('');
    $('#MonthlyInvoiceList-search-form input:text:first').focus();
    //ResetPageState();
    oTable.fnClearTable();
});

$(window).bind('resize', function () {
    $('#searchMonthlyInvoiceList').css('width', '100%');
    fitCalculatedHeightForSearchDataTable();
});

//page.$table.on('page.dt', function () {
//    var table = page.$table.dataTable();
//    var pageInfo = table.fnPagingInfo();
//    var pageNumber = pageInfo.iPage;
//    simpleStorage.set(page.resultPageIdKey, pageNumber, { TTL: page.ttl });
//});

$(function(){

    search();
})