var page = (function () {
    return {
        $table: $('#MotionsView'),
        ttl: 600000 /*In Milli seconds*/
    };
})();


var oTable = $('#MotionsView').dataTable({
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
                 if (full.MotionCount > 0) {
                     return ('<a class="btn btn-info btn-xs motions" data-petitiondocketnumber="' + full.Child + ' ' + full.PetitionDocketNumber + '"  petitionId="' + full.PetitionID + '"><i class="fa fa-eye"></i> View Motions</a>');

                 } else {
                     return ('<a class="btn btn-info btn-xs motionsadd" data-petitiondocketnumber="' + full.Child + ' ' + full.PetitionDocketNumber + '"  petitionId="' + full.PetitionID + '"><i class="fa fa-eye"></i> Add Motion</a>');

                 }
             }
         }
    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    "deferRender": true
});

var oTableMotions = $('#MotionsListView').dataTable({
    "searching": false,
    "bSort": false,
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,

    "columns": [

          {
              "data": "MotionFileDate",
              "render": function (data, type, full, meta) {
                  return ('<a href="/Motions/MotionAddEdit/' + full.EncryptedMotionID + '?petitionID=' + full.EncryptedPetitionId + '">' + data + '</a>');
              }
          },
          { "data": "MotionTypeCodeValue" },
          { "data": "MotionRequestByCodeValue" },
          { "data": "MotionDecisionCodeValue" },
          { "data": "MotionDecisionDate" },
          {
              "render": function (data, type, full, meta) {
                  if ($canDeleteAccess)
                      return ('<a class="btn btn-danger btn-xs delete" data-id="' + full.EncryptedMotionID + '"><i class="fa fa-trash-o"></i>Delete</a>')
                  else
                      return ''
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

function loadData() {
    $.ajax({
        type: "POST", url: '/Motions/GetPetitions',
        success: function (data) {
            setData(data);
        },
        dataType: 'json'
    });
}

function setDataMotionsList(data,caseNumber) {
    oTableMotions.fnClearTable();
    if (data.data.length > 0) {
        oTableMotions.fnAddData(data.data);
        fitCalculatedHeightForSearchDataTable();
    } else
        Notify('No Motion available for ' + caseNumber, 'bottom-right', '5000', 'blue', 'fa-frown-o', true);
}

function setData(data) {
    oTable.fnClearTable();
    if (data.data.length > 0) {
        oTable.fnAddData(data.data);
        fitCalculatedHeightForSearchDataTable();
        if ($('#MotionsView tbody tr').length > 0) {
           
            $('#MotionsView tbody tr').first().find('.motions').trigger('click');
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

$('#MotionsView').on('draw.dt', function () {
    if (device.mobile() || device.tablet())//if mobile or tablet
    {
        $('#MotionsView_info').parent().addClass('col-xs-4').removeClass('col-xs-6');
        $('#MotionsView_paginate').parent().addClass('col-xs-8').removeClass('col-xs-6');
    }

});

$(window).bind('resize', function () {
    $('#MotionsView').css('width', '100%');
});

$('#divSearchResult').on('click', '.motions', function () {
    $('#divSearchResult a.motions').removeClass('hide');
    $(this).addClass('hide');
    var caseNumber = $(this).data('petitiondocketnumber');
    $('#motionFor').text("Motion For " + caseNumber);
    $('#btnAdd').show();
    $('#btnAdd').text("Add Motion For " + caseNumber);
    
    var id = $(this).attr('petitionId');
    $('#btnAdd').attr("petitionId", id);
    $.ajax({
        type: "POST", url: '/Motions/GetMotions?petitionID=' + id,
        success: function (data) {
            setDataMotionsList(data, caseNumber);
        },
        dataType: 'json'
    });
});
$('#divSearchResult').on('click', '.motionsadd', function () {
    var id = $(this).attr('petitionId');
    window.location.href = '/Motions/MotionAddEdit?petitionID=' + id;
});
$('#btnAdd').on('click', function () {
    var id = $(this).attr('petitionId');
    window.location.href = '/Motions/MotionAddEdit?petitionID=' + id;
});

$('body').on('click', '.delete', function () {
    var id = $(this).attr('data-id');
    var tr = $(this).parent().parent();
    confirmBox("Are you sure you want to remove selected records?", function (result) {
        if (result) {
            $.ajax({
                type: "POST", url: '/Motions/MotionDelete/' + id,
                dataType: "json",
                success: function (data) {
                    tr.remove();
                    Notify('Selected record delete successfully.', 'bottom-right', '5000', 'success', 'fa-check', true);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                }
            });
        }
    });
});

$(document).ready(function () {
    loadData();
});
