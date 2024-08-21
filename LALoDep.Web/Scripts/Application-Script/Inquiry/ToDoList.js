var page = (function () {
    return {
        $form: $('#todolist-form'),
        $table: $('#searchToDoList'),
        mainKey: 'todolist-form' + GetWindowID(),
        resultPageIdKey: GetWindowID() + 'todolist-form-page#',
        rowSelectedKey: GetWindowID() + 'todolist-form-row#',
        ttl: 600000 /*In Milli seconds*/
    };
})();


$PDActionIDList = ',';
$StatusChangeIDList = ','
$ActionStatus = 0;
var oTable = $('#searchToDoList').dataTable({
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,
    "searching": false,
    "bSort": false,
    "columns": [
 {
     "data": "CaseFileDisplay",
     "render": function (data, type, full, meta) {
         return '<a  class="auto-download" href="/Inquiry/DownloadToDoFile?name=' + data + '&path=' + full.CaseFilePath + '" >' + data + '</a>';
     }
 },{
     "data": "ActionType",
     "render": function (data, type, full, meta) {
         return '<a class="go-to-edit" href="/Inquiry/ToDoAddEdit/' + full.PDActionEcryptedID + '" >' + data + '</a>';
     }
 },
 { "data": "Action" }, {
     "data": "Jcats",
     "render": function (data, type, full, meta) {
         return '<a href="#"  onclick="Process(\'' + full.EncryptedCaseID + '\')">' + data + '</a>';
     }
 },

 { "data": "CaseName" },
 { "data": "ReminderDate" },
 { "data": "DueDate" },

 {
     "render": function (data, type, full, meta) {
         if (full.ActionStatusCodeID == 3382) {
             return ('<input type="checkbox" value="' + full.PDActionEcryptedID + '" id="checkboxStatus' + full.PDActionEcryptedID + '" class="checkBoxStatusClass form-control input-sm" onclick="onChkChangeStatusChecked(' + "'" + full.PDActionEcryptedID + "'," + full.ActionStatusCodeID + ');"/>')
         }
         else if (full.ActionStatusCodeID == 3383) {
             return ('<input type="checkbox" checked="checked"  value="' + full.PDActionEcryptedID + '" id="checkboxStatus' + full.PDActionID + '" class="checkBoxStatusClass form-control input-sm" onclick="onChkChangeStatusChecked(' + "'" + full.PDActionEcryptedID + "'," + full.ActionStatusCodeID + ');" />')
         }
         else {
             return '';
         }
     }
 },
 {
     "render": function (data, type, full, meta) {

         return ('<input type="checkbox" value="' + full.PDActionEcryptedID + '" id="checkbox' + full.PDActionEcryptedID + '" class="checkBoxClass form-control input-sm" onclick="onChkChecked(' + "'" + full.PDActionEcryptedID + "'" + ');" />')


     },

 },

    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    "deferRender": true,
    "fnDrawCallback": function (oSettings) {
        $("a.go-to-edit").on('click', onGoToEditClick);
    },
});
function Process(caseId) {
    if ($PDActionIDList != ",") {
        var data = getData()
        data = data + "&DeleteIDList=" + $PDActionIDList;
        data = data + "&StatusChangeIDList=" + $StatusChangeIDList;
        data = data + "&ActionStatus=" + $ActionStatus;
        confirmBox("Are you sure you want to remove selected records?", function (result) {
            if (result) {
                $.ajax({
                    type: "POST", url: '/Inquiry/ToDoListSearch', data: data,
                    dataType: "json",
                    success: function (data) {
                        setData(data);
                        notifySuccess('Data saved successfully.<br/>Redirecting to Main page');
                        $PDActionIDList = ",";
                        $StatusChangeIDList = ",";
                        $ActionStatus = 0;

                        document.location.href = '/Case/Main/' + caseId;
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                    }
                });
            }
            else {
                $('.checkBoxClass').prop('checked', false);
                $('#checkboxAll').prop("checked", false);
                $PDActionIDList = ",";
            }
        });


    }
    else if ($StatusChangeIDList != ",") {

        var data = getData()
        data = data + "&DeleteIDList=" + $PDActionIDList;
        data = data + "&StatusChangeIDList=" + $StatusChangeIDList;
        data = data + "&ActionStatus=" + $ActionStatus;
        $.ajax({
            type: "POST", url: '/Inquiry/ToDoListSearch', data: data,
            success: function (data) {
                if ($ActionStatus == 3383) {
                    $('#checkallStatus').prop("checked", "checked");
                }
                setData(data);
                notifySuccess('Data saved successfully.<br/>Redirecting to Main page');
                $PDActionIDList = ",";
                $StatusChangeIDList = ",";

                document.location.href = '/Case/Main/' + caseId;
            },
            dataType: 'json'
        });
    } else {

        document.location.href = '/Case/Main/' + caseId;
    }
}
function checkPrinValidations() {
    if (($('#StartDate').val().length != 0 || $('#EndDate').val().length != 0) && ($('#DateTypeID').val().length == 0)) {
        notifyDanger('Date Type is required.');
        $('#DateTypeID').focus();
        return false;
    }

    if (($('#StartDate').val().length == 0)) {
        notifyDanger('Start Date is required', 'bottom-right');
        $('#StartDate').focus();
        return false;
    }

    if (($('#StartDate').val().length != 0) && ($('#EndDate').val().length == 0)) {
        notifyDanger('End Date is required', 'bottom-right');
        $('#EndDate').focus();
        return false;
    }
    if (($('#StartDate').val().length != 0) && ($('#EndDate').val().length != 0)) {
        var result = compareDates($('#StartDate').val(), $('#EndDate').val())
        if (result) {
            notifyDanger('End Date cannot be earlier than start date');
            $('#EndDate').focus();
            return false;
        }
    }
    if ($('#ActionStatusCodeID').val().length == 0) {
        notifyDanger('Status is required.');
        $('#ActionStatusCodeID').focus();
        return false;
    }


    return true;
}

function loadData() {
    var formInvalid = $('#ActionTypeCodeID').val().length == 0
                && $('#ActionStatusCodeID').val().length == 0
                && $('#StartDate').val().length == 0
                && $('#EndDate').val().length == 0
                && $('#DateType').val() == '';

    if (formInvalid) {
        notifyDanger('At least one search parameter is required.');
        return false;
    }
    var validationForSearch = searchValidation();
    if (validationForSearch) {
        if ($PDActionIDList != ",") {
            var data = getData()
            data = data + "&DeleteIDList=" + $PDActionIDList;
            data = data + "&StatusChangeIDList=" + $StatusChangeIDList;
            data = data + "&ActionStatus=" + $ActionStatus;
            confirmBox("Are you sure you want to remove selected records?", function (result) {
                if (result) {
                    $.ajax({
                        type: "POST", url: '/Inquiry/ToDoListSearch', data: data,
                        dataType: "json",
                        success: function (data) {
                            setData(data);
                            notifySuccess('Data saved successfully.');
                            $PDActionIDList = ",";
                            $StatusChangeIDList = ",";
                            $ActionStatus = 0;
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                        }
                    });
                }
                else {
                    $('.checkBoxClass').prop('checked', false);
                    $('#checkboxAll').prop("checked", false);
                    $PDActionIDList = ",";
                }
            });


        }
        else if ($StatusChangeIDList != ",") {

            var data = getData()
            data = data + "&DeleteIDList=" + $PDActionIDList;
            data = data + "&StatusChangeIDList=" + $StatusChangeIDList;
            data = data + "&ActionStatus=" + $ActionStatus;
            $.ajax({
                type: "POST", url: '/Inquiry/ToDoListSearch', data: data,
                success: function (data) {
                    if ($ActionStatus == 3383) {
                        $('#checkallStatus').prop("checked", "checked");
                    }
                    setData(data);
                    notifySuccess('Data saved successfully.');
                    $PDActionIDList = ",";
                    $StatusChangeIDList = ",";
                },
                dataType: 'json'
            });
        }
        else {
            var data = getData()
            $.ajax({
                type: "POST", url: '/Inquiry/ToDoListSearch', data: data,
                success: function (data) {
                    setData(data);
                },
                dataType: 'json'
            });
        }
    }
}

function searchValidation() {
    if ($('#StartDate').val().length == 0) {
        notifyDanger('Start Date is required.')
        $('#StartDate').focus();
        return false;
    }

    if ($('#EndDate').val().length == 0) {
        notifyDanger('End Date is required.');
        $('#EndDate').focus();
        return false;
    }

    if (($('#StartDate').val().length != 0 || $('#EndDate').val().length != 0) && ($('#DateType').val().length == 0)) {
        notifyDanger('Date Type is required.');
        $('#DateType').focus();
        return false;
    }
    return true;
}

function getData() {
    var fData = $('#todolist-form').serialize();
    return fData;
}
function setData(data) {
    SavePageState(data);

    oTable.fnClearTable();
    if (data.data.length > 0) {
        $('#btnPrint').removeClass("hidden");
        if (data.data[0].ActionStatusCodeID == 3383) {
            $('#checkallStatus').prop("checked", false);
            $ActionStatus = data.data[0].ActionStatusCodeID;
        }
        else {
            $('#checkallStatus').prop("checked", false);
            $ActionStatus = data.data[0].ActionStatusCodeID;
        }
        oTable.fnAddData(data.data);
        fitCalculatedHeightForSearchDataTable();
    } else {
        $('#btnPrint').addClass("hidden");
        $('#checkallStatus').prop("checked", false);
        notifyInfo('No results found.');
    }

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
function onChkChecked(PDActionID) {
    if ($('#checkbox' + PDActionID).prop("checked") == true) {
        $PDActionIDList = $PDActionIDList + PDActionID + ",";
        $counterChecked = 0;
        $(".checkBoxClass").each(function () {
            if ($(this).prop("checked") == true) {
                $counterChecked = parseInt($counterChecked) + 1;
            }
        });
        if ($counterChecked == $(".checkBoxClass").length) {
            $('#checkboxAll').prop("checked", true);
        }
    }
    else {
        $uncheck = "," + PDActionID + ",";

        $PDActionIDList = $PDActionIDList.replace($uncheck, ",");

        $('#checkboxAll').prop("checked", false);
    }

}

function onChkChangeStatusChecked(PDActionID, StatusID) {


    if (StatusID == 3382) {
        $ActionStatus = 3383;
        if ($('#checkboxStatus' + PDActionID).prop("checked") == true) {

            $StatusChangeIDList = $StatusChangeIDList + PDActionID + ",";
            $counterCheckedStatus = 0;
            $(".checkBoxStatusClass").each(function () {
                if ($(this).prop("checked") == true) {
                    $counterCheckedStatus = parseInt($counterCheckedStatus) + 1;
                }
            });
            if ($counterCheckedStatus == $(".checkBoxStatusClass").length) {
                $('#checkallStatus').prop("checked", true);
            }
        }
        else {
            $ActionStatus = 3383;

            $uncheckStatus = "," + PDActionID + ",";
             if ($StatusChangeIDList == ',') {
                $StatusChangeIDList = $StatusChangeIDList + PDActionID + ",";

            } else {
                $StatusChangeIDList = $StatusChangeIDList.replace($uncheckStatus, ",");

            }
            $('#checkallStatus').prop("checked", false);
        }
    }
    else if (StatusID == 3383) {
        $ActionStatus = 3382;
        $counterCheckedStatus = 0;

        if ($('#checkboxStatus' + PDActionID).prop("checked") == false) {
            $StatusChangeIDList = $StatusChangeIDList + PDActionID + ",";
            $counterCheckedStatus = 0;
            $('#checkallStatus').prop("checked", false);
        }
        else {
            $ActionStatus = 3382;


            $uncheckStatus = "," + PDActionID + ",";
            if ($StatusChangeIDList == ',') {
                $StatusChangeIDList = $StatusChangeIDList + PDActionID + ",";

            } else {
                $StatusChangeIDList = $StatusChangeIDList.replace($uncheckStatus, ",");

            }

            $(".checkBoxStatusClass").each(function () {
                if ($(this).prop("checked") == true) {
                    $counterCheckedStatus = parseInt($counterCheckedStatus) + 1;
                }
            });
            if ($counterCheckedStatus == $(".checkBoxStatusClass").length) {
                $('#checkallStatus').prop("checked", true);
            }
        }
    }

}
function compareDates(start, end) {
    var startDate = new Date(start);
    var endDate = new Date(end);
    if (endDate < startDate) {
        return true;
    }
    else {
        return false;
    }
}

function CheckAll() {
    if ($ActionStatus == 3382 && $StatusChangeIDList == ",") {
        if ($('#checkallStatus').prop("checked") == true) {
            $(".checkBoxStatusClass").prop("checked", true);

            $StatusChangeIDList = ',';
            $(".checkBoxStatusClass").each(function () {
                var value = ($(this).val())
                $StatusChangeIDList = $StatusChangeIDList + value + ",";
            });
            $ActionStatus = 3383
        }
    }
    else if ($ActionStatus == 3383 && $StatusChangeIDList != ",") {
        $('.checkBoxStatusClass').prop("checked", false);
        $StatusChangeIDList = ',';
        $ActionStatus = 3382
    }
    else if ($ActionStatus == 3383 && $StatusChangeIDList == ",") {

        $('.checkBoxStatusClass').prop("checked", false);
        $StatusChangeIDList = ',';
        $ActionStatus = 3382
        $StatusChangeIDList = ',';
        $(".checkBoxStatusClass").each(function () {
            var value = ($(this).val())
            $StatusChangeIDList = $StatusChangeIDList + value + ",";
        });
        $ActionStatus = 3382

    }
    else if ($ActionStatus == 3382 && $StatusChangeIDList != ",") {
        if ($('#checkallStatus').prop("checked") == true) {
            $(".checkBoxStatusClass").prop("checked", true);
            $StatusChangeIDList = ',';
            $ActionStatus = 3383;
        }
    }

}

function getDate(d) {
    d = (d || new Date());

    var month = d.getMonth() + 1;
    var day = d.getDate();
    return (month < 10 ? '0' : '') + month + '/' +
           (day < 10 ? '0' : '') + day + '/' + d.getFullYear();
}

function onGoToEditClick(e) {
    var $this = $(this);
    e.preventDefault();

    enableLoadDataFromCache();

    simpleStorage.set(page.rowSelectedKey, $this.closest('tr').index(), { TTL: page.ttl });
    document.location = $this.attr('href');
}

function ResetPageState() {
    simpleStorage.deleteKey(page.mainKey);
    simpleStorage.deleteKey(page.resultPageIdKey);
    simpleStorage.deleteKey(page.rowSelectedKey);
};

function SavePageState(results) {
    var formData = {};
    page.$form.serializeArray().map(function (item) {
        formData[item.name] = item.value;
    });
    var pageState = { formData: formData, results: results };

    simpleStorage.set(page.mainKey, pageState, { TTL: page.ttl });

    enableLoadDataFromCache();
};


page.$table.on('page.dt', function () {
    var table = page.$table.dataTable();
    var pageInfo = table.fnPagingInfo();
    var pageNumber = pageInfo.iPage;
    simpleStorage.set(page.resultPageIdKey, pageNumber, { TTL: page.ttl });
});

function LoadPreviousPageState() {
    var pageState = simpleStorage.get(page.mainKey);
    if (pageState && pageState.formData && pageState.results) {

        /*Load Results*/
        setData(pageState.results);

        /*Load Page #*/
        var pageNumber = simpleStorage.get(page.resultPageIdKey);
        if (pageNumber) page.$table.dataTable().fnPageChange(pageNumber);

        /*Load Form inputs*/
        var formData = pageState.formData;
        $('*', page.$form).filter(':input').each(function () {
            var $this = $(this);
            if (formData[$this.attr('id')])
                $this.val(formData[$this.attr('id')]);
        });

        /*Highlight selected Row*/
        var rowIndex = simpleStorage.get(page.rowSelectedKey);
        if (rowIndex >= 0) {
            page.$table.find("tr:nth-child(" + (rowIndex + 1) + ")").css("background", "#f3f7b4");
            page.$table.find("tr:nth-child(" + (rowIndex + 1) + ") td").css("background", "#f3f7b4");
        }

    }
};


$(window).bind('resize', function () {
    $('#searchToDoList').css('width', '100%');
    fitCalculatedHeightForSearchDataTable();
});

$(document).ready(function () {
    
    enableLoadDataFromCache();
    if ($loadFromCache && isLoadDataFromCache() && simpleStorage.get(page.mainKey)) {
        LoadPreviousPageState();
    }
    else {
        ResetPageState();
        loadData();
    }

    $(window).on("keydown", handleHotkey);

    function handleHotkey(e) {
        if (!e.ctrlKey) return;
        switch (String.fromCharCode(e.keyCode).toLowerCase()) {
            case 'i':
                $('#btnPrint').trigger('click');
                e.preventDefault();
                break;
            default:
                break;
        }
    }


    $("#addNew").on("click", function () {
        window.location.href = "/Inquiry/ToDoAddEdit";
    });

    $('#btnSearch').on('click', function () {
        IPadKeyboardFix();

        loadData();
    });

    $('#btnPrint').on('click', function () {

        var data = {
            'ActionStatusCodeID': $('#ActionStatusCodeID').val(),
            'StartDate': $('#StartDate').val(),
            'EndDate': $('#EndDate').val(),
            'DateType': $('#DateType').val(),
            'AssignedToPersonID': $('#AssignedToPersonID').val(),
            'ActionTypeCodeID': $('#ActionTypeCodeID').val(),
        }
        var _target = $("body").data("print-document-on") == "NewWindow" ? 'target="_blank"' : '';
       $.download($('#hdnCurrentSessionGuidPath').val()+'/Inquiry/PrintToDoList', data, "POST",_target);
    });
});