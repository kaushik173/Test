var oTable = $('#systemValueList').dataTable({
    "searching": false,
    "bSort": false,
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,

    "columns": [
    {
        "render": function (data, type, full, meta) {
            if (full.CanEditFlag == 1) {
                return ('<a href="/Administration/SystemValuesAddEdit/' + full.EncryptedSystemValueTypeID + '?systemValueTypeEntry=' + full.EncryptedSystemValue + '" data-secure-link-id="175">' + full.SystemValue + '</a>');
            }
            else {
                return full.SystemValue;
            }
        }
    },
    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    "deferRender": true,
    "fnDrawCallback": function (oSettings) {
        $(oSettings.nTHead).hide();
    }
});

if (device.mobile() || device.tablet()) {
    var middleColumn = oTable.DataTable().column(2);
    middleColumn.visible(false);
}

$(window).bind('resize', function () {
    $('#systemValueList').css('width', '100%');
});

function setData(data) {
    oTable.fnClearTable();
    if (data.data.length > 0) {
        oTable.fnAddData(data.data);
        fitCalculatedHeightForSearchDataTable();
    } else
        Notify('No results found.', 'bottom-right', '5000', 'blue', 'fa-frown-o', true);


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
        $('#divSearchResult .dataTables_scrollBody').css('max-height', calc_height + 'px');
        oTable.fnAdjustColumnSizing();
    }
    return calc_height;
}

function loadData(id)
{
    $.ajax({
        type: "POST", url: '/Administration/SystemLValueList/'+id,
        success: function (data) {
            setData(data);
        },
        dataType: 'json'
    });
}

$('#SystemValueTypeCodeTypeID').on('change', function () {
    loadData($(this).val());
});

$(document).ready(function () {
    //default load all data
    loadData('');
});