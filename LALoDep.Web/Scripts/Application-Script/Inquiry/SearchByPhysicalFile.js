var page = (function () {
    return {
        $form: $('#SearchByPhysicalFile-search-form'),
        $table: $('#SearchByPhysicalFile'),
        mainKey: 'SearchByPhysicalFile-search-form',
        resultPageIdKey: 'SearchByPhysicalFile-search-form-page#',
        rowSelectedKey: 'SearchByPhysicalFile-search-form-row#',
        ttl: 600000 /*In Milli seconds*/
    };
})();

var oTable = $('#SearchByPhysicalFile').dataTable({
    //"lengthMenu": [20],
    //"lengthChange": false,
    "searching": false,
    "bSort": false,
    "scrollY": "auto",
    "scrollCollapse": true,
    "paging": false,

    "columns": [
        { "data": "PhysicalFileName" },
        { "data": "Client" },
        { "data": "DOB" },
        { "data": "Role" },
        {
            "render": function (data, type, full, meta) {
                return ('<a href="/Case/Main/' + full.EncryptedCaseID + '">' + full.CaseNumber + '</a>');
            }
        },
        {
            "render": function (data, type, full, meta) {
                return (full.CloseDate == null ? '' : '<a href="javascript:void(0);" class="petition-close-date" data-id="' + full.EncryptedRoleID + '">' + full.CloseDate + '</a>')
            }
        },
        { "data": "PetitionDocketNumber" },
        { "data": "Attorney" }
    ],
    "loadingRecords": "Loading...",
    "processing": "Processing...",
    fnRowCallback: function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {

        if (aData.RoleClient == 1) {
            $(nRow).addClass('highLightBlue');
        }
    },
    "deferRender": true
});

if (device.mobile() || device.tablet()) {
    var middleColumn = oTable.DataTable().column(2);
    middleColumn.visible(false);
}

function setData(data) {
    oTable.fnClearTable();
    if (data.data.length > 0) {
        oTable.fnAddData(data.data);
        fitCalculatedHeightForSearchDataTable();
    } else
        notifyBlue('No results found.');


}

function getData() {
    var fData = $('#SearchByPhysicalFile-search-form').serialize();
    return fData;
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

function SavePageState(results) {
    var formData = {};
    page.$form.serializeArray().map(function (item) {
        formData[item.name] = item.value;
    });
    var pageState = { formData: formData, results: results };

    simpleStorage.set(page.mainKey, pageState, { TTL: page.ttl });
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
        $('#SearchByPhysicalFile-search-form *').filter(':input').each(function () {
            var $this = $(this);
            if (formData[$this.attr('id')])
                $this.val(formData[$this.attr('id')]);
        });

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

$(window).bind('resize', function () {
    $('#searchInvoiceQueue').css('width', '100%');
    fitCalculatedHeightForSearchDataTable();
});

$('#SearchByPhysicalFile').on('draw.dt', function () {
    if (device.mobile() || device.tablet())//if mobile or tablet
    {
        $('#SearchByPhysicalFile_info').parent().addClass('col-xs-4').removeClass('col-xs-6');
        $('#SearchByPhysicalFile_paginate').parent().addClass('col-xs-8').removeClass('col-xs-6');
    }

});
$('#search').on('click', function () {
    var $form = $('#SearchByPhysicalFile-search-form');
    $form.submit();
});

$('#SearchByPhysicalFile-search-form').on('submit', function (e) {
    e.preventDefault();
    var $form = $('#SearchByPhysicalFile-search-form');

    var formInvalid = true;

    if ($('#FileName1').val() == ""
        && $('#FileName2').val() == ""
        && $('#FileName3').val() == ""
        && $('#ClientLastName').val() == ""
        && $('#ClientFirstName').val() == ""
        && $('#CaseNumber').val() == ""
        && $('#AgencyID').val() == "") {
        formInvalid = false;
    }

    if (!formInvalid) {
        notifyDanger('At least one search parameter is required.');
        return false;
    }
    if (!IsInvalidCharactersNotExistsInSearchFields($form))
        return false;
 
    IPadKeyboardFix();
    
    var data = getData();
    $.ajax({
        type: "POST", url: '/SearchByPhysicalFile/Search', data: data,
        success: function (data) {
            setData(data);
            SavePageState(data);
        },
        dataType: 'json'
    });

});

$("#SearchByPhysicalFile").on("click", ".petition-close-date", function () {
    OpenPopup('/Case/PetitionAndRolePopUp/' + $(this).attr('data-id'), 'Petition Info');
});

$(document).ready(function () {
    if ($onViewLoad != "True") {
        ResetPageState();
        var data = getData();
        $.ajax({
            type: "POST", url: '/SearchByPhysicalFile/Search', data: data,
            success: function (data) {
                setData(data);
                SavePageState(data);
            },
            dataType: 'json'
        });
    }
});