
var oTable = null;
var origin_wrapper_height = 0, origin_content_height = 0;

function changeFont(elem, ratio) {
    var font_size = $(elem).css('font-size');
    //var font_size = "1.35px";
    var regex = /[\d.]+/gi;
    var match = regex.exec(font_size);
    var size = parseFloat(match[0]) * ratio;
    var unit = font_size.substring(regex.lastIndex);
    var changedFontSize = size.toString() + unit;
    console.log(match);
    console.log(size);
    console.log(unit);
    console.log(changedFontSize);
    $(elem).css('font-size', changedFontSize);
    if (oTable != null)
        oTable.fnAdjustColumnSizing();
}

function fitCalculatedHeightForSearchDataTable() {
    var calc_height = 0;
    if (oTable != null) {
        calc_height = $(window).height();
        var _offset = 25;
        origin_wrapper_height = $('body>div.container-fluid').height();
        origin_content_height = $('#searchCaseAdvancedSearchPanelTableDiv .dataTables_scrollBody').height();

        $("#searchCaseAdvancedSearchPanelTableDiv .dataTables_scrollBody").children().first().parentsUntil("body").each(function () {
            $(this).siblings().each(function () {
                if (calc_height > $(this).outerHeight(true) && $(this).css('display') != 'none') {
                    // console.log(calc_height + " - " + $(this).outerHeight(true));
                    if ($(this).attr("id") == 'loading')
                        return;
                    calc_height = calc_height - $(this).outerHeight(true);
                }
            });
            _offset = _offset + $(this).outerHeight(true) - $(this).height();
        });

        // console.log("calc :" + calc_height + " offset: " + _offset);
        calc_height = calc_height - _offset;
        // console.log("total: " + calc_height);
        $('#searchCaseAdvancedSearchPanelTableDiv .dataTables_scrollBody').css('max-height', calc_height + 'px');


        oTable.fnAdjustColumnSizing();
    }
    return calc_height;
}

function ShowParametersDiv() {
    /*$('#searchCaseAdvancedSearchParameters').fadeIn(100, function () {
       
    });*/
    $('#searchCaseAdvancedSearchParameters').show();
    fitCalculatedHeightForSearchDataTable();
    var moreDetails = "<div class=" + '"' + "widget-buttons" + '"' + "><a data-toggle=" + '"' + "collapse" + '"' + "title=" + '"' + "More-Details" + '"' + "onclick=" + '"' + "ShowParametersDiv()" + '"' + " style=" + '"' + "cursor:pointer" + '"' + "><i class=" + '"' + "fa fa-plus danger" + '"' + "></i></a></div>";
    var lessDetails = "<div class=" + '"' + "widget-buttons" + '"' + "><a data-toggle=" + '"' + "collapse" + '"' + "title=" + '"' + "Lesser-Details" + '"' + "onclick=" + '"' + "HideParametersDiv()" + '"' + " style=" + '"' + "cursor:pointer" + '"' + "><i class=" + '"' + "fa fa-minus danger" + '"' + "></i></a></div>"; "<a title=" + '"' + "Less-Details" + '"' + "onclick=" + '"' + "HideParametersDiv()" + '"' + " style=" + '"' + "cursor:pointer" + '"' + " id=" + '"' + "zoom-in" + '"' + ">" + "<i class=" + '"' + "fa fa-minus" + '"' + "></i>" + "</a>"
    $(".search-criteria").children("div").remove();
    $(".search-criteria").append(lessDetails);
};
function HideParametersDiv() {
    /*$('#searchCaseAdvancedSearchParameters').fadeOut(100, function () {
        
    });*/
    $('#searchCaseAdvancedSearchParameters').hide();
    fitCalculatedHeightForSearchDataTable();
    $(".search-criteria").children("div").remove();
    var moreDetails = "<div class=" + '"' + "widget-buttons" + '"' + "><a data-toggle=" + '"' + "collapse" + '"' + "title=" + '"' + "More-Details" + '"' + "onclick=" + '"' + "ShowParametersDiv()" + '"' + " style=" + '"' + "cursor:pointer" + '"' + "><i class=" + '"' + "fa fa-plus danger" + '"' + "></i></a></div>";
    document.getElementById("searchCaseAdvancedSearchPanelTableDiv").style.height = ($(window).height() - $('#searchCaseAdvancedSearchParameters').height());
    $(".search-criteria").append(moreDetails);
};


