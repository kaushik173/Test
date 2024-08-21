

var page = (function () {
    return {
        $form: $('#case-search-form'),
        $table: $('#searchCase'),
        mainKey: 'case-AddApptCase-form',
        resultPageIdKey: 'case-search-form-page#',
        rowSelectedKey: 'case-search-form-row#',
        ttl: 600000 /*In Milli seconds*/
    };
})();

var oTable = $('#searchCase').dataTable({

    "lengthMenu": [50],
    "lengthChange": false,
    "paging": true,
    "searching": false,
    "bSort": false,

    "columns": [

        {
            "render": function (data, type, full, meta) {
                if (full.CaseNumber !== null)
                    return ('<a href="/Case/Main/' + full.EncryptedCaseID + '" target="_blank">' + full.CaseNumber + '</a>');
                else
                    return '';

            }
        }, { "data": "Agency" },
        { "data": "Role" },
         { "data": "PersonNameLast" },
        { "data": "PersonNameFirst" },
        { "data": "DOB" },
        { "data": "Sex", "title": "Gender" },
         { "data": "PetitionDocketNumber" },
         { "data": "ClosedDate" },
        { "data": "LeadAttorney" }


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

$('#searchCase').on('draw.dt', function () {
    if (device.mobile() || device.tablet())//if mobile or tablet
    {
        $('#searchCase_info').parent().addClass('col-xs-4').removeClass('col-xs-6');
        $('#searchCase_paginate').parent().addClass('col-xs-8').removeClass('col-xs-6');
    }

});



LoadPreviousPageState();


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

$('#search').on('click', function () {
    $('#AddNew').val('0');
    Search();

});
$('#searchAndAdd').on('click', function () {
    $('#AddNew').val('0');
    Search();

});

$('#btnBypassAndAdd').on('click', function () {
    $('#AddNew').val('1');
    Search();

});


$(window).bind('resize', function () {
    $('#searchCase').css('width', '100%');
});

function Search() {
    IPadKeyboardFix();
    var $form = $('#case-search-form');
    if (!IsInvalidCharactersNotExistsInSearchFields($('#case-search-form')))
        return false;
    if ($('#AgencyID').val() == '') {
        $('#AgencyID').focus();
        Notify('Agency is required.', 'bottom-right', '5000', 'danger', 'fa-warning', true);
        return false;
    }
    if ($('#CaseNumber1').val() == '') {
        $('#CaseNumber1').focus();
        Notify('Case Number is required.', 'bottom-right', '5000', 'danger', 'fa-warning', true);
        return false;
    }
    if ($('#AppointmentDate').val() == '') {
        $('#AppointmentDate').focus();
        Notify('Appointment Date is required.', 'bottom-right', '5000', 'danger', 'fa-warning', true);
        return false;
    } if ($('#DepartmentID').val() == '') {
        $('#DepartmentID').focus();
        Notify('Department is required.', 'bottom-right', '5000', 'danger', 'fa-warning', true);
        return false;
    }


    if ($("#MotherLastName").val() != '' && $("#MotherFirstName").val() == '') {
        $("#MotherFirstName").focus();
        notifyDanger("Mother First Name is required.");
        return false;
    }
    if ($("#MotherFirstName").val() != '' && $("#MotherLastName").val() == '') {
        $("#MotherLastName").focus();
        notifyDanger("Mother Last Name is required.");
        return false;
    }
    if ($("#MotherDOB").val() != '' && $("#MotherLastName").val() == '') {
        $("#MotherLastName").focus();
        notifyDanger("Mother Last Name is required.");
        return false;
    }

    if ($("#ChildLastName1").val() != '' && $("#ChildFirstName1").val() == '') {
        $("#ChildFirstName1").focus();
        notifyDanger("Child 1 First Name is required.");
        return false;
    }
    if ($("#ChildFirstName1").val() != '' && $("#ChildLastName1").val() == '') {
        $("#ChildLastName1").focus();
        notifyDanger("Child 1 Last Name is required.");
        return false;
    }
    if ($("#Child1DOB").val() != '' && $("#ChildLastName1").val() == '') {
        $("#ChildLastName1").focus();
        notifyDanger("Child 1 Last Name is required.");
        return false;
    }

    if ($("#ChildLastName2").val() != '' && $("#ChildFirstName2").val() == '') {
        $("#ChildFirstName2").focus();
        notifyDanger("Child 2 First Name is required.");
        return false;
    }
    if ($("#ChildFirstName2").val() != '' && $("#ChildLastName2").val() == '') {
        $("#ChildLastName2").focus();
        notifyDanger("Child 2 Last Name is required.");
        return false;
    }
    if ($("#Child2DOB").val() != '' && $("#ChildLastName2").val() == '') {
        $("#ChildLastName2").focus();
        notifyDanger("Child 2 Last Name is required.");
        return false;
    }
    if ($("#ChildLastName3").val() != '' && $("#ChildFirstName3").val() == '') {
        $("#ChildFirstName3").focus();
        notifyDanger("Child 3 First Name is required.");
        return false;
    }
    if ($("#ChildFirstName3").val() != '' && $("#ChildLastName3").val() == '') {
        $("#ChildFirstName3").focus();
        notifyDanger("Child 3 Last Name is required.");
        return false;
    }
    if ($("#Child3DOB").val() != '' && $("#ChildLastName3").val() == '') {
        $("#ChildLastName3").focus();
        notifyDanger("Child 3 Last Name is required.");
        return false;
    }
    if ($("#FatherLastName1").val() != '' && $("#FatherFirstName1").val() == '') {
        $("#FatherFirstName1").focus();
        notifyDanger("Father 1 First Name is required.");
        return false;
    }
    if ($("#FatherFirstName1").val() != '' && $("#FatherLastName1").val() == '') {
        $("#FatherLastName1").focus();
        notifyDanger("Father 1 Last Name is required.");
        return false;
    }
    if ($("#Father1DOB").val() != '' && $("#FatherLastName1").val() == '') {
        $("#FatherLastName1").focus();
        notifyDanger("Father 1 Last Name is required.");
        return false;
    }

    if ($("#FatherLastName2").val() != '' && $("#FatherFirstName2").val() == '') {
        $("#FatherFirstName2").focus();
        notifyDanger("Father 2 First Name is required.");
        return false;
    }
    if ($("#FatherFirstName2").val() != '' && $("#FatherLastName2").val() == '') {
        $("#FatherLastName2").focus();
        notifyDanger("Father 2 Last Name is required.");
        return false;
    }

    if ($("#Father2DOB").val() != '' && $("#FatherLastName2").val() == '') {
        $("#FatherLastName2").focus();
        notifyDanger("Father 2 Last Name is required.");
        return false;
    }


    
    var params = getData();

    if ($('#AddNew').val() == '1') {
        $.ajax({
            type: "POST", url: '/CaseOpening/InsertCase', data: params,
            success: function (result) {
                if (result.Status == 'Done') {
                    document.location.href = result.URL;
                }
            },
            dataType: 'json'
        });
    }
    else {
        $("#btnBypassAndAdd").addClass('hide');
        $.ajax({
            type: "POST", url: '/CaseOpening/AddApptCase', data: params,
            success: function (data) {

                if (data.Status == "Done") {
                    $("#btnBypassAndAdd").removeClass('hide');
                    setData(data.SearchData);
                } else if (data.Status == "CaseRedirect") {
                    document.location.href = data.URL;
                }
                // SavePageState(data);
            },
            dataType: 'json'
        });
    }
}

function setData(data) {
    oTable.fnClearTable();
    if (data.data.length > 0) {
        oTable.fnAddData(data.data);
        $('#CountSearchResult').text(data.data.length);
        fitCalculatedHeightForSearchDataTable();
    } else {
        $('#CountSearchResult').text('0');
        //   Notify('No results found.', 'bottom-right', '5000', 'blue', 'fa-frown-o', true);
    }
}

function getData() {
    var fData = $('#case-search-form').serialize();
    return fData;
}


$('#reset').on('click', function (e) {
    e.preventDefault();
    var $formErrorContainer = $('#search-validation-error');
    $formErrorContainer.addClass('hidden');
    $('#case-search-form *').val('');
    $('#CountSearchResult').text('0');
    $('#case-search-form input:text:first').focus();
    ResetPageState();
    oTable.fnClearTable();
});
$(window).bind('resize', function () {
    fitCalculatedHeightForSearchDataTable();
});
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
        $('#case-search-form *').filter(':input').each(function () {
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

