//var page = (function () {
//    return {
//        $form: $('#CountyCounselView-search-form'),
//        $table: $('#CountyCounselView'),
//        mainKey: 'CountyCounselView-search-form',
//        resultPageIdKey: 'CountyCounselView-search-form-page#',
//        rowSelectedKey: 'CountyCounselView-search-form-row#',
//        ttl: 600000 /*In Milli seconds*/
//    };
//})();


var oTable = $('#CountyCounselView').dataTable({
    //"lengthMenu": [20],
    //"lengthChange": false,
    "searching": false,
    "bSort": false,
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,

    "columns": [
        {
            "render": function(data, type, full, meta) {
                return ('<a href="/CountyCounselList/AddEditCountyCounsel/' + full.PersonID + '">' + full.LastName + '</a>');
            }
        },
        { "data": "FirstName" },
        { "data": "StartDate" },
        { "data": "EndDate" },
        { "data": "BarNumber" },
        {
            "render": function(data, type, full, meta) {
                return (' <a class="btn btn-info btn-xs contact" href="/Users/UserContact/' + full.PersonID + '?pageid=2"})"><i class="fa fa-edit"></i> Contact</a>');
            }
        },
        {
            "render": function(data, type, full, meta) {
                return (' <a class="btn btn-danger btn-xs delete" data-id="'+full.PersonID+'"><i class="fa fa-trash-o"></i> Delete</a>');
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

$('#CountyCounselView').on('draw.dt', function () {
    if (device.mobile() || device.tablet())//if mobile or tablet
    {
        $('#CountyCounselView_info').parent().addClass('col-xs-4').removeClass('col-xs-6');
        $('#CountyCounselView_paginate').parent().addClass('col-xs-8').removeClass('col-xs-6');
    }

});

$('#search').on('click', function () {
    loadData();
});

$('#btnAddNew').on('click', function () {
    window.location.href = "/CountyCounselList/AddEditCountyCounsel";
});



//href="/Case/Main/' + full.ClientCaseId + '"
$(window).bind('resize', function () {
    $('#CountyCounselView').css('width', '100%');
});

function loadData() {
    var $form = $('#CountyCounselView-search-form');
    IPadKeyboardFix()

    var data = getData();

    $.ajax({
        type: "POST", url: '/CountyCounselList/Search', data: data,
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
    var fData = $('#CountyCounselView-search-form').serialize();
    return fData;
}

$(window).bind('resize', function () {
    fitCalculatedHeightForSearchDataTable();
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

$(function () {

    $('body').on('click', '.delete', function () {
        var id = $(this).attr('data-id');
        confirmBox("Are you sure you want to remove selected records?", function (result) {
            if (result) {

                var tr = $(this).parent().parent();
                $.ajax({
                    type: "POST", url: '/CountyCounselList/CountyCounselDelete/' + id,
                    dataType: "json",
                    success: function (data) {
                        if (data.isSuccress) {
                            tr.remove();
                            Notify('Delete successfully.', 'bottom-right', '5000', 'success', 'fa-check', true);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                    }
                });
            }
        });
    });

});



