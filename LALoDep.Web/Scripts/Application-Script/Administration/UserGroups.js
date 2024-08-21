//var page = (function () {
//    return {
//        $table: $('#searchUserGroups'),
//        ttl: 600000 /*In Milli seconds*/
//    };
//})();

$(document).ready(function () {
    if ($onViewLoad == "True") {
        //ResetPageState();
        $.ajax({
            type: "GET", url: '/UserGroups/GetData',
            success: function (data) {
                setData(data);
            },
            dataType: 'json'
        });
    } else {
        /*Load Default state of the form*/
        //LoadPreviousPageState();
    }
});

var oTable = $('#searchUserGroups').dataTable({
    "lengthMenu": [50],
    "lengthChange": false,
    "paging": true,
    "searching": true,
    "bSort": false,

    "columns": [
    {
        "render": function (data, type, full, meta) {
            return ('<a href="/UserGroups/GroupSecurity/' + full.EncryptedGroupID + '">' + "Edit Security" + '</a>');
        }
    },
    {
        "render": function (data, type, full, meta) {
            return ('<a href="/UserGroups/EditUserGroup/?GroupId=' + full.EncryptedGroupID + '">' + full.Group + '</a>');
        }
    },
   {
       "render": function (data, type, full, meta) {
           return ('<a href="/UserGroups/EditUserGroup/?GroupId=' + full.EncryptedGroupID + '&Mode=New">' + "Add New Group - Use (" + full.Group + ") As Template" + '</a>');
       }
   },
   {
       "render": function (data, type, full, meta) {
           return ('<a class="btn btn-danger btn-xs delete ' + (full.UserCount > 0 ? ' hidden' : '') + '" href="#" data-id="' + full.GroupId + '"><i class="fa fa-trash-o"></i>Delete</a>');
       }
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

$('body').on('click','.delete', function () {
    $this = $(this);
    confirmBox("Are you sure you want to delete?", function (result) {
        if (result) {
         
            $.ajax({
                type: "POST",
                url: '/UserGroups/UserGroupDelete/' + $this.attr('data-id'),
                success: function (result) {
                    if (result.Status == "Done") {
                        notifySuccess('Group deleted Successfully.');
                         $this.parents('tr').hide();
                    } else {
                        document.location.href = result.URL;
                    }
                },
                dataType: 'json'
            });
        }
    });
});
$('#searchUserGroups').on('draw.dt', function () {
    if (device.mobile() || device.tablet())//if mobile or tablet
    {
        $('#searchUserGroups_info').parent().addClass('col-xs-4').removeClass('col-xs-6');
        $('#searchUserGroups_paginate').parent().addClass('col-xs-8').removeClass('col-xs-6');
    }

});

$(window).on("keydown", handleHotkey);

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

$(window).bind('resize', function () {
    $('#searchUserGroups').css('width', '100%');
});

$('#add').on("click", function () {
    window.location.href = "/UserGroups/EditUserGroup?Mode=New";
});

function setData(data) {
    oTable.fnClearTable();
    if (data.data.length > 0) {
        oTable.fnAddData(data.data);
        fitCalculatedHeightForSearchDataTable();
    } else
        Notify('No results found.', 'bottom-right', '5000', 'blue', 'fa-frown-o', true);


}

$(window).bind('resize', function () {
    fitCalculatedHeightForSearchDataTable();
});



//function ResetPageState() {
//    simpleStorage.deleteKey(page.mainKey);
//    simpleStorage.deleteKey(page.resultPageIdKey);
//    simpleStorage.deleteKey(page.rowSelectedKey);
//};

//page.$table.on('page.dt', function () {
//    var table = page.$table.dataTable();
//    var pageInfo = table.fnPagingInfo();
//    var pageNumber = pageInfo.iPage;
//    simpleStorage.set(page.resultPageIdKey, pageNumber, { TTL: page.ttl });
//});

//function LoadPreviousPageState() {
//    var pageState = simpleStorage.get(page.mainKey);
//    if (pageState && pageState.formData && pageState.results) {

//        /*Load Results*/
//        setData(pageState.results);

//        /*Load Page #*/
//        var pageNumber = simpleStorage.get(page.resultPageIdKey);
//        if (pageNumber) page.$table.dataTable().fnPageChange(pageNumber);

//        /*Load Form inputs*/
//        var formData = pageState.formData;

//        /*Highlight selected Row*/
//        var rowIndex = simpleStorage.get(page.rowSelectedKey);
//        if (rowIndex) {
//            page.$table.find("tr:nth-child(" + (rowIndex + 1) + ")").css("background", "#f3f7b4");
//        }

//    }
//};

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