$(document).ready(function () {
    $('#HearingTime').val('');
    var moreDetails = "<div class=" + '"' + "widget-buttons" + '"' + "><a data-toggle=" + '"' + "collapse" + '"' + "title=" + '"' + "More-Details" + '"' + "onclick=" + '"' + "ShowParametersDiv()" + '"' + " style=" + '"' + "cursor:pointer" + '"' + "><i class=" + '"' + "fa fa-plus danger" + '"' + "></i></a></div>";
    var lessDetails = "<div class=" + '"' + "widget-buttons" + '"' + "><a data-toggle=" + '"' + "collapse" + '"' + "title=" + '"' + "Lesser-Details" + '"' + "onclick=" + '"' + "HideParametersDiv()" + '"' + " style=" + '"' + "cursor:pointer" + '"' + "><i class=" + '"' + "fa fa-minus danger" + '"' + "></i></a></div>"; "<a title=" + '"' + "Less-Details" + '"' + "onclick=" + '"' + "HideParametersDiv()" + '"' + " style=" + '"' + "cursor:pointer" + '"' + " id=" + '"' + "zoom-in" + '"' + ">" + "<i class=" + '"' + "fa fa-minus" + '"' + "></i>" + "</a>"

    var page = (function () {
        return {
            $form: $('#case-search-form-advanced'),
            $table: $('#searchCaseAdvanced'),
            mainKey: 'case-search-form-advanced' + GetWindowID(),
            resultPageIdKey: GetWindowID() + 'case-search-form-advanced-page#',
            rowSelectedKey: GetWindowID() + 'case-search-form-advanced-row#',
            ttl: 600000 /*In Milli seconds*/
        };
    })();

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

    var height = $('#searchCaseAdvancedSearchPanel').height();
    if (parseInt(height) < parseInt($(window).height())) {
        if (parseInt($(window).height()) >= 950 && parseInt($(window).height()) <= 1024) {
            height = 800;
        }
        else if (parseInt($(window).height()) >= 760 && parseInt($(window).height()) < 950) {
            height = 500;
        }
        else {
            height = parseInt($(window).height()) - $('#searchCaseAdvancedSearchParameters').height();
        }
    }
    if ($('#searchCaseAdvancedSearchPanel').height() >= $(window).height()) {
        if (parseInt($(window).height()) >= 590 && parseInt($(window).height()) < 760) {
            height = 400;
        }
        else {
            height = parseInt($(window).height()) - $('#searchCriteria').height();
        }
    }

    oTable = $('#searchCaseAdvanced').dataTable({

        "scrollY": "auto",
        "scrollCollapse": true,
        "paging": false,
        "searching": false,
        "bSort": false, "columns": [
                { "data": "PersonNameLast" },
                { "data": "PersonNameFirst" },
                { "data": "PersonDob" },
                { "data": "Sex", "title": "Gender" },
                { "data": "Role" },
                { "data": "HHSANumber" },
                {
                    "render": function (data, type, full, meta) {
                        //return ('<a href="/Case/Main/' + full.EncryptedCaseID + '?fromSearch=2" class="lnkCaseNumber">' + full.PetitionDocketNumber + '</a>')
                        if (hasUserAccessToSecurityItemId('1500')) {
                            return '<span>' + full.PetitionDocketNumber + '</span>';
                        } else {
                        return ('<a href="/Case/Main/' + full.EncryptedCaseID + '" class="lnkCaseNumber"  data-secure-linkremovediffound-id="1500">' + full.PetitionDocketNumber + '</a>')
                        }
                  
                    }
                },
                {
                    "render": function (data, type, full, meta) {
                        return ('<a href="#">' + full.casecloseddate + '</a>')

                    }
                },
                { "data": "CaseNumber" },
                { "data": "LeadAttorney" },
        ],
        "loadingRecords": "Loading...",
        "processing": "Processing...",
        "fnDrawCallback": function (oSettings) {
            $("a.lnkCaseNumber").on('click', onCaseNumberClick);
        },
        "deferRender": true
    });

    if (device.mobile() || device.tablet()) {
        var middleColumn = oTable.DataTable().column(2);
        middleColumn.visible(false);
    }

    if (simpleStorage.get(page.mainKey)) {
        LoadPreviousPageState();
    }
    else {
        $('#HearingTime').val('');
        ///*Load Default state of the form*/
        //ResetPageState();
        //var data = getData();
        //$.ajax({ type: "POST", url: '/Case/SearchForCaseAdvanced', data: data, success: function (data) { setData(data); SavePageState(data); }, dataType: 'json' });
    }


    $('#search').on('click', function () {
        var $form = $('#case-search-form-advanced');
        $form.submit();
    });

    $(window).bind('resize', function () {
        $('#searchCaseAdvanced').css('width', '100%');
        fitCalculatedHeightForSearchDataTable();
    });


    $('#case-search-form-advanced').on('submit', function (e) {
        IPadKeyboardFix();
        ResetPageState();
        e.preventDefault();
        var $form = $('#case-search-form-advanced');
        var formInvalid = $('#ClientLastName').val().length == 0
            && $('#ClientFirstName').val().length == 0
            && $('#ClientDOB').val().length == 0
            && $('#ClientAddress').val().length == 0
            && $('#ClientCity').val().length == 0
            && $('#ClientStateID').val().length == 0
            && $('#ClientZip').val().length == 0
            && $('#ClientPhoneNumber').val().length == 0
            && $('#ClientSSN').val().length == 0
            && $('#BookingNumber').val().length == 0
            && $('#PetitionNumber').val().length == 0
            && $('#CountyCounselNumber').val().length == 0
            && $('#HHSANumber').val().length == 0
            && $('#InmateNumber').val().length == 0
            && $('#CaseNumber').val().length == 0
            && $('#ChildLastName').val().length == 0
            && $('#ChildFirstName').val().length == 0
            && $('#ChildFirstName').val().length == 0
            && $('#ChildDOB').val().length == 0
            && $('#ParentLastName').val().length == 0
            && $('#ParentFirstName').val().length == 0
            && $('#ParentDOB').val().length == 0
            && $('#ParentAddress').val().length == 0
            && $('#ParentCity').val().length == 0
            && $('#ParentStateID').val().length == 0
            && $('#ParentZip').val().length == 0
            && $('#ParentPhone').val().length == 0
            && $('#ParentSSN').val().length == 0
            && $('#AgencyAttorneyID').val().length == 0
            && $('#InvestigatorID').val().length == 0
            && $('#LegalAssistantID').val().length == 0
            && $('#RoleStatus').val().length == 0
            && $('#CaretakerLastName').val().length == 0
            && $('#CaretakerFirstName').val().length == 0
            && $('#CaretakerAddress').val().length == 0
            && $('#CaretakerPhoneNumber').val().length == 0
            && $('#SocialWorkerLastName').val().length == 0
            && $('#SocialWorkerFirstName').val().length == 0
            && $('#SocialWorkerPhoneNumber').val().length == 0
            && $('#HearingTypeID').val().length == 0
            && $('#HearingDate').val().length == 0
            && $('#HearingTime').val().length == 0
            && $('#HearingOfficerID').val().length == 0
            && $('#HearingDepartmentID').val().length == 0
            && $('#CaseDepartmentID').val().length == 0
            && $('#AllegationID').val().length == 0
            && $('#CaseStatus').val().length == 0
            && $('#PetitionStatus').val().length == 0
            && $('#LanguageID').val().length == 0
            && $('#ParentalRightsTerminationDate').val().length == 0
            && $('#PetitionTypeID').val().length == 0
            && $('#ReportFilingDueTypeID').val().length == 0;

        if (formInvalid) {
            notifyDanger('At least one search parameter is required.');
            return false;
        }
        if (!IsInvalidCharactersNotExistsInSearchFields($('#case-search-form-advanced')))
            return false;
        var data = getData();
        $.ajax({ type: "POST", url: '/Case/SearchForCaseAdvanced', data: data, success: function (data) { setData(data); SavePageState(data); }, dataType: 'json' });
    });

    function setData(data) {
        oTable.fnClearTable();
        if (data.data != undefined && data.data.length > 0) {
            oTable.fnAddData(data.data);
            /*$('#searchCaseAdvancedSearchParameters').fadeOut(400, function () {
                
            });*/
            $('#searchCaseAdvancedSearchParameters').hide();
            fitCalculatedHeightForSearchDataTable();
            origin_wrapper_height = $('body>div.container-fluid').height();
            origin_content_height = $('#searchCaseAdvancedSearchPanelTableDiv .dataTables_scrollBody').height();
            $(".search-criteria").children("div").remove();
            $(".search-criteria").append(moreDetails);
        }
        else {
            if (data.ErrorMessage != undefined && data.ErrorMessage != "") {
                notifyDanger(data.ErrorMessage);
            }
            else {
                $(".search-criteria").children("div").remove();
                $(".search-criteria").append(lessDetails);
                notifyBlue('No results found.');
            }
        }
    }

    function getData() {
        var fData = $('#case-search-form-advanced').serialize();
        return fData;
    }

    $('#reset').on('click', function (e) {
        e.preventDefault();
        var $formErrorContainer = $('#search-validation-error');
        $('#case-search-form-advanced input:text:first').focus();
        $('#case-search-form-advanced *').val('');
        ResetPageState();
        oTable.fnClearTable();
        //$('#searchCaseAdvancedSearchParameters').fadeIn();
        $('#searchCaseAdvancedSearchParameters').show();
        $(".search-criteria").children("div").remove();
        $(".search-criteria").append(lessDetails);
    });

    //function onCaseNumberClick(e) {
    //    var $this = $(this);
    //    e.preventDefault();
    //    simpleStorage.set(page.rowSelectedKey, $this.closest('tr').index(), { TTL: page.ttl });
    //    document.location = $this.attr('href');
    //}


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
            $('#case-search-form-advanced *').filter(':input').each(function () {
                var $this = $(this);
                if (formData[$this.attr('id')])
                    $this.val(formData[$this.attr('id')]);
            });

            /*Highlight selected Row*/
            var rowIndex = simpleStorage.get(page.rowSelectedKey);
            if (rowIndex > 0) {
                page.$table.find("tr:nth-child(" + (rowIndex + 1) + ")").css("background", "#f3f7b4");
            }
        }
        else {
            $(".search-criteria").children("div").remove();
            $(".search-criteria").append(lessDetails);
        }
    };

    function onCaseNumberClick(e) {
        var $this = $(this);
        e.preventDefault();
        simpleStorage.set(page.rowSelectedKey, $this.closest('tr').index(), { TTL: page.ttl });
        document.location = $this.attr('href');
    }
});

