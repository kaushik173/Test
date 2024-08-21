var page = (function () {
    return {
        $table: $('#HearingRate'),
        ttl: 600000 /*In Milli seconds*/
    };
})();

var oTable = $('#HearingRate').dataTable({
    //"lengthMenu": [20],
    //"lengthChange": false,
    "searching": false,
    "bSort": false,
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,

    "columns": [
        { "data": "HearingType" },
        {
            "render": function (data, type, full, meta) {
                return ('<a href="/HearingRates/AddEdit?AgencyID=' + full.EncryptedMCO + '&HearingTypeID=' + full.EncryptedHearingTypeID + '">' + (full.MCOHearingRate == null ? "Add" : full.MCOHearingRate) + '</a>');
            }
        },
        {
            "render": function (data, type, full, meta) {
                return ('<a href="/HearingRates/AddEdit?AgencyID=' + full.EncryptedCPO + '&HearingTypeID=' + full.EncryptedHearingTypeID + '">' + (full.CPOHearingRate == null ? "Add" : full.CPOHearingRate) + '</a>');
            }
        },
        {
            "render": function (data, type, full, meta) {
                return ('<a href="/HearingRates/AddEdit?AgencyID=' + full.EncryptedPPO + '&HearingTypeID=' + full.EncryptedHearingTypeID + '">' + (full.PPOHearingRate == null ? "Add" : full.PPOHearingRate) + '</a>');
            }
        },
        {
            "render": function (data, type, full, meta) {
                return ('<a href="/HearingRates/AddEdit?AgencyID=' + full.EncryptedCCO + '&HearingTypeID=' + full.EncryptedHearingTypeID + '">' + (full.CCOHearingRate == null ? "Add" : full.CCOHearingRate) + '</a>');
            }
        }
    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    "fnDrawCallback": function (oSettings) {
        $("a.lnkCaseNumber").on('click', onCaseNumberClick);
    },
    "deferRender": true
});
var oTableApi = oTable.dataTable().api();

if (device.mobile() || device.tablet()) {
    var middleColumn = oTable.DataTable().column(2);
    middleColumn.visible(false);
}

$('#HearingRate').on('draw.dt', function () {
    if (device.mobile() || device.tablet()) //if mobile or tablet
    {
        $('#HearingRatey_info').parent().addClass('col-xs-4').removeClass('col-xs-6');
        $('#HearingRate_paginate').parent().addClass('col-xs-8').removeClass('col-xs-6');
    }

});


//Mousetrap.bind(['command+alt+shift+r', 'alt+shift+r'], function () { $('#reset').trigger('click'); });
if ($onViewLoad == "True") {
    debugger;
    $.ajax({
        type: "GET",
        url: '/HearingRates/SearchHearingData',
        success: function (data) {
            debugger;
            setData(data);
        },
        dataType: 'json'
    });
}

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

function setData(data) {
    debugger;
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

function onCaseNumberClick(e) {
    var $this = $(this);
    e.preventDefault();
    simpleStorage.set(page.rowSelectedKey, $this.closest('tr').index(), { TTL: page.ttl });
    document.location = $this.attr('href');
}

page.$table.on('page.dt', function () {
    var table = page.$table.dataTable();
    var pageInfo = table.fnPagingInfo();
    var pageNumber = pageInfo.iPage;
    simpleStorage.set(page.resultPageIdKey, pageNumber, { TTL: page.ttl });
});

function fitCalculatedHeightForSearchDataTable() {
    debugger;
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