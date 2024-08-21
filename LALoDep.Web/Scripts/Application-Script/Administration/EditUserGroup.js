var page = (function () {
    return {
        $table: $('#editUserGroup'),
        ttl: 600000 /*In Milli seconds*/
    };
})();

$(document).ready(function () {
    var groupId = $("#hdn_JcatsGroupID").val();
        ResetPageState();
        $.ajax({
            type: "GET", url: '/UserGroups/GetGroupAgencyList/?GroupId=' + groupId,
            success: function (data) {
                setData(data);
                //eventChangeBind();
            },
            dataType: 'json'
        });
});
var oTable = $('#editUserGroup').dataTable({
    //"lengthMenu": [20],
    //"lengthChange": false,
    "searching": false,
    "bSort": false,
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,

    "columns": [
        {
            "render": function (data, type, full, meta) {
                if (full.Selected == 0)
                {
                    return ('<input class="accessChk" type="checkbox" data-jcatsgroupagencyid="' + full.JcatsGroupAgencyID + '" data-selected="' + full.Selected + '" data-id="' + full.AgencyId + '" value="' + full.AgencyId + '">');
                }
                else
                {
                    return ('<input  class="accessChk" type="checkbox" data-jcatsgroupagencyid="' + full.JcatsGroupAgencyID + '" data-selected="' + full.Selected + '" data-id="' + full.AgencyId + '" value="' + full.AgencyId + '" checked="true">');
                }
               
            }
        },
    { "data": "AgencyName" }
    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    "fnDrawCallback": function (oSettings) {
    },
    "deferRender": true
}); 

if (device.mobile() || device.tablet()) {
    var middleColumn = oTable.DataTable().column(2);
    middleColumn.visible(false);
}

$('#editUserGroup').on('draw.dt', function () {
    if (device.mobile() || device.tablet())//if mobile or tablet
    {
        $('#searchUserGroups_info').parent().addClass('col-xs-4').removeClass('col-xs-6');
        $('#searchUserGroups_paginate').parent().addClass('col-xs-8').removeClass('col-xs-6');
    }

});

$('#cancel').on('click', function () {
    window.location.href = "/UserGroups/Search";
});

$(window).on("keydown", handleHotkey);
$("#save").on("click", function() {
    saveData(1);
});
$("#saveandReturn").on("click", function () {
    saveData(2);
});
function handleHotkey(e) {
    if (!e.ctrlKey) return;
    switch (String.fromCharCode(e.keyCode).toLowerCase()) {
        case 'r':
            $('#reset').trigger('click');
            e.preventDefault();
            break;
        default:
            break;
    }
}

function saveData(burronID) {
    IPadKeyboardFix();
    var changedBoxes = [];
    var newGroupName = $("#JcatsGroup_JcatsGroupName").val();
    var oldGroupName = $("#OldGroupName").val();
    var groupId = $("#JcatsGroup_JcatsGroupID").val();

    var $accessTr = $("#editUserGroup tbody tr");

    for (var indx = 0; indx < $accessTr.length; indx++) {
        $tr = $accessTr.eq(indx);
        $IsAccess = $tr.find(".accessChk");
        var oldSelected = $IsAccess.attr("data-selected");
        var checked = "0";
        if ($IsAccess.is(':checked'))
            checked="1";
        
        if (checked != oldSelected) {
            
            var accessRightgList = {
                Id: $IsAccess.attr("data-id"),
                Checked: $IsAccess.is(':checked'),
                JcatsGroupAgencyID: $IsAccess.attr("data-jcatsgroupagencyid"),
            };
            changedBoxes.push(accessRightgList);
        }
    }

    //$("[changed]").each(function () {
    //    changedBoxes.push({ Id: $(this).val(), Checked: $(this).is('checked'), OldSelected: $(this).attr('data-selected') });
    //});
    var data = {
        changedBoxes: changedBoxes,
        newGroupName: newGroupName,
        oldGroupName: oldGroupName,
        groupId: groupId
    }
    $.ajax({
        type: "POST", url: '/UserGroups/SaveChangedBoxes',
        data: data,
        dataType: 'json',
        success: function (result) {
            if (result.isSuccess) {
                if (burronID == 1)
                {
                    window.location.href = "/UserGroups/Search";
                }
                else if (burronID == 2) {
                    window.location.href = window.location.href;
                    //Notify('Data save successfully', 'bottom-right', '4000', 'success', 'fa-check', true);
                }
            }
            else {
                Notify('There is something wrong while processing request.', 'bottom-right', '4000', 'danger', 'fa-info', true);
            }
        }
    });
    

}
$(window).bind('resize', function () {
    $('#editUserGroup').css('width', '100%');
    fitCalculatedHeightForSearchDataTable();
});

function setData(data) {
    oTable.fnClearTable();
    if (data.data.length > 0) {
        oTable.fnAddData(data.data);
        fitCalculatedHeightForSearchDataTable();
    } else
        Notify('No results found.', 'bottom-right', '5000', 'blue', 'fa-frown-o', true);


}

function onCaseNumberClick(e) {
    var $this = $(this);
    e.preventDefault();
    simpleStorage.set(page.rowSelectedKey, $this.closest('tr').index(), { TTL: page.ttl });
    document.location = $this.attr('href');
}


function ResetPageState() {
    simpleStorage.deleteKey(page.mainKey);
    simpleStorage.deleteKey(page.resultPageIdKey);
    simpleStorage.deleteKey(page.rowSelectedKey);
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

        /*Highlight selected Row*/
        var rowIndex = simpleStorage.get(page.rowSelectedKey);
        if (rowIndex) {
            page.$table.find("tr:nth-child(" + (rowIndex + 1) + ")").css("background", "#f3f7b4");
        }

    }
};

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