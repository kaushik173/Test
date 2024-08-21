var $BaseURL = '/';
var personName = '';
var oTable = $('#PeopleLegalNumbersList').dataTable({
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,
    "searching": false,
    "bSort": false,
    "columns": [
 {
     "data": "Type",
     "render": function (data, type, full, meta) {
         return '<a href="/Case/LegalNumberEdit/' + full.EncryptedID + '?name=' + $("#btnAddLegaNumberFor").attr('data-person') +'" >' + data + '</a>';
     }
 },
 {
     "data": "Number" 
 },{
     "data": "LegalNumberComment" 
 },
 {
     "render": function (data, type, full, meta) {
         return ('<a class="btn btn-danger btn-xs delete" data-id="' + full.EncryptedID + '"><i class="fa fa-trash-o"></i>Delete</a>')
     },
 },

    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    "deferRender": true
});

function loadData(id, person) {    
    $.ajax({
        type: "POST", url: '/Case/LegalNumberForPerson/' + id + '',
        success: function (data) {
            setData(data, person);
        },
        dataType: 'json'
    });
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

function setData(data, person) {
    
    oTable.fnClearTable();
    if (data.data.length > 0) {
        oTable.fnAddData(data.data);
        fitCalculatedHeightForSearchDataTable();
    }
    else {
        Notify('No LegalNumber available for ' + person, 'bottom-right', '5000', 'blue', 'fa-frown-o', true);
    }

}

$(window).bind('resize', function () {
    fitCalculatedHeightForSearchDataTable();
});

$('body').on('click', '.delete', function () {
    var id = $(this).attr('data-id');
    var tr = $(this).parent().parent();
    confirmBox("Are you sure you want to remove selected records?", function (result) {
        if (result) {
            $.ajax({
                type: "POST", url: '/Case/LegalNumberDeleteForPerson/' + id,
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

$('body').on('click', '.PersonLegalNumView', function () {
    var person = $(this).attr('data-person');
    $('#legalNumberFor').html('Legal Numbers For <b>' + person+'</b>');
    
    $('#btnAddLegaNumberFor').val('Add Legal Number For ' + person);
    $("#btnAddLegaNumberFor").attr("data-id", $(this).attr('data-id'));
    $("#btnAddLegaNumberFor").attr("data-person", person);

    $(this).addClass('hidden');
    $('.PersonLegalNumView').not(this).removeClass('hidden');
    loadData($(this).attr('data-id'), person);
});

$('#btnAddLegaNumberFor').on('click', function () {
    var person = $(this).attr('data-person');
    window.location.href = '/Case/LegalNumberAdd/' + $(this).attr('data-id') + '?name=' + person;
});

$(document).ready(function () {
    if ($('#legalNumbersList tbody tr').length > 0) {
        var firstView = $('#legalNumbersList tbody tr').first().find('.PersonLegalNumView').trigger('click');
    }

});