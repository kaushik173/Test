$BaseURL = '/';
var oTable = null;
var origin_wrapper_height = 0, origin_content_height = 0;

function loadData() {
    $.ajax({
        type: "POST", url: $BaseURL + 'Case/GetExpenseByCaseList',
        success: function (data) {
            if (data.URL != undefined) {
                window.location.href = '/' + data.URL;
            } else { setData(data); }
        }, dataType: 'json'
    });
}

function Validation() {
    IPadKeyboardFix();

    if (!IsValidFormRequest()) {
        return false;
    }
    var flag = true;
    var message;
    $('.required').each(function () {

        if ($(this).val() == '') {
            if ($(this).parent().find('.control-label').text() == '')
                message = $(this).parent().parent().find('.control-label').text();
            else
                message = $(this).parent().find('.control-label').text();
            notifyDanger(message + ' is required');
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

    if (!hasFormChanged('expenseAddEdit-form')) {
        if (buttonID == 2) {
            window.location.href = $BaseURL + 'Case/Expense';
            return false;
        } 
        notifyDanger('Nothing was changed.');
       
        return false;
    }
    
    if ($('#HourlyExpenseDate').val() != $('#hdn_HourlyExpenseDate').val()
        || $('#HourlyExpenseTypeCodeID').val() != $('#hdn_HourlyExpenseTypeCodeID').val()
        || $('#PersonID').val() != $('#hdn_PersonID').val()
        || $('#HourlyExpenseProviderCodeID').val() != $('#hdn_HourlyExpenseProviderCodeID').val()
        || $('#HourlyExpenseDescription').val() != $('#hdn_HourlyExpenseDescription').val()
        || $('#HourlyExpenseAmount').val() != $('#hdn_HourlyExpenseAmount').val()) {
        //chages done so need to save
        var data = $('#expenseAddEdit-form').serialize();
        $.ajax({
            type: "POST", url: $BaseURL + 'Case/ExpenseSave',
            data: data,
            success: function (data) {
                if (data.isSuccess) {
                    RequestSubmitted();

                    if (buttonID == 1) {
                        //Notify('Selected record has been saved successfully.', 'bottom-right', '5000', 'success', 'fa-check', true);
                        window.location.href = window.location.href;
                    }
                    else {
                        window.location.href = $BaseURL + 'Case/Expense';
                    }
                } else { }
            }, dataType: 'json'
        });
    }
    else {
        if (buttonID == 1) {
            window.location.href = window.location.href;
        }
        else {
            window.location.href = $BaseURL + 'Case/Expense';
        }
    }

}

$(window).bind('resize', function () {
    $('#expenseList').css('width', '100%');
    fitCalculatedHeightForSearchDataTable
});

$('body').on('click', '.delete', function () {
    var id = $(this).attr('data-id');
    var tr = $(this).parent().parent();

    confirmBox("Are you sure you want to remove selected records?", function (result) {
        if (result) {
            $.ajax({
                type: "POST", url: '/Case/ExpenseDelete/' + id,
                dataType: "json",
                success: function (data) {
                    if (data.isSuccess) {
                        //tr.remove();
                        //Notify('Selected record delete successfully.', 'bottom-right', '5000', 'success', 'fa-check', true);
                        window.location.href = $BaseURL + 'Case/Expense';
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
});

$(document).ready(function () {
    oTable = $('#expenseList').dataTable({
        "scrollY": "auto",
        "scrollCollapse": true,
        "paging": false,
        "searching": false,
        "bSort": false,
        "columns": [

                { "data": "HourlyExpenseDate" },
                {
                    "data": "HourlyExpenseType",
                    "render": function (data, type, full, meta) {
                        if ($IsEdit) {
                            return '<a href="/Case/Expense/' + full.EncryptedHourlyExpenseID + '">' + data + '</a>';
                        }
                        else {
                            return data;
                        }
                    }
                },
                { "data": "Attoreny" },
                { "data": "HourlyExpenseAmount" },
                { "data": "HourlyExpenseProvider" },
                { "data": "HourlyExpenseDescription" },
                {
                    "render": function (data, type, full, meta) {
                        if ($IsDelete) {
                            return '<a class="btn btn-danger btn-xs delete"  href="javascript:void(0);" data-id="' + full.EncryptedHourlyExpenseID + '"><i class="fa fa-trash-o"></i> Delete</a>';
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
            var Id = $("#HourlyExpenseID").val();
            if (Id == aData.HourlyExpenseID) {
                $(nRow).find("td").addClass("selectedrow");
            }
        }
    });
    // Load the expenseList
    loadData();
    setInitialFormValues('expenseAddEdit-form', true);

});