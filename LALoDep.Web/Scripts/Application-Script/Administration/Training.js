var page = (function () {
    return {
        $form: $('#Training-search-form'),
        $table: $('#Training'),
        mainKey: 'Training-search-form',
        resultPageIdKey: 'Training-search-form-page#',
        rowSelectedKey: 'Training-search-form-row#',
        ttl: 600000 /*In Milli seconds*/
    };
})();


var oTable1 = $('#TrainingSummary').dataTable({
    "searching": false,
    "bSort": false,
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,

    "columns": [
        { "data": "CreditType" },
        { "data": "P", className: "text-right" },
        { "data": "NP", className: "text-right" },
        { "data": "Total", className: "text-right" },

    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    "deferRender": true
});

var oTable = $('#Training').dataTable({
    "searching": false,
    "bSort": false,
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,

    "columns": [
        {
            "data": "CourseTitle",
            "render": function (data, type, full, meta) {
                return ('<a href="/TrainingSummary/TrainingEdit/' + full.EncryptedTrainingID + '?personID=' + full.EncryptedPersonID + '">' + data + '</a>');
            }
        },
        { "data": "StartDate" },
        { "data": "CreditType" },
        { "data": "Venue" },
        { "data": "Hours", className: "text-right" },
          
        {
            "render": function (data, type, full, meta) {
                return ('<a class="btn btn-danger btn-xs delete" data-secure-id="124" data-id="' + full.EncryptedTrainingID + '"><i class="fa fa-trash-o"></i>Delete</a>');
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

$('#Training').on('draw.dt', function () {
    if (device.mobile() || device.tablet())//if mobile or tablet
    {
        $('#Training_info').parent().addClass('col-xs-4').removeClass('col-xs-6');
        $('#Training_paginate').parent().addClass('col-xs-8').removeClass('col-xs-6');
    }

});

$('#filter').on('click', function () {
    loadData();
});

$("#cancel").on('click', function () {
    window.location.href = "/TrainingSummary/Search";
});

$("#addTrainingEntry").on('click', function () {
    window.location.href = "/TrainingSummary/TrainingEdit?personID=" + $(this).attr('data-id');
});

$('#print').on('click', function () {

    //var data = {
    //    'PersonID': $('#PersonID').val(),
    //    'StartDate': $('#StartDate').val(),
    //    'EndDate': $('#EndDate').val(),
    //}
    //$.download('/TrainingSummary/TraniningList', data, "POST");
});

$('body').on('click', '.delete', function () {
    var id = $(this).attr('data-id');
    var tr = $(this).parent().parent();

    confirmBox("Are you sure you want to remove selected records?", function (result) {
        if (result) {
            $.ajax({
                type: "POST", url: '/Administration/TrainingDelete/' + id,
                dataType: "json",
                success: function (data) {
                    if (data.isSuccess) {
                        tr.remove();
                        notifySuccess('Selected record delete successfully.');
                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                }
            });
        }
    });
    //$.download('/TrainingSummary/TraniningList', data, "POST");
});



$(window).bind('resize', function () {
    $('#Training').css('width', '100%');
});

function loadData() {
    var $form = $('#trainingSummary-search-form');

    IPadKeyboardFix();

    var data = getData();

    $.ajax({
        type: "POST", url: '/Administration/GetTrainingSummaryData', data: data,
        success: function (data) {
            setDataSummary(data, oTable1);
        },
        dataType: 'json'
    });

    $.ajax({
        type: "POST", url: '/Administration/GetTrainingData', data: data,
        success: function (data) {
            setData(data, oTable);
        },
        dataType: 'json'
    });

}

function setDataSummary(data, table) {
    table.fnClearTable();
    if (data.data.length > 0) {
        table.fnAddData(data.data);
        $('#TrainingSummary').find('tr').last().addClass('grandTotal')
        fitCalculatedHeightForSearchDataTable();
    }
}

function setData(data, table) {
    table.fnClearTable();
    if (data.data.length > 0) {
        table.fnAddData(data.data);

    } else
        notifyBlue('No results found.', 'bottom-right');
}

function getData() {
    var fData = $('#Training-search-form').serialize();
    return fData;
}

function fitCalculatedHeightForSearchDataTable() {
    var calc_height = 0;
    calc_height = $(window).height();
    var _offset = 25;
    origin_wrapper_height = $('body>div.container-fluid').height();
    origin_content_height = $('#divSearchResult .dataTables_scrollBody').height();

    $("#divSearchResult .dataTables_scrollBody").children().first().parentsUntil("body").each(function () {
        $(this).siblings().each(function () {
            if (calc_height > $(this).outerHeight(true) && $(this).css('display') != 'none') {
                //console.log(calc_height + " - " + $(this).outerHeight(true));
                if ($(this).attr("id") == 'loading')
                    return;
                calc_height = calc_height - $(this).outerHeight(true);
            }
        });
        _offset = _offset + $(this).outerHeight(true) - $(this).height();
    });

    //console.log("calc :" + calc_height + " offset: " + _offset);
    calc_height = calc_height - _offset;
    //console.log("total: " + calc_height);
    $('#divSearchResult .dataTables_scrollBody').css('max-height', calc_height + 'px');
    oTable.fnAdjustColumnSizing();
    //new $.fn.dataTable.FixedHeader(oTable);
    return calc_height;
}


$(document).ready(function () {
    loadData();
});



