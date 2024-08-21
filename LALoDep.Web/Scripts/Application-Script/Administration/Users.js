var page = (function () {
    return {
        $form: $('#UsersView-search-form'),
        $table: $('#UsersView'),
        mainKey: 'UsersView-search-form',
        resultPageIdKey: 'UsersView-search-form-page#',
        rowSelectedKey: 'UsersView-search-form-row#',
        ttl: 600000 /*In Milli seconds*/
    };
})();

var ResetPageState = function () {
    simpleStorage.deleteKey(page.mainKey);
    simpleStorage.deleteKey(page.resultPageIdKey);
    simpleStorage.deleteKey(page.rowSelectedKey);
};

var SaveFormState = function () {
    var formData = {};
    page.$form.serializeArray().map(function (item) {
        formData[item.name] = item.value;
    });
    var pageState = { formData: formData  };
    simpleStorage.set(page.mainKey, pageState, { TTL: page.ttl });
};

var LoadPreviousFormState = function () {
    var flag = false;
    var pageState = simpleStorage.get(page.mainKey);
    if (pageState && pageState.formData) {
        /*Load Form inputs*/
        var formData = pageState.formData;
        page.$form.find('*').filter(':input').each(function () {
            var $this = $(this);
            if (formData[$this.attr('id')]) {
                $this.val(formData[$this.attr('id')]);
                flag = true;

            }
        });
    }
    $('#chkActiveUserOnly').prop('checked', $('#ActiveUserOnly').val() == 'true');
    $('#chkOpenPositionsOnly').prop('checked', $('#OpenPositionsOnly').val() == 'true');

    formDataPased = page.$form.serialize();
    return flag;
};

var oTable = $('#UsersView').dataTable({
    //"lengthMenu": [20],
    //"lengthChange": false,
    "searching": false,
    "bSort": true,
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,

    "columns": [
        //{
        //    "render": function(data, type, full, meta) {
        //        return ('<a href="/CountyCounselList/AddEditCountyCounsel/' + full.PersonID + '">' + full.LastName + '</a>');
        //    }
        //},
        { "data": "Name" },
        { "data": "Agency", "orderable": false }, 
        {
            "orderable": false,
            "render": function (data, type, full, meta) {
                return ('<a href="/UserGroups/GroupSecurity/' + full.JcatsGroupID + '?page=userlist">' + (full.SecurityGroup != null ? full.SecurityGroup : "") + '</a>');
            }
        }
        ,
        { "data": "Role", "orderable": false },
        { "data": "UserLevel" },
        { "data": "LastLogin", "orderable": false },
        { "data": "UserEndDate", "orderable": false, },
        {
            "orderable": false,
            "render": function (data, type, full, meta) {
                return ('<a href="/Users/AddEdit?jcatsUserID=' + full.JcatsUserID + '&personID=' + full.PersonID + '">' + (full.LoginName != null ? full.LoginName : "") + '</a>');
            }
        },
        {
            "orderable": false,
            "render": function (data, type, full, meta) {
                return (' <a class="btn btn-info btn-xs edit" href="/Users/PersonLegalNumbers/' + full.PersonID + '"><i class="fa fa-edit"></i> Legal Nbrs</a>');
            }
        },
        {
            "orderable": false,
            "render": function (data, type, full, meta) {
                return (' <a class="btn btn-info btn-xs dept" href="/Users/Department/' + full.PersonID + '"><i class="fa fa-edit"></i> Dept</a>');
            }
        },
        {
            "orderable": false,
            "render": function (data, type, full, meta) {
                return (' <a class="btn btn-info btn-xs contact" href="/Users/UserContact/' + full.PersonID + '"><i class="fa fa-edit"></i> Contact</a>');
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

$('#UsersView').on('draw.dt', function () {
    if (device.mobile() || device.tablet())//if mobile or tablet
    {
        $('#UsersView_info').parent().addClass('col-xs-4').removeClass('col-xs-6');
        $('#UsersView_paginate').parent().addClass('col-xs-8').removeClass('col-xs-6');
    }

});

$('#search').on('click', function () {
    var formInvalid = $('#LastName').val().length == 0
        && $('#FirstName').val().length == 0
        && $('#AgencyID').val().length == 0
        && $('#JcatsGroupID').val().length == 0
        && $('#RoleTypeCodeID').val().length == 0;

    if (formInvalid) {
        notifyDanger('At least one search parameter is required.');
        return false;
    } else {
        loadData();
        return true;
    }
});

$('#btnAdd').on("click", function () {
    window.location.href = "/Users/AddEdit?lastname=" + $('#LastName').val() + "&FirstName=" + $('#FirstName').val();
});


//href="/Case/Main/' + full.ClientCaseId + '"
$(window).bind('resize', function () {
    $('#UsersView').css('width', '100%');
    fitCalculatedHeightForSearchDataTable();
});

function loadData() {
    var $form = $('#UsersView-search-form');
    IPadKeyboardFix();
    var data = getData();

    $.ajax({
        type: "POST", url: '/Users/Search', data: data,
        success: function (data) {

            SaveFormState();
            setData(data);
        },
        dataType: 'json'
    });

}

function setData(data) {
    oTable.fnClearTable();
    if (data.data.length > 0) {
        oTable.fnAddData(data.data);
        fitCalculatedHeightForSearchDataTable();
    } else
        notifyInfo('No results found.');
}

function getData() {
    var fData = $('#UsersView-search-form').serialize();
    return fData;
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
        calc_height = calc_height < 250 ? 250 : calc_height;
        $('#divSearchResult .dataTables_scrollBody').css('max-height', calc_height + 'px');
        oTable.fnAdjustColumnSizing();
    }
    return calc_height;
}


$(function () {

    if (LoadPreviousFormState()) {
        $('#search').click();
    }
    else
        ResetPageState();
})