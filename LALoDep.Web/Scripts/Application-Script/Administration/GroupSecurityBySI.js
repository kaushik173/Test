$BaseURL = '/';
var $anythingChangeOnPage = false;
var activeTable = $('#userGroupActiveTable').dataTable({
    "scrollY": "auto",
    "ordering": false,
    "searching": false,
    "paging": false,
    "bSort": false,
    "columns": [
      {
          "data": "",
          "render": function (data, type, full, meta) {
              return ('<input type="checkbox" data-value="' + full.JcatsGroupID + '" id="checkbox' + full.JcatsGroupID + '" class="checkBoxClassActive  "/>')
          }
      },
        { "data": "JcatsGroupName" }

    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    "deferRender": true
});


var inactiveTable = $('#userGroupInactiveTable').dataTable({
    "scrollY": "auto",
    "ordering": false,
    "paging": false,
    "searching": false,
    "bSort": false,
    "columns": [
      {
          "data": "",
          "render": function (data, type, full, meta) {
              return ('<input type="checkbox" data-value="' + full.JcatsGroupID + '" id="checkbox' + full.JcatsGroupID + '" class="checkBoxClassInactive "/>')
          }
      },

        { "data": "JcatsGroupName" }


    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    "deferRender": true
});



var origin_wrapper_height = 0, origin_content_height = 0;

$(document).ready(function () {

    $('body').on('change', 'input.checkBoxClassActive,input.checkBoxClassInactive', function () {

        $anythingChangeOnPage = true;



    });

    $(window).on("keydown", handleHotkey);
    function handleHotkey(e) {
        if (!e.ctrlKey) return;
        switch (String.fromCharCode(e.keyCode).toLowerCase()) {
            case 'l':
                $('#cancel').trigger('click');
                e.preventDefault();
                break;
            case 'f':
                $('#btnFilter').trigger('click');
                e.preventDefault();
                break;
            case 's':
                $('#save').trigger('click');
                e.preventDefault();
                break;
            case 'r':
                $('#return').trigger('click');
                e.preventDefault();
                break;
            default:
                break;
        }
    }

    $('#cancel').on('click', function () {
        IPadKeyboardFix()
        window.location.href = $BaseURL + "UserGroups/GroupSecurity/" + groupId;
    });

    $('#save').on('click', function () {

        var isValid = Validations();

        if (isValid) {
            SaveData(2);
        }


    });

    $('#return').on('click', function () {
        var isValid = Validations();

        if (isValid) {
            SaveData(1);
        }

    });
    $('#btnFilter').on('click', function () {
        PopulateData();
    });



    $(window).bind('resize', function () {
        $('#user-group-add-edit-form').css('width', '100%');
        sizeForTable();
    });

    PopulateData();

});
function PopulateData() {
    var model = {
        'SecurityItemId': $('#SecurityItemId').val(),
        'AgencyId': $('#AgencyId').val(),
        'JcatsGroupName': $('#JcatsGroupName').val(),
    };

    $.ajax({
        type: "POST",
        url: $BaseURL + 'UserGroups/UserGroupListForActiveAndInactive',
        data: { 'model': model },
        dataType: 'json',
        success: function (data) {

            setData(data);



        }

    });
}


function Validations() {

    IPadKeyboardFix()

    if (!$anythingChangeOnPage) {
        Notify('Nothing was changed.', 'bottom-right', '4000', 'danger', 'fa-warning', true);
        return false;
    }

    return true;
}
function SaveData(buttonId) {

    var fdata = getdata(buttonId);

    $.ajax({
        type: "POST",
        url: $BaseURL + 'UserGroups/UserGroupBySecurityItemSave',
        data: { 'model': fdata },
        dataType: 'json',
        success: function (data) {
            if (data.URL != '') {
                if (buttonId == 1) {
                    Notify('Data saved!', 'bottom-right', '2000', 'success', 'fa-check', true);

                    setTimeout(function () {
                        PopulateData();
                    }, 500);
                } else {
                    Notify('Data saved!', 'bottom-right', '2000', 'success', 'fa-check', true);

                    setTimeout(function () {
                                 window.location.href = $BaseURL + "UserGroups/GroupSecurity/" + groupId
        
                    }, 500);
               }

            }
        }
    });





}
function setData(data) {
    activeTable.fnClearTable();
    inactiveTable.fnClearTable();
    if (data.length > 0) {
        if (data[0].data.length > 0) {
            activeTable.fnAddData(data[0].data);
        }
        if (data[1].data.length > 0) {
            inactiveTable.fnAddData(data[1].data);
        }
        sizeForTable();
    }
}

function getdata(buttonId) {
    var authorizedToDeactivateGroupIds = '';
    $('input.checkBoxClassActive:checked').each(function () {
        authorizedToDeactivateGroupIds += $(this).attr('data-value') + ',';
    });
    var notAuthorizedToActivateGroupIds = '';
    $('input.checkBoxClassInactive:checked').each(function () {
        notAuthorizedToActivateGroupIds += $(this).attr('data-value') + ',';
    });

    var data = {
        'SecurityItemId': $('#SecurityItemId').val(),
        'AuthorizedToDeactivateGroupIds': authorizedToDeactivateGroupIds,
        'NotAuthorizedToActivateGroupIds': notAuthorizedToActivateGroupIds,
        'ButtonId': buttonId
    };


    return data;
}

function sizeForTable() {
    var topdivHeight = $('#topdivHeight').height();
    var calc_height = $(window).height();

    var offSet = 290;
    var remainingHeight = calc_height - offSet - topdivHeight;
    $('#userGroupActiveInactiveTablesDiv .dataTables_scrollBody').css('max-height', remainingHeight + 'px');
    activeTable.fnAdjustColumnSizing();
    inactiveTable.fnAdjustColumnSizing();
    return remainingHeight;

}
function CheckAllInactive() {
    $anythingChangeOnPage = true;

    if ($('#checkboxAllInactive').prop("checked") == true) {
        $(".checkBoxClassInactive").prop("checked", true);
    }
    else {
        $(".checkBoxClassInactive").prop("checked", false);
    }
}
function CheckAllActive() {
    $anythingChangeOnPage = true;

    if ($('#checkboxAllActive').prop("checked") == true) {
        $(".checkBoxClassActive").prop("checked", true);
    }
    else {
        $(".checkBoxClassActive").prop("checked", false);
    }
}