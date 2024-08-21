var $BaseURL = '/';
var oTable = $('#akasForList').dataTable({
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,
    "searching": false,
    "bSort": false,
    "columns": [
        {
            "data": "PreferredFlag",
            "render": function (data, type, full, meta) {
                return '<input type="checkbox" id="" class="chkPreferredName" data-person-id="' + full.PersonID + '" data-person-name-id="' + full.PersonNameID + '" ' + (data === 1 ? 'checked="checked"' : '') + ' />';
            }
        },
        {
            "data": "PersonNameLast",
            "render": function (data, type, full, meta) {
                return '<a href="/Case/AKAsAddEdit/' + full.EncryptedPersonID + '?nameID=' + full.EncryptedPersonNameID + '&name=' + $('#btnAddakasrFor').attr('data-person') + '" >' + data + '</a>';
            }
        },
        {
            "data": "PersonNameFirst",
            "render": function (data, type, full, meta) {
                return '<a href="#" >' + data + '</a>';
            }
        },
        {
            "data":"EncryptedPersonNameID",
            "render": function (data, type, full, meta) {
                return ('<a class="btn btn-danger btn-xs delete" data-id="' + full.EncryptedPersonNameID + '"><i class="fa fa-trash-o"></i>Delete</a>');
            }
        }

    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    "deferRender": true
});

function loadData(id, person) {
    $.ajax({
        type: "POST", url: '/Case/PersonNameGetAKA/' + id + '',
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
        Notify('No AKAs available for ' + person, 'bottom-right', '5000', 'blue', 'fa-frown-o', true);
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
                type: "POST", url: '/Case/PersonNameGetAKADelete/' + id,
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

$('body').on('click', '.akasviewFor', function () {
    var person = $(this).attr('data-person');
    $('#akasFor').html('AKAs For <b>' + person + '</b>');
    $('#btnAddakasrFor').val('Add AKAs For ' + person);
    $("#btnAddakasrFor").attr("data-id", $(this).attr('data-id'));

    $("#btnAddakasrFor").attr("data-person", person);

    $(this).addClass('hidden');
    $('.akasviewFor').not(this).removeClass('hidden');
    loadData($(this).attr('data-id'), person);
});

$('#btnAddakasrFor').on('click', function () {
    var person = $(this).attr('data-person');
    window.location.href = '/Case/AKAsAddEdit/' + $(this).attr('data-id') + '?name=' + person;
});

$("#akasForList").on("click", ".chkPreferredName", function () {
    var selected = $(this).is(":checked");
    $(".chkPreferredName").each(function (i, e) {
        $(this).prop("checked", false);
    });

    $(this).prop("checked", selected);
});

$("#btnSave").on("click", function () {
    var personId = $(".chkPreferredName:first").data("person-id");
    var personNameId = -1;
    if ($(".chkPreferredName:checked").length > 0) {
        personId = $(".chkPreferredName:checked").data("person-id");
        personNameId = $(".chkPreferredName:checked").data("person-name-id");
    }

    $.ajax({
        type: "POST", url: '/Case/UpdatedPreferredName',
        data: { personId: personId, personNameId: personNameId },
        success: function (result) {
            if (result.isSuccess) {
                notifySuccess("Saved successfully.");
            }
        }
    });
});

$(document).ready(function () {
    if ($('#akasList tbody tr').length > 0) {
        var firstView = $('#akasList tbody tr').first().find('.akasviewFor').trigger('click');
    }
    else {
        $('#btnAddakasrFor').addClass('hidden');
    }
});