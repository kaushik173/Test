var page = (function () {
    return {
        $table: $('#CodeTableView'),
        ttl: 600000 /*In Milli seconds*/
    };
})();

var oTable = $('#CodeTableView').dataTable({
    "searching": true,
    "bSort": false,
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,
    "columns": [
        {
            "render": function (data, type, full, meta) {
                return ('<a href="/CodeTables/Values?codeTypeID=' + full.CodeTypeID + '">' + full.CodeTypeValue + '</a>');
            }
        }
    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    "deferRender": true,  
});

if (device.mobile() || device.tablet()) {
    var middleColumn = oTable.DataTable().column(2);
    middleColumn.visible(false);
}

$('#CodeTableView').on('draw.dt', function () {
    if (device.mobile() || device.tablet())//if mobile or tablet
    {
        $('#CodeTableView_info').parent().addClass('col-xs-4').removeClass('col-xs-6');
        $('#CodeTableView_paginate').parent().addClass('col-xs-8').removeClass('col-xs-6');
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
    $('#CodeTableView').css('width', '100%');
});

function loadData() {
    $.ajax({
        type: "GET", url: '/CodeTables/GetCodeTableData',
        success: function (data) {
            setData(data);         
        },
        dataType: 'json'
    });

}
$('.widget-buttons').html($('#CodeTableView_filter'));
function setData(data) {
    oTable.fnClearTable();
    if (data.data.length > 0) {
        oTable.fnAddData(data.data);
        fitCalculatedHeightForSearchDataTable();
    } else
        Notify('No results found.', 'bottom-right', '5000', 'blue', 'fa-frown-o', true);
}

$(window).bind('resize', function () {
    fitCalculatedHeightForSearchDataTable();
});

page.$table.on('page.dt', function () {
    var table = page.$table.dataTable();
    var pageInfo = table.fnPagingInfo();
    var pageNumber = pageInfo.iPage;
    simpleStorage.set(page.resultPageIdKey, pageNumber, { TTL: page.ttl });
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