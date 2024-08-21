$BaseURL = '/';
var oTable = null;
var origin_wrapper_height = 0, origin_content_height = 0;

function loadData() {
    $.ajax({
        type: "POST", url: $BaseURL + 'Case/ClientContactList',
        success: function (data) {
            if (data.URL != undefined) {
                window.location.href = '/' + data.URL;
            } else { setData(data); }
        }, dataType: 'json'
    });
}

function Validation() {

    var flag = true;
    var message;
    $('.required').each(function () {

        if ($(this).val() == '') {
            if ($(this).parent().find('.control-label').text() == '')
                message = $(this).parent().parent().find('.control-label').text();
            else
                message = $(this).parent().find('.control-label').text();
            Notify(message + ' is required', 'bottom-right', '4000', 'danger', 'fa-warning', true);
            $(this).focus();
            flag = false;
            return false;
        }

    });
    return flag;
}

function setData(data) {
    oTable.fnClearTable();
    if (data.data != undefined && data.data.length > 0) {
        oTable.fnAddData(data.data);
        fitCalculatedHeightForSearchDataTable();
        origin_wrapper_height = $('body>div.container-fluid').height();
        origin_content_height = $('#divSearchResult .dataTables_scrollBody').height();
    }
}

function fitCalculatedHeightForSearchDataTable() {
    var calc_height = 0;
    if (oTable != null) {
        calc_height = $(window).height();
        var _offset = 40;
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
    }
    return calc_height;
}

function SaveData(buttonID) {

    IPadKeyboardFix();

    if (!IsValidFormRequest()) {
        return;
    }

    if ($('#ConflictDate').val() != $('#hdn_ConflictDate').val()
        || $('#RoleID').val() != $('#hdn_RoleID').val()
        || $('#ConflictTypeCodeID').val() != $('#hdn_ConflictTypeCodeID').val()
        || $('#NoteEntry').GetHtml() != $('#hdn_NoteEntry').val()) {

        var updateConflictRecord = ($('#ConflictDate').val() != $('#hdn_ConflictDate').val() || $('#RoleID').val() != $('#hdn_RoleID').val() || $('#ConflictTypeCodeID').val() != $('#hdn_ConflictTypeCodeID').val()) ? "true" : "";
        var updateConflictNote = ($('#NoteEntry').GetHtml() != $('#hdn_NoteEntry').val())?"true":"";

        //var data = $('#confiltAddEdit-form').serialize();
        //if ($('#NoteEntry.summernote').length > 0) {
        //    data = data.replace('NoteEntry=', 'NoteEntryold=') + '&NoteEntry=' + $('#NoteEntry').GetHtmlWithEscape();
        //}
     //   data = data + "&UpdateConflictRecord=" + updateConflictRecord + '&UpdateConflictNote=' + updateConflictNote;
        var data = {};
        var unindexed_array = $('#confiltAddEdit-form').serializeArray();
        $.map(unindexed_array, function (n, i) {
            
            data[n['name']] = $('#' + n['name']).GetHtml();
        });
       
        data["UpdateConflictRecord"] = updateConflictRecord
        data["UpdateConflictNote"] = updateConflictNote



        if ($('#ConflictID').val() > 0 && $('#NoteEntry').GetText() == '' && $('#NoteID').val() > 0) {
        confirmBox("Are you sure you want to remove the note from this record?", function (result) {
            if (result) {
                                $.ajax({
                        type: "POST", url: $BaseURL + 'Case/ConflictSave',
                        data: data,
                        dataType: 'json',
                        success: function (data) {
                            if (data.isSuccess) {
                                RequestSubmitted();
                                if (buttonID == 1) {
                                   window.location.href = window.location.href;
                                }
                                else {
                                 window.location.href = $BaseURL + 'Case/Conflict';
                                }
                            } else { }
                        }, dataType: 'json'
                    });
            }
        });
        } else {
            $.ajax({
                type: "POST", url: $BaseURL + 'Case/ConflictSave',
                data: data,
                dataType: 'json',
                success: function (data) {
                    if (data.isSuccess) {
                        RequestSubmitted();
                        if (buttonID == 1) {
                            window.location.href = window.location.href;
                        }
                        else {
                            window.location.href = $BaseURL + 'Case/Conflict';
                        }
                    } else { }
                }, dataType: 'json'
            });
        }

   
    }
    else {
        if (buttonID == 1) {
            window.location.href = window.location.href;
        }
        else {
            window.location.href = $BaseURL + 'Case/Conflict';
        }
    }

}

$(window).bind('resize', function () {
    $('#conflictList').css('width', '100%');
    fitCalculatedHeightForSearchDataTable
});

$('body').on('click', '.delete', function () {
    var id = $(this).attr('data-id');
    var tr = $(this).parent().parent();

    confirmBox("Are you sure you want to delete?", function (result) {
        if (result) {
            $.ajax({
                type: "POST", url: '/Case/ConflictDelete/' + id,
                dataType: "json",
                success: function (data) {
                    if (data.isSuccess) {
                        //tr.remove();
                        //Notify('Selected record delete successfully.', 'bottom-right', '5000', 'success', 'fa-check', true);
                        window.location.href = $BaseURL + 'Case/Conflict';
                    }

                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                }
            });
        }
        else {
        }
    });
});

$('#btnSave').on('click', function () {
    if (Validation())
        SaveData(1);
    else
        return false;

});

$('#btnSaveAndAddNew').on('click', function () {
    if (Validation())
        SaveData(2);
    else
        return false;
}); $('#btnBack').on('click', function () {
    window.location.href = '/Case/Main';
});

$(document).ready(function () {
    oTable = $('#conflictList').dataTable({
        "scrollY": "auto",
        "scrollCollapse": true,
        "paging": false,
        "searching": false,
        "bSort": false,
        "columns": [
                {
                    "data": "ConflictDate",
                    "render": function (data, type, full, meta) {
                        if ($IsEdit == 'True') {
                            return '<a href="/Case/Conflict/' + full.EncrypredConflictID + '">' + data + '</a>';
                        }
                        else {
                            return data;
                        }
                    }
                },
                { "data": "CaseRoleDisplay" },
                { "data": "ConflictType" },
                {
                    "render": function (data, type, full, meta) {
                        if ($IsDelete=='True') {
                            return '<a class="btn btn-danger btn-xs delete"  href="javascript:void(0);" data-id="' + full.EncrypredConflictID + '"><i class="fa fa-trash-o"></i> Delete</a>';
                        }
                        else {
                            return '';
                        }

                    }
                }
        ],
        "loadingRecords": "Loading...",
        "processing": "Processing...",
        "deferRender": true,
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            var contactId = $("#ConflictID").val();
            if (contactId == aData.ConflictID) {
                $(nRow).find("td").addClass("selectedrow");
            }
        }
    });
    // Load the conflict list
    loadData();

});