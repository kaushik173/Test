var page = (function () {
    return {
        $tableCurrent: $('#CurrentCodeTableView'),
        $tableOther: $('#OtherCodeTableView'),
        ttl: 600000 /*In Milli seconds*/
    };
})();

var oTableCurrent = $('#CurrentCodeTableView').dataTable({
    "searching": true,
    "bSort": false,
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,
    "columns": [
        {
            "data": "CodeValue",
            "render": function (data, type, full, meta) {
                return ('<a href="/CodeTables/EditCode/' + full.CodeID + '?typeId=' + $EncryptedCodeTypeID + '">' + data + '</a>');
            }
        },
        {
            "data": "CodeShortValue",
            "render": function (data, type, full, meta) {
                return ('<a href="/CodeTables/EditCode/' + full.CodeID + '?typeId=' + $EncryptedCodeTypeID + '">' + data + '</a>');
            }
        },
    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    "deferRender": true
});

var oTableOther = $('#OtherCodeTableView').dataTable({
    "searching": true,
    "bSort": false,
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,
    "columns": [
        {
            "data": "CodeValue",
            "render": function (data, type, full, meta) {
                return ('<a href="/CodeTables/EditCode/' + full.CodeID + '?typeId=' + $EncryptedCodeTypeID + '">' + data + '</a>');
            }
        },
        {
            "data": "CodeShortValue",
            "render": function (data, type, full, meta) {
                return ('<a href="/CodeTables/EditCode/' + full.CodeID + '?typeId=' + $EncryptedCodeTypeID + '">' + data + '</a>');
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

$('#CurrentCodeTableView').on('draw.dt', function () {
    if (device.mobile() || device.tablet())//if mobile or tablet
    {
        $('#CurrentCodeTableView_info').parent().addClass('col-xs-4').removeClass('col-xs-6');
        $('#CurrentCodeTableView_paginate').parent().addClass('col-xs-8').removeClass('col-xs-6');
    }

});

$('#OtherCodeTableView').on('draw.dt', function () {
    if (device.mobile() || device.tablet())//if mobile or tablet
    {
        $('#OtherCodeTableView_info').parent().addClass('col-xs-4').removeClass('col-xs-6');
        $('#OtherCodeTableView_paginate').parent().addClass('col-xs-8').removeClass('col-xs-6');
    }

});

if (!simpleStorage.get(page.mainKey)) {
    loadData();
}

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
    $('#CurrentCodeTableView').css('width', '100%');
    $('#OtherCodeTableView').css('width', '100%');
    fitCalculatedHeightForSearchDataTableCurrent();
});



function loadData() {
    $.ajax({
        type: "GET",
        url: '/CodeTables/GetCurrent?codeTypeID=' + $codeTypeID,
        success: function (result) {
            setDataCurrent(result.current);
            setDataOther(result.otherCodes);
        },
        dataType: 'json'
    });

    //$.ajax({
    //    type: "GET",
    //    url: '/CodeTables/GetOther?codeTypeID=' + $codeTypeID,
    //    success: function (data) {
    //        setDataOther(data);
    //    },
    //    dataType: 'json'
    //});
}

function setDataCurrent(data) {
    oTableCurrent.fnClearTable();
    if (data.data.length > 0) {
        oTableCurrent.fnAddData(data.data);
        fitCalculatedHeightForSearchDataTableCurrent();
    }// else
     //   $("#divSearchResultCurrent").hide();
}

function setDataOther(data) {
    oTableOther.fnClearTable();
    if (data.data.length > 0) {
        oTableOther.fnAddData(data.data);
        fitCalculatedHeightForSearchDataTableCurrent();
    } //else
      //  $("#divSearchResultOther").hide();
}

page.$tableCurrent.on('page.dt', function () {
    var table = page.$tableCurrent.dataTable();
    var pageInfo = table.fnPagingInfo();
    var pageNumber = pageInfo.iPage;
    simpleStorage.set(page.resultPageIdKey, pageNumber, { TTL: page.ttl });
});

$('#btnCancel').on('click', function () {
    window.location.href = "/CodeTables/List";
});

function fitCalculatedHeightForSearchDataTableCurrent() {
    var calc_height = 0;
    if (oTableCurrent != null) {
        calc_height = $(window).height();
        var _offset = 25;
        $("#divSearchResultCurrent .dataTables_scrollBody").children().first().parentsUntil("body").each(function () {
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
        $('#divSearchResultCurrent .dataTables_scrollBody').css('max-height', calc_height + 'px');
        oTableCurrent.fnAdjustColumnSizing();
    }
    return calc_height;
}

$("#btnAddCode").on("click", function () {
    window.location.href = "/CodeTables/AddCode/" + $EncryptedCodeTypeID;
});

$('#divSearchResultCurrent  .widget-buttons').html($('#CurrentCodeTableView_filter'));
$('#divSearchResultOther  .widget-buttons').html($('#OtherCodeTableView_filter'));