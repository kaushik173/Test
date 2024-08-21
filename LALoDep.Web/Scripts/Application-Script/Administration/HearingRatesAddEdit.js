var page = (function () {
    return {
        $table: $('#Training'),
        ttl: 600000 /*In Milli seconds*/
    };
})();

var oTable = $('#AddEditHearingRates').dataTable({
    "searching": false,
    "bSort": false,
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,
    "columns": [
        {
            "render": function (data, type, full, meta) {
                return ('<input type="text" class="form-control input-sm rate full-wdth" value="' + (full.HearingRate !== null && full.HearingRate !== undefined?full.HearingRate:"") + '">');
            }
        },
        {
            "render": function (data, type, full, meta) {          
                return ('<input type="text" class="datepicker form-control input-sm startdate full-wdth" value="' + (full.StartDate !== null && full.StartDate !== undefined ? full.StartDate : "") + '">');
            }
        },
                {
                    "render": function (data, type, full, meta) {
                        return ('<input type="text" class="datepicker form-control input-sm enddate full-wdth" value="' + (full.EndDate !== null && full.EndDate !== undefined ? full.EndDate : "") + '">');
                    }
                },
                        {
                            "render": function (data, type, full, meta) {
                                return ('<input type="checkbox" class="delete">');
                            }
                        }
    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    "deferRender": true,
    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
        nRow.Id = aData.HearingRateID;
        return nRow;
    }
});


var oTableApi = oTable.dataTable().api();

if (device.mobile() || device.tablet()) {
    var middleColumn = oTable.DataTable().column(2);
    middleColumn.visible(false);
}

$('#AddEditHearingRates').on('draw.dt', function () {
    if (device.mobile() || device.tablet())//if mobile or tablet
    {
        $('#AddEditHearingRates_info').parent().addClass('col-xs-4').removeClass('col-xs-6');
        $('#AddEditHearingRates_paginate').parent().addClass('col-xs-8').removeClass('col-xs-6');
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


$('#SaveExit').on('click', function () {
    saveData();
});

$('#SaveAdd').on('click', function () {
    var tableData = getData();
    var validatedData = ValidateSaveAdd(tableData);
    if (validatedData == "true") {       
        var newRow = '<tr role="row" isNew="1" class=""><td><input type="text" class="form-control input-sm rate full-wdth" ></td>' +
            '<td><input type="text" class="datepicker form-control input-sm  startdate full-wdth" ></td>' +
            '<td><input type="text" class="datepicker form-control input-sm  enddate full-wdth" ></td>' +
            '<td><input type="checkbox" class="delete"></td></tr>';
        oTableApi.row.add($(newRow)).draw();
        initDatePicker();
    } else {
        Notify(validatedData, 'bottom-right', '5000', 'red', 'fa-frown-o', true);
    }
});

//$("#cancel").on('click', function () {
//    window.location.href = "/TrainingSummary/Search";
//});

$(window).bind('resize', function () {
    $('#AddEditHearingRates').css('width', '100%');
});

function loadData() {
    $.ajax({
        type: "GET", url: '/HearingRates/AddEditHearingRates?AgencyID=' + $agencyID + '&HearingTypeID=' + $hearingTypeID,
        success: function (data) {
            setData(data);
            initDatePicker();           
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

function initDatePicker() {

    $(".datepicker").on('show', function () {
        $(this).attr('mustHide', "1");
        $('.datepicker[mustHide="0"]').datepicker('hide');
        $(this).attr('mustHide', "0");
    });

    $('.datepicker').on('changeDate', function () {
        $(this).datepicker('hide');
    });

    $(".datepicker").each(function () {
        $(this).datepicker();
    });  
}

function getData() {
    var data = new Array();
    var rows = $('#AddEditHearingRates tr:not(":has(th)")');
    rows.each(function () {
        var rate = $(this).children("td").children("input.rate").val();
        var startDate = $(this).children("td").children("input.startdate").val();
        var endDate = $(this).children("td").children("input.enddate").val();
        var del = $(this).children("td").children("input.delete").prop("checked");
        if (typeof $(this).attr('isNew') !== "undefined") {
            data.push({ HearingRateId: $(this).prop("Id"), HearingRate: rate, DateStartDate: startDate, DateEndDate: endDate, Deleted: del, NewRow: true });
        } else {
            data.push({ HearingRateId: $(this).prop("Id"), HearingRate: rate, DateStartDate: startDate, DateEndDate: endDate, Deleted: del, NewRow: false });
        }
    });
    return data;
}
function saveData() {
    IPadKeyboardFix();
    var tableData = getData();
    var data = {
        rates: tableData,
        agencyId: $agencyID,
        hearingTypeId:$hearingTypeID
    } 
    var validatedData = Validate(tableData);    
    if (validatedData == "true") {
        $.ajax({
            type: "POST",
            url: '/HearingRates/SaveDeleteHearingRates/',
            data: data,
            success: function (data) {
                window.location.replace("/HearingRates");
            },
            dataType: 'json'
        });
    } else {
        Notify(validatedData, 'bottom-right', '5000', 'red', 'fa-frown-o', true);
    }
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

function ValidateSaveAdd(data) {
    var pEndDate;
    var message;
    $.each(data, function (index, value) {
        if (value.DateStartDate > value.DateEndDate) {
            message = "Date range invalid";
            return false;
        } else if (value.DateStartDate < pEndDate) {
            message = "Date range invalid";
            return false;
        } else {
            pEndDate = value.DateEndDate;
            message = "true";
            return true;
        }
    });
    return message;
}

function Validate(data) {
    var pEndDate;
    var message;
    $.each(data, function (index, value) {
        if (index != data.length - 1 || data.length == 1) {          
            if (value.DateStartDate > value.DateEndDate) {
                message = "Date range invalid";
                return false;
            } else if (value.DateStartDate < pEndDate) {
                message = "Date range invalid";
                return false;
            } else {
                pEndDate = value.DateEndDate;
                message = "true";
                return true;
            }
        } else {
            if (value.DateStartDate < pEndDate) {
                message = "Date range invalid";
                return false;
            } else {
                return true;
            }
        }
    });
    return message;
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
        $('#divSearchResult .dataTables_scrollBody').css('max-height', calc_height + 'px');
        oTable.fnAdjustColumnSizing();
    }
    return calc_height;
}