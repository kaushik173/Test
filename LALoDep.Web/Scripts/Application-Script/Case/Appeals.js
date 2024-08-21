var page = (function () {
    return {
        $table: $('#AppealsView'),
        ttl: 600000 /*In Milli seconds*/
    };
})();

$(document).ready(function () {
    loadData();
});

var oTable = $('#AppealsView').dataTable({
    "searching": false,
    "bSort": false,
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,

    "columns": [
        { "data": "PetitionFileDate" },
        { "data": "PetitionCloseDate" },
        { "data": "PetitionDocketNumber" },
        { "data": "PetitionTypeCodeValue" },
        { "data": "Child" },
        {
            "render": function (data, type, full, meta) {
                var pageTitle = full.Child + ', ' + full.PetitionFileDate + ', ' + full.PetitionTypeCodeValue + ', ' + full.PetitionDocketNumber;
                if (full.AppealCount > 0) {
                    return ('<a class="btn btn-info btn-xs appeals" data-case="' + full.Child + ' ' + full.PetitionDocketNumber + '" petitionId="' + full.PetitionID + '" data-page-title="' + pageTitle + '"><i class="fa fa-eye"></i> View Appeals</a>');
                } else {
                    return ('<a class="btn btn-info btn-xs appealsadd" data-docket="' + full.PetitionDocketNumber + '" data-case="' + full.Child + ' ' + full.PetitionDocketNumber + '" data-id="' + full.PetitionID + '" data-page-title="' + pageTitle + '"><i class="fa fa-eye"></i> Add Appeal</a>');
                }
            }
        }
    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    "deferRender": true
});

var oTableAppeals = $('#AppealsListView').dataTable({
    "searching": false,
    "bSort": false,
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,

    "columns": [
         {
             "data": "AppealFileDate",
             "render": function (data, type, full, meta) {

                 return ('<a href="/WritsAppeals/AddEditAppeals?appealID=' + full.EncryptedAppealID + '&pageTitle=' + $("#add").data('page-title') + '">' + data + '</a>');
             }
         },

        { "data": "AppealTypeCodeValue" },
        { "data": "AppealOralArgumentDate" },
        {
            "data": "DecisionCodeValue",
            "render": function (data, type, full, meta) {
                return ('<a href="/WritsAppeals/AppealDecisionAddEdit/' + full.EncryptedDecisionID + '?appealID=' + full.EncryptedAppealID + '">' + data + '</a>');
            }
        },
        { "data": "AppealDecisionDate" },
        {
            "render": function (data, type, full, meta) {
                return ('<a class="btn btn-danger btn-xs delete" data-id="' + full.EncryptedAppealID + '"><i class="fa fa-trash-o"></i>Delete</a>')
            },
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

$('#AppealsView').on('draw.dt', function () {
    if (device.mobile() || device.tablet())//if mobile or tablet
    {
        $('#AppealsView_info').parent().addClass('col-xs-4').removeClass('col-xs-6');
        $('#AppealsView_paginate').parent().addClass('col-xs-8').removeClass('col-xs-6');
    }

});

$(window).bind('resize', function () {
    $('#AppealsView').css('width', '100%');
});

$('#divSearchResult').on('click', '.appeals', function () {
    $('#add').show();
    $('#divSearchResult a.appeals').removeClass('hide');
    $(this).addClass('hide');
    var caseNumber = $(this).attr('data-case');
    $('#add').val('Add Appeal For ' + caseNumber);
    $('#appealFor').text('Appeal For ' + caseNumber);
    $("#add").attr("data-id", $(this).attr('petitionId'));
    $("#add").attr("data-docket", caseNumber);
    $("#add").data('page-title', $(this).data('page-title'));

    var id = $(this).attr('petitionId');
    $.ajax({
        type: "GET", url: '/WritsAppeals/GetAppeals?petitionID=' + id,
        success: function (data) {
            setDataAppealsList(data, caseNumber);
        },
        dataType: 'json'
    });
});
$('#divSearchResult').on('click', '.appealsadd', function () {
    var id = $(this).attr('data-id');
    var docketNo = $(this).attr('data-docket');
    var pageTitle = $(this).data('page-title');
    window.location.href = "/WritsAppeals/AddEditAppeals/" + id + "?docketNo=" + docketNo + "&pageTitle=" + pageTitle;
});
$('#add').on('click', function () {
    var id = $(this).attr('data-id');
    var docketNo = $(this).attr('data-docket');
    var pageTitle = $(this).data('page-title');
    window.location.href = "/WritsAppeals/AddEditAppeals/" + id + "?docketNo=" + docketNo + "&pageTitle=" + pageTitle;
});

$('body').on('click', '.delete', function () {
    var id = $(this).attr('data-id');
    var tr = $(this).parent().parent();
    confirmBox("Are you sure you want to remove selected records?", function (result) {
        if (result) {
            $.ajax({
                type: "POST", url: '/WritsAppeals/ApplealDelete/' + id,
                dataType: "json",
                success: function (data) {
                    tr.remove();
                    Notify('Record delete successfully.', 'bottom-right', '5000', 'success', 'fa-check', true);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                }
            });
        }
    });
});

function loadData() {
    $.ajax({
        type: "GET", url: '/WritsAppeals/GetPetitions',
        success: function (data) {
            setData(data);
        },
        dataType: 'json'
    });
}

function setDataAppealsList(data, caseNumber) {
    oTableAppeals.fnClearTable();
    if (data.data.length > 0) {
        oTableAppeals.fnAddData(data.data);
        fitCalculatedHeightForSearchDataTable();
    } else
        Notify('No Appeal available for ' + caseNumber, 'bottom-right', '5000', 'blue', 'fa-frown-o', true);
}

function setData(data) {
    oTable.fnClearTable();
    if (data.data.length > 0) {
        oTable.fnAddData(data.data);
        fitCalculatedHeightForSearchDataTable();
        if ($('#AppealsView tbody tr').length > 0) {

            $('#AppealsView tbody tr').first().find('.appeals').trigger('click');
        }
    } else
        Notify('No results found.', 'bottom-right', '5000', 'blue', 'fa-frown-o', true);
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
    $('#add').hide();
});