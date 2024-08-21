$BaseURL = '/';
var origin_wrapper_heightL = 0, origin_content_heightL = 0;
var origin_wrapper_heightR = 0, origin_content_heightR = 0;

function CheckAllRestrictedItems() {
    if ($('#chkAddAllRestrictedItems').prop("checked") == true)
        $(".chkAddRestrictedItemClass").prop("checked", true);
    else
        $(".chkAddRestrictedItemClass").prop("checked", false);

}

function CheckAllAccessRights() {

    if ($('#chkDeleteAllAccessRights').prop("checked") == true)
        $(".chkDeleteAccessRightsClass").prop("checked", true);
    else
        $(".chkDeleteAccessRightsClass").prop("checked", false);

}

function GetData() {
    var data = {
        AccessRightsItemList: [],
        RestrictedItemsList: [],
        'JcatsGroupID':$('#JcatsGroupID').val()
    };
    //get del list
    var $accessTr = $("#accessRightsItemList tbody tr");
   
    for (var indx = 0; indx < $accessTr.length; indx++) {
        $tr = $accessTr.eq(indx);
        $IsDelete = $tr.find(".chkDeleteAccessRightsClass");
        if ($IsDelete.is(':checked')) {
            var deleteAccessRightgList = {
                SecurityItemID: $tr.data("securityitemid"),
                JcatsGroupID: $tr.data("jcatsgroupid"),
            };
            data.AccessRightsItemList.push(deleteAccessRightgList);
        }
    }

    //get add List
    var $ristrictedTr = $("#restrictedItemsList tbody tr");
    for (var i = 0; i < $ristrictedTr.length; i++) {
        $Rtr = $ristrictedTr.eq(i);
        $IsAdd = $Rtr.find(".chkAddRestrictedItemClass");
        if ($IsAdd.is(':checked'))
        {
            var restreicteList = {
                SecurityItemID: $Rtr.data("securityitemid"),
            };
            data.RestrictedItemsList.push(restreicteList);
        }

    }
    return data;
}

function SaveData(buttonID) {
    IPadKeyboardFix();
    var data = JSON.stringify(GetData());

    $.ajax({
        type: "POST", dataType: 'json', url: '/UserGroups/GroupSecuritySave', data: data, contentType: "application/json",
        success: function (result) {
            if (result.isSuccess) {
                
                if (buttonID == 1) {
                    if (Contains(window.location.href, '?page=userlist')) {
                        window.location.href = $BaseURL + 'Users';
                    }
                    else {
                        window.location.href = $BaseURL + 'UserGroups/Search';
                    }
                }
                else if (buttonID == 2) {

                    window.location.href = window.location.href.replace('#','');
                }
            }
            else {
                Notify('There is something wrong while processing request.', 'bottom-right', '4000', 'danger', 'fa-info', true);
            }
        }
    });
}

$('#btnCancel').on('click', function () {
    if (Contains(window.location.href, '?page=userlist')) {
        window.location.href = $BaseURL + 'Users';
    }
    else {
        window.location.href = $BaseURL + 'UserGroups/Search';
    }
});

$('#btnSave').on('click', function () {
    SaveData(1);
});
$('#btnSaveAndAddMore').on('click', function () {
    SaveData(2);
});

$(document).ready(function () {
    var oTableL = $('#accessRightsItemList').dataTable({
        "searching": false,
        "bSort": false,
        "scrollY": "auto",
        "scrollCollapse": true,
        "paging": false,
    });
    var oTableR = $('#restrictedItemsList').dataTable({
        "searching": false,
        "bSort": false,
        "scrollY": "auto",
        "scrollCollapse": true,
        "paging": false,
    });

    fitCalculatedHeightForSearchDataTableL();
    fitCalculatedHeightForSearchDataTableR();


    //origin_wrapper_heightL = $('body>div.container-fluid').height();
    //origin_content_heightL = $('#accessRightsItemDiv .dataTables_scrollBody').height();
    //origin_wrapper_heightR = $('body>div.container-fluid').height();
    //origin_content_heightR = $('#restrictedItemsDiv .dataTables_scrollBody').height();

    $(window).bind('resize', function () {
        $('#accessRightsItemList').css('width', '100%');
        
        fitCalculatedHeightForSearchDataTableL();
        
    });
    $(window).bind('resize', function () {
        $('#restrictedItemsList').css('width', '100%');
        fitCalculatedHeightForSearchDataTableR();
    });


    function fitCalculatedHeightForSearchDataTableL() {
        var calc_height = 0;
        calc_height = $(window).height();
        var _offset = 25;
        origin_wrapper_heightL = $('body>div.container-fluid').height();
        origin_content_heightL= $('#accessRightsItemDiv .dataTables_scrollBody').height();

        $("#accessRightsItemDiv .dataTables_scrollBody").children().first().parentsUntil("body").each(function () {

            $(this).siblings().each(function () {
                    if (calc_height > $(this).outerHeight(true) && $(this).css('display') != 'none' && !$(this).hasClass("right-panel")) {
                    //console.log(calc_height + " - " + $(this).outerHeight(true));

                    console.log($(this).siblings());
                    calc_height = calc_height - $(this).outerHeight(true);
                }
            });
            _offset = _offset + $(this).outerHeight(true) - $(this).height();
        });

        //console.log("calc :" + calc_height + " offset: " + _offset);
        calc_height = calc_height - _offset;
        //console.log("total: " + calc_height);
        $('#accessRightsItemDiv .dataTables_scrollBody').css('max-height', calc_height + 'px');
        oTableL.fnAdjustColumnSizing();
        return calc_height;
    }

    function fitCalculatedHeightForSearchDataTableR() {
        var calc_heightR = 0;
        calc_heightR = $(window).height();
        var _offsetR = 25;
        origin_wrapper_heightR = $('body>div.container-fluid').height();
        origin_content_heightR = $('#restrictedItemsDiv .dataTables_scrollBody').height();

        $("#restrictedItemsDiv .dataTables_scrollBody").children().first().parentsUntil("body").each(function () {
            $(this).siblings().each(function () {
                if (calc_heightR > $(this).outerHeight(true) && $(this).css('display') != 'none' && !$(this).hasClass("left-panel")) {
                    //console.log(calc_height + " - " + $(this).outerHeight(true));
                    console.log($(this).siblings());
                    calc_heightR = calc_heightR - $(this).outerHeight(true);
                }
            });
            _offsetR = _offsetR + $(this).outerHeight(true) - $(this).height();
        });

        //console.log("calc :" + calc_height + " offset: " + _offset);
        calc_heightR = calc_heightR - _offsetR;
        console.log("total: " + calc_heightR);
        $('#restrictedItemsDiv .dataTables_scrollBody').css('max-height', calc_heightR + 'px');
        oTableR.fnAdjustColumnSizing();
        return calc_heightR;
    }

});
