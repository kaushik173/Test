$BaseURL = '/';
$ReportID = '';
var origin_wrapper_height = 0, origin_content_height = 0;
var page = (function () {
    return {
        $form: $('#myCaseLoad-form'),

        mainKey: 'myCaseLoadDataV2' + GetWindowID(),

        ttl: 600000 /*In Milli seconds*/
    };
})();
$('#btnSearch').on('click', function (e) {
    e.preventDefault();
    IPadKeyboardFix();
    if ($('#AgeRangeStart').val() !== '' && $('#AgeRangeEnd').val() !== '' && parseFloat($('#AgeRangeStart').val()) > parseFloat($('#AgeRangeEnd').val())) {
        notifyDanger("Age Range, start Age cannot be greater than end Age");
        $('#AgeRangeStart').focus()
        return false;
    }
    ResetPageState();
    $('#myCaseLoad-form').submit();

});

$('#myCaseLoad-form').on('submit', function (e) {
    e.preventDefault();

    var $form = $('#myCaseLoad-form');
    var fData = $form.serialize();
   

    if ($ReportID !== '') {
        fData += '&ReportID=' + $ReportID
    }
    $.ajax({
        type: "POST", url: '/Inquiry/MyCaseLoadV2', data: fData, success: function (data) {
            $('#myCaseLoadData').html(data);
            SavePageState(fData);
            HideParametersDiv();
            $('#spanHeaderTextMain').text($('#spanHeaderText').text())
            if ($ReportID !== '') {
                var $form = $('#myCaseLoad-form');
                var fData = $form.serialize();

                var _target = $("body").data("print-document-on") == "NewWindow" ? 'target="_blank"' : '';

                $.download($('#hdnCurrentSessionGuidPath').val() + '/Inquiry/PrintMyCaseloadV2', fData, "POST", _target);
                $ReportID = ''
            }
            $ReportID = ''
        }
    });
});
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
function LoadPreviousPageState() {
    var pageState = simpleStorage.get(page.mainKey);
    if (pageState && pageState.formData && pageState.results) {



        /*Load Form inputs*/
        var formData = pageState.formData;
        $('#myCaseLoad-form *').filter(':input').each(function () {
            var $this = $(this);
            if ($this.attr('type') == 'checkbox') {
                if (formData[$this.attr('id')])
                    $this.prop('checked', formData[$this.attr('id')] == 'true');
            } else {
                if (formData[$this.attr('id')])
                    $this.val(formData[$this.attr('id')]);
            }

        });


    }

};
$('#btnPrint').on('click', function () {
    IPadKeyboardFix();
    if ($('#AgeRangeStart').val() !== '' && $('#AgeRangeEnd').val() !== '' && parseFloat($('#AgeRangeStart').val()) > parseFloat($('#AgeRangeEnd').val())) {
        notifyDanger("Age Range, start Age cannot be greater than end Age");
        $('#AgeRangeStart').focus()
        return false;
    }
    ResetPageState();
    $ReportID='132'
    $('#myCaseLoad-form').submit();
     
    
});

$(window).bind('resize', function () {
    fitCalculatedHeightForSearchDataTable();
});

function fitCalculatedHeightForSearchDataTable() {
    var calc_height = 0;
    calc_height = $(window).height();
    var _offset = 25;
    origin_wrapper_height = $('body>div.container-fluid').height();
    origin_content_height = $('#divMyCaseLoadReult .table-responsive').height();

    $("#divMyCaseLoadReult .table-responsive").children().first().parentsUntil("body").each(function () {

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
    $('#divMyCaseLoadReult .table-responsive').css('max-height', calc_height + 'px');

    return calc_height;
}

function ShowParametersDiv() {
    /*$('#searchCaseAdvancedSearchParameters').fadeIn(100, function () {
       
    });*/
    $('#searchCaseAdvancedSearchParameters').show();
    fitCalculatedHeightForSearchDataTable();
    var moreDetails = "<div class=" + '"' + "widget-buttons " + '"' + "><button data-toggle=" + '"' + "collapse" + '"' + "title=" + '"' + "More-Details" + '"' + "onclick=" + '"' + "ShowParametersDiv()" + '"' + " style=" + '"' + "cursor:pointer" + '"' + " class='btn btn-labeled btn-default btn'>Show Criteria</button></div>";
    var lessDetails = "<div class=" + '"' + "widget-buttons  " + '"' + "><button data-toggle=" + '"' + "collapse" + '"' + "title=" + '"' + "Lesser-Details" + '"' + "onclick=" + '"' + "HideParametersDiv()" + '"' + " style=" + '"' + "cursor:pointer" + '"' + " class='btn btn-labeled btn-default btn'>Hide Criteria</button></div>"; "<button title=" + '"' + "Less-Details" + '"' + "onclick=" + '"' + "HideParametersDiv()" + '"' + " style=" + '"' + "cursor:pointer" + '"' + " id=" + '"' + "zoom-in" + '"' + " class='btn btn-labeled btn-default btn'>" + "Hide Criteria" + "</button>"

    var reset = " <button data-toggle=" + '"' + "collapse" + '"' + "title=" + '"' + "Lesser-Details" + '"' + "onclick=" + '"' + "ResetCriteria()" + '"' + " style=" + '"' + "cursor:pointer" + '"' + " class='btn btn-labeled btn-default btn'>Reset Criteria</button>";
    $(".search-criteria").children("div").remove();
    $(".search-criteria").append(lessDetails);
    $(".search-criteria .widget-buttons").append(reset);
};
function HideParametersDiv() {
    /*$('#searchCaseAdvancedSearchParameters').fadeOut(100, function () {
        
    });*/
    $('#searchCaseAdvancedSearchParameters').hide();
    fitCalculatedHeightForSearchDataTable();
    $(".search-criteria").children("div").remove();
    var moreDetails = "<div class=" + '"' + "widget-buttons " + '"' + "><button data-toggle=" + '"' + "collapse" + '"' + "title=" + '"' + "More-Details" + '"' + "onclick=" + '"' + "ShowParametersDiv()" + '"' + " style=" + '"' + "cursor:pointer" + '"' + " class='btn btn-labeled btn-default btn'>Show Criteria</button></div>";
    document.getElementById("divMyCaseLoadReult").style.height = ($(window).height() - $('#searchCaseAdvancedSearchParameters').height());
    $(".search-criteria").append(moreDetails);
    var reset = " <button data-toggle=" + '"' + "collapse" + '"' + "title=" + '"' + "Lesser-Details" + '"' + "onclick=" + '"' + "ResetCriteria()" + '"' + " style=" + '"' + "cursor:pointer" + '"' + " class='btn btn-labeled btn-default btn'>Reset Criteria</button>";
    $(".search-criteria .widget-buttons").append(reset);

};

function ResetCriteria() {

    ResetPageState();
    document.location.href = document.location.href.replace('#', '');
}


$(document).ready(function () {
    LoadPreviousPageState();
    if ($('#PersonID').val() == "0" && $onViewLoad != "True") {

        return false;
    }
    else {
        $('#myCaseLoad-form').submit();
    }
});

$('#SortByID').on('change', function () {

    $('#myCaseLoad-form').submit();
});

$('input:checkbox').on('change', function () {

    $(this).attr('value', $(this).is(':checked'));
    $('input[name="' + $(this).attr('id') + '"]').attr('value', $(this).is(':checked'));
});